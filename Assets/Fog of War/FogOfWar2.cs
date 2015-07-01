﻿using UnityEngine;
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

public class FogOfWar2 : MonoBehaviour
{

    [Range(0, 1)]
    public float CloudStrength = 1.0f;
    public float ChangeTime = 0.5f;

    private const float RollLeftSpeed1 = 0.04f;
    private const float RollUpSpeed1 = 0.025f;
    private const float RollLeftSpeed2 = 0.03f;
    private const float RollUpSpeed2 = 0.015f;

    private float RollLeft1 = 0;
    private float RollUp1 = 0;
    private float RollLeft2 = 500;
    private float RollUp2 = 0;

    private Viewport visibleViewport = null;
    private Viewport oldViewport = null;
    private bool redraw = true;

    private class Viewport : IDisposable
    {
        public Rect Area = new Rect(0, 0, 0, 0);
        public Texture2D Map = null;
        public bool IsEmpty = true;

        public void Expand(Vector3 position, float range)
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
    }

    private Viewport DrawMap(List<Vector3> list, float strength, float range, Viewport baseViewport, float cloudStrength)
    {
        Viewport newViewport = new Viewport();
        if (baseViewport != null)
        {
            newViewport.Area = baseViewport.Area;
            newViewport.Map = baseViewport.Map;
            newViewport.IsEmpty = false;
        }
        foreach (Vector3 point in list)
        {
            newViewport.Expand(point, range);
        }

        Color[] colors = new Color[FogOfWar.Precision * FogOfWar.Precision];

        float baseAlpha = cloudStrength;

        int index = 0;
        // Texture is divided into 'Precision'^2 amount of pieces. For each piece:
        for (int yy = 0; yy < FogOfWar.Precision; ++yy)
        {
            for (int xx = 0; xx < FogOfWar.Precision; ++xx, ++index)
            {
                // TODO ???? wat is dis?:
                float wx = newViewport.Area.xMin + xx * newViewport.Area.width / FogOfWar.Precision;
                float wz = newViewport.Area.yMin + yy * newViewport.Area.height / FogOfWar.Precision;

                // Initialize the cell to not be revealed and not be opaque:
                float opaqued = 0;
                float revealed = 0;

                // If something was revealed before:
                if (baseViewport != null)
                {
                    // TODO Get the coordinates of the revealed area?:
                    float ux = (wx - baseViewport.Area.xMin) / (baseViewport.Area.width);
                    float vy = (wz - baseViewport.Area.yMin) / (baseViewport.Area.height);
                    // If not outside bounds, get base color, opaque and revealed values:
                    if (ux >= 0 && ux <= 1 && vy >= 0 && vy <= 1)
                    {
                        Color baseColor = baseViewport.Map.GetPixelBilinear(ux, vy);
                        opaqued = baseColor.r;
                        revealed = baseColor.g;
                    }
                }

                // For each fog change point:
                foreach (Vector3 point in list)
                {
                    // Get X and Y distances:
                    float dx = Math.Abs(point.x - wx);
                    float dz = Math.Abs(point.z - wz);

                    // For each point withing X, Y bounds:
                    if (dx < range && dz < range)
                    {
                        // Get distance; If within range:
                        float dist = Mathf.Sqrt(dx * dx + dz * dz);
                        if (dist < range)
                        {
                            // Power depends on distance, range, strength.
                            float away = dist / range;
                            float power = strength * (1 - away * away);
                            // Set revealed and opaque values to max of former values and new values:
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

                // Clamp the color to opaqueness and revealedness: 
                colors[index].r = Mathf.Clamp(opaqued, 0, 1);
                colors[index].g = Mathf.Clamp(revealed, 0, 1);
                colors[index].b = 0;

                // Smoothly fade fog out if visibility is positive. Fade out if not.
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

        // Redraw the map
        newViewport.Map = new Texture2D(FogOfWar.Precision, FogOfWar.Precision);
        newViewport.Map.SetPixels(colors);
        newViewport.Map.Apply();
        return newViewport;
    }
    
    private float _oldCloudStrength = 0;
    private float _transitionStartTime = 0;

    public static FogOfWar2 FindExisting
    {
        get
        {
            GameObject obj = GameObject.Find("FogOfWar2");
            if (obj != null && obj.activeInHierarchy)
            {
                return obj.GetComponent<FogOfWar2>();
            }
            return null;
        }
    }

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
            redraw = true;
            _oldCloudStrength = CloudStrength;
        }

        /*
         * Push the new viewport into the shader, and then we're done with
         * updating the viewports.  Whew.
         */

        if (redraw)
        {
            if (visibleViewport.IsEmpty)
            {
                mat.SetVector("CurrentViewport", new Vector4(0, 0, 0, 0));
                mat.SetTexture("CurrentTexture", null);
            }
            else
            {
                mat.SetVector("CurrentViewport", visibleViewport.ToVector4());
                mat.SetTexture("CurrentBeaconMap", visibleViewport.Map);

                if (oldViewport == null)
                {
                    _transitionStartTime = 0;
                    mat.SetVector("PreviousViewport", new Vector4(0, 0, 0, 0));
                    mat.SetTexture("PreviousBeaconMap", null);
                }
                else
                {
                    _transitionStartTime = Time.time;
                    mat.SetVector("PreviousViewport", oldViewport.ToVector4());
                    mat.SetTexture("PreviousBeaconMap", oldViewport.Map);
                }
            }
        }

        redraw = false;

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
            mat.SetTexture("PreviousBeaconMap", oldViewport.Map);
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

    private bool HasVisibleRegion
    {
        get
        {
            return (visibleViewport != null);
        }
    }

    private Rect VisibleRegion
    {
        get
        {
            if (visibleViewport == null)
            {
                return new Rect(0, 0, 0, 0);
            }
            else
            {
                return visibleViewport.Area;
            }
        }
    }

    private float GetOpacity(Vector3 coord)
    {
        Rect rr = VisibleRegion;
        Vector2 coordXZ = new Vector2(coord.x, coord.z);
        if (!rr.Contains(coordXZ))
        {
            return CloudStrength;
        }
        float uu = (coord.x - rr.xMin) / rr.width;
        float vv = (coord.z - rr.yMin) / rr.height;
        Color color = visibleViewport.Map.GetPixelBilinear(uu, vv);
        return color.a;
    }

    public bool IsFoggy(Vector3 coord)
    {
        return GetOpacity(coord) >= 0.5;
    }

    public void ManipulateFog(List<Vector3> points, float strength, float range)
    {
        visibleViewport = DrawMap(points, strength, range, visibleViewport, CloudStrength);
        redraw = true;
    }
}