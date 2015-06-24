using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * The Fog of War is instantiated as an XZ plane that you should position
 * above your scene--for best effect, put it as close to your scene as
 * you can without having anything climb above the clouds (because that
 * looks dumb).  There is one place in this code where the Fog of War
 * has to adjust its behavior based on the height of your terrain: as
 * a default behavior it simply guesses Y=0, but you can search for the
 * word "ground" in this source file to find the relevant code and then
 * plug in any special code you want there to get better behavior.
 *
 * By itself, that Fog of War plane is just a bunch of clouds--prettyish
 * ones, true, but also boring.  To actually use it as a fog of war,
 * just instantiate Beacon objects on the map--and they'll create holes
 * in the clouds through which you can see.  (For an interesting effect,
 * you can also set the default cloud opacity lower and then use some
 * negative-strength beacons, which will draw clouds around them.)
 *
 * This model is designed to provide very low performance cost during
 * normal rendering: instead, a performance penalty is assessed at most
 * once per frame, and then only on frames when a beacon is changed.
 * Further, you can divide your beacons into Static and Dynamic ones,
 * so that changes to Dynamic beacons will not require recalculating the
 * Static beacons--and so the performance cost of the fog of war can
 * be further reduced.
 *
 */

public class FogOfWar : MonoBehaviour
{
    /*
     * The CloudStrength property indicates how visible the clouds are
     * on the Fog of War plane by default, wherever there are no beacons
     * affecting the view.  A value of 0 means the clouds are completely
     * absent, and 1 means they completely obscure anything underneath.
     */

    [Range(0, 1)]
    public float CloudStrength = 1.0f;


    /*
     * When a new beacon appears or an old one disappears, or even when
     * an existing beacon changes properties (location, strength etc),
     * the Fog of War changes to accomodate the new visibility pattern.
     * In most cases the shader is able to gracefully transition between
     * the old view and the new one over a short period of time, expressed
     * in seconds.
     */

    public float ChangeTime = 0.5f;


    /*
     * The Fog of War uses lazy updating: on most display frames it does
     * no thinking at all, just renders the shader using precalculated
     * data.  On any frame when a beacon changes (position, strength, etc)
     * we flag that corresponding list of beacons as needing to be studied
     * to update the instructions for the shader.
     */

    public void Invalidate(BeaconType type)
    {
        if (type == BeaconType.Static)
        {
            _recalcStaticViewport = true;
        }
        _recalcSummaryViewport = true;
    }

    private bool _recalcStaticViewport = true;
    private bool _recalcSummaryViewport = true;


    /*
     * This constant is pretty important: it indicates the size of the
     * Viewport textures that we calculate.  Higher numbers will burn a
     * lot more CPU whenever we have to rebuild a viewport (which happens
     * on any frame in which a beacon changes), while lower numbers will
     * produce a more jagged appearance to the cloud cover around the
     * edges of a beacon.  I'd recommend using as low a constant here
     * as you can visually get away with; 128 is a good general-purpose
     * value that seems to work for most scenes, so it's a good default.
     */

    public const int Precision = 128;


    /*
     * There are two kinds of beacons: static and dynamic.  Functionally
     * they're the same--you can move or change the properties of any
     * beacon at any time and everything will work fine.  They're separated
     * because when you change a static beacon we have to think hard about
     * every beacon in your scene--but when you change a dynamic beacon, we
     * only have to think hard about the other dynamic beacons.  So if you
     * have some beacons that aren't going to change much, make them Static
     * and you'll have to do less thinking when the more mobile Dynamic
     * beacons change.
     */

    public List<Beacon> StaticBeacons = new List<Beacon>();
    public List<Beacon> DynamicBeacons = new List<Beacon>();


    /*
     * The cloud image shown by the shader involves one image, but
     * we'll interpolate between two versions of it: one regular and
     * one reversed--and the two images will move at different rates,
     * to make the clouds look a little more flowing and billowy.
     * These values represent the speeds at which those cloud layers
     * move, and their current positions.
     */

    private const float RollLeftSpeed1 = 0.04f;
    private const float RollUpSpeed1 = 0.025f;
    private const float RollLeftSpeed2 = 0.03f;
    private const float RollUpSpeed2 = 0.015f;

    private float RollLeft1 = 0;
    private float RollUp1 = 0;
    private float RollLeft2 = 500;
    private float RollUp2 = 0;


    /*
     * The Viewport class represents a 2D image summarizing where all
     * the beacons are in the world.  We'll have one Viewport that
     * summarizes all the static beacons, and a second one that
     * summarizes all the static beacons plus all the dynamic beacons.
     */

    internal class Viewport : IDisposable
    {
        public Rect Area = new Rect(0, 0, 0, 0);
        public Texture2D Map = null;
        public bool IsEmpty = true;


        void Expand(Vector3 position, float range)
        {
            // Expand a *little* more than we really need to, so the edges stay clean
            range *= 1.1f;
            if (IsEmpty)
            {
                Area = new Rect(position.x - range, position.z - range, range * 2, range * 2);
            }
            else
            {
                Area.xMin = Math.Min(Area.xMin, position.x - range);
                Area.yMin = Math.Min(Area.yMin, position.z - range);
                Area.xMax = Math.Max(Area.xMax, position.x + range);
                Area.yMax = Math.Max(Area.yMax, position.z + range);
            }
            IsEmpty = false;
        }

        public Vector4 ToVector4()
        {
            return new Vector4(Area.xMin, Area.yMin, Area.xMax, Area.yMax);
        }

        public void Dispose()
        {
            if (Map != null)
            {
                Texture2D.Destroy(Map);
                Map = null;
                IsEmpty = true;
            }
        }


        public void DrawMap(List<Beacon> list, Viewport baseViewport, bool isRequired, float cloudStrength)
        {
            foreach (Beacon beacon in list)
            {
                Expand(beacon.Position, beacon.Range);
            }
            if (IsEmpty && !isRequired)
            {
                return;
            }

            if (baseViewport != null && baseViewport.IsEmpty)
            {
                baseViewport = null;
            }
            if (baseViewport != null)
            {
                if (IsEmpty)
                {
                    Area = baseViewport.Area;
                }
                else
                {
                    Area.xMin = Math.Min(Area.xMin, baseViewport.Area.xMin);
                    Area.yMin = Math.Min(Area.yMin, baseViewport.Area.yMin);
                    Area.xMax = Math.Max(Area.xMax, baseViewport.Area.xMax);
                    Area.yMax = Math.Max(Area.yMax, baseViewport.Area.yMax);
                }
            }

            Color[] colors = new Color[FogOfWar.Precision * FogOfWar.Precision];

            float baseAlpha = cloudStrength;

            int index = 0;
            for (int yy = 0; yy < FogOfWar.Precision; ++yy)
            {
                for (int xx = 0; xx < FogOfWar.Precision; ++xx, ++index)
                {
                    float wx = Area.xMin + xx * Area.width / FogOfWar.Precision;
                    float wz = Area.yMin + yy * Area.height / FogOfWar.Precision;

                    float opaqued = 0;
                    float revealed = 0;
                    if (baseViewport != null)
                    {
                        float ux = (wx - baseViewport.Area.xMin) / (baseViewport.Area.width);
                        float vy = (wz - baseViewport.Area.yMin) / (baseViewport.Area.height);
                        if (ux >= 0 && ux <= 1 && vy >= 0 && vy <= 1)
                        {
                            Color baseColor = baseViewport.Map.GetPixelBilinear(ux, vy);
                            opaqued = baseColor.r;
                            revealed = baseColor.g;
                        }
                    }

                    foreach (Beacon beacon in list)
                    {
                        float dx = Math.Abs(beacon.Position.x - wx);
                        float dz = Math.Abs(beacon.Position.z - wz);
                        if (dx < beacon.Range && dz < beacon.Range)
                        {
                            float dist = Mathf.Sqrt(dx * dx + dz * dz);
                            if (dist < beacon.Range)
                            {
                                float away = dist / beacon.Range;
                                float power = beacon.Strength * (1 - away * away);
                                if (power > 0)
                                {
                                    revealed = Mathf.Max(revealed, power);
                                }
                                else
                                {
                                    opaqued = Mathf.Max(opaqued, 0 - power);
                                }
                            }
                        }
                    }

                    colors[index].r = Mathf.Clamp(opaqued, 0, 1);
                    colors[index].g = Mathf.Clamp(revealed, 0, 1);
                    colors[index].b = 0;

                    float visible = revealed - opaqued;
                    if (visible > 0)
                    {   // visible
                        colors[index].a = Mathf.Lerp(baseAlpha, 0, visible);
                    }
                    else
                    {
                        colors[index].a = Mathf.Lerp(baseAlpha, 1, 0 - visible);
                    }
                }
            }

            Map = new Texture2D(FogOfWar.Precision, FogOfWar.Precision);
            Map.SetPixels(colors);
            Map.Apply();
        }
    }

    private Viewport _currentStaticViewport = null;
    private Viewport _currentSummaryViewport = null;


    /*
     * If you've recently moved some beacons, we'll attempt to fade
     * gracefully from the prior summary viewport to the current one.
     * The shader will interpolate between this old summary viewport
     * to the current summary viewport over a selectable period of time.
     */

    private float _transitionStartTime = 0;
    private Viewport _oldSummaryViewport = null;
    private float _oldCloudStrength = 0;


    /*
     * If you're using the beacon prefabs and StaticBeacon/DynamicBeacon
     * scripts, then you'll need to instantiate a FogOfWar object in your
     * scene hierarchy using the name "FogOfWar" since those scripts look
     * for that object name to figure out which FogOfWar to affect.
     */

    public static FogOfWar FindExisting
    {
        get
        {
            GameObject obj = GameObject.Find("FogOfWar");
            if (obj != null && obj.activeInHierarchy)
            {
                return obj.GetComponent<FogOfWar>();
            }
            return null;
        }
    }


    /*
     * Your fog of war should automatically adjust as you add, remove
     * or update beacons within it.
     */

    public void AddBeacon(Beacon beacon)
    {
        if (beacon.Type == BeaconType.Static)
        {
            StaticBeacons.Add(beacon);
        }
        else
        {
            DynamicBeacons.Add(beacon);
        }
        Invalidate(beacon.Type);
    }

    public void RemoveBeacon(Beacon beacon)
    {
        if (beacon.Type == BeaconType.Static)
        {
            StaticBeacons.Remove(beacon);
        }
        else
        {
            DynamicBeacons.Remove(beacon);
        }
        Invalidate(beacon.Type);
    }

    public void UpdateBeacon(Beacon beacon)
    {
        Invalidate(beacon.Type);
    }


    /*
     * The important bit: this method is invoked on every frame, and
     * is an opportunity for us to tell the Fog of War shader how it
     * should do its work.
     */

    void Update()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        Material mat = GetComponent<MeshRenderer>().material;

        /*
         * Roll the cloud textures and tell the shader how strongly
         * they should appear (except where beacon map says otherise)
         */

        mat.SetFloat("Strength", CloudStrength);
        mat.SetFloat("CosFactor", 0.50f + Mathf.Cos(Time.time / 2f) * 0.30f);

        RollLeft1 -= RollLeftSpeed1 * Time.deltaTime;
        RollUp1 -= RollUpSpeed1 * Time.deltaTime;
        RollLeft2 -= RollLeftSpeed2 * Time.deltaTime;
        RollUp2 -= RollUpSpeed2 * Time.deltaTime;

        mat.SetFloat("RollLeft1", RollLeft1);
        mat.SetFloat("RollUp1", RollUp1);
        mat.SetFloat("RollLeft2", RollLeft2);
        mat.SetFloat("RollUp2", RollUp2);

        /*
         * If the overall cloud strength has changed, we have to
         * rebuild our viewports so the shader's viewport region
         * will react to the new conditions.
         */

        if (CloudStrength != _oldCloudStrength)
        {
            _recalcStaticViewport = true;
            _recalcSummaryViewport = true;
            _oldCloudStrength = CloudStrength;
        }

        /*
         * Update the viewports if the beacons have changed.  We have
         * two possible viewports to update, since we don't want to
         * have to rethink all the static becaons just because a
         * dynamic beacon changed.
         */

        if (_recalcStaticViewport || _recalcSummaryViewport)
        {
            if (_oldSummaryViewport != null)
            {
                _oldSummaryViewport.Dispose();
            }
            _oldSummaryViewport = _currentSummaryViewport;
            _currentSummaryViewport = null;
        }

        /*
         * First step: if the static beacons have changed, then we
         * have to rebuild the static viewport.
         */

        if (_recalcStaticViewport)
        {
            if (_currentStaticViewport != null)
            {
                _currentStaticViewport.Dispose();
            }
            _currentStaticViewport = new Viewport();
            _currentStaticViewport.DrawMap(StaticBeacons, null, false, CloudStrength);
        }

        if (_recalcSummaryViewport)
        {
            if (_currentSummaryViewport != null)
            {
                _currentSummaryViewport.Dispose();
            }
            _currentSummaryViewport = new Viewport();
            _currentSummaryViewport.DrawMap(DynamicBeacons, _currentStaticViewport, true, CloudStrength);
        }

        /*
         * Push the new viewport into the shader, and then we're done with
         * updating the viewports.  Whew.
         */

        if (_recalcStaticViewport || _recalcSummaryViewport)
        {
            if (_currentStaticViewport.IsEmpty && _currentSummaryViewport.IsEmpty)
            {
                mat.SetVector("CurrentViewport", new Vector4(0, 0, 0, 0));
                mat.SetTexture("CurrentTexture", null);
            }
            else
            {
                mat.SetVector("CurrentViewport", _currentSummaryViewport.ToVector4());
                mat.SetTexture("CurrentBeaconMap", _currentSummaryViewport.Map);

                if (_oldSummaryViewport == null)
                {
                    _transitionStartTime = 0;
                    mat.SetVector("PreviousViewport", new Vector4(0, 0, 0, 0));
                    mat.SetTexture("PreviousBeaconMap", null);
                }
                else
                {
                    _transitionStartTime = Time.time;
                    mat.SetVector("PreviousViewport", _oldSummaryViewport.ToVector4());
                    mat.SetTexture("PreviousBeaconMap", _oldSummaryViewport.Map);
                }
            }
        }

        _recalcStaticViewport = false;
        _recalcSummaryViewport = false;

        /*
         * If we're still transitioning from an old viewport to a new one,
         * update the lerp value.
         */

        float deltaTime = Time.time - _transitionStartTime;
        if (deltaTime > ChangeTime)
        {
            _transitionStartTime = 0;
        }
        if (_transitionStartTime == 0)
        {
            mat.SetTexture("PreviousBeaconMap", null);
            mat.SetFloat("PreviousPercent", 0);
        }
        else
        {
            mat.SetTexture("PreviousBeaconMap", _oldSummaryViewport.Map);
            mat.SetFloat("PreviousPercent", (ChangeTime - deltaTime) / ChangeTime);
        }

        /*
         * Now the hard part: we need to reposition the fog-of-war plane.
         * The first step is to find the screen-space pixel in the middle
         * of the screen, and cast a ray from there down through the camera
         * until we hit the ground--so we can find out what world-space
         * pixel you're staring at directly.  Since we don't know the real
         * topology of your world, we'll use Y=0 as the touch plane;
         * you're welcome to insert your own logic here to find what
         * world-space coordinate is in the center of the camera.
         */

        float groundPlaneY = 0; // Pick your own value here

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        Plane ground = new Plane(new Vector3(0, 1, 0), new Vector3(0, groundPlaneY, 0));

        float rayDistance = 0;
        ground.Raycast(ray, out rayDistance);
        Vector3 worldCenter = ray.GetPoint(rayDistance);

        /*
         * Now things get a little easier: find out where that same ray
         * presently intersects our cloud layer.
         */

        Plane sky = new Plane(new Vector3(0, 1, 0), gameObject.transform.position);
        sky.Raycast(ray, out rayDistance);
        Vector3 skyImpact = ray.GetPoint(rayDistance);

        /*
         * Now slide the sky around so that the two line up. Child's play.
         */

        mat.SetVector("AdjustViewpoint", new Vector4((worldCenter.x - skyImpact.x), 0, (worldCenter.z - skyImpact.z), 0));
    }

    // Made by Linas.
    internal Viewport GetViewport()
    {
        return _currentSummaryViewport;
    }
}

