using UnityEngine;
using System.Collections;

public class Corpse : MonoBehaviour {

    // duration after which the corpse will start to decay and disappear.
    public float decayStart;
    public float decayEnd;
    public int decaySlowness;

    private Renderer rend;
    private FogOfWar fog;

    // Use this for initialization
    void Start () {
        // Manage corpse rendering for red player.
        fog = FogOfWar.FindExisting(Player.RED);
        rend = GetComponent<Renderer>();

        StartCoroutine(StartDecay(decayStart));
        Destroy(this.gameObject, decayEnd);
    }

    void Update ()
    {
        if (fog.IsFoggy(transform.position))
        {
            rend.enabled = false;
        }
        else
        {
            rend.enabled = true;
        }
    }

    private IEnumerator StartDecay(float time)
    {
        yield return new WaitForSeconds(time);
        this.GetComponent<Rigidbody>().drag = decaySlowness;
        this.GetComponent<Rigidbody>().useGravity = true;
        Collider coll = this.GetComponent<Collider>();
        if (coll != null) coll.enabled = false;
    }
}
