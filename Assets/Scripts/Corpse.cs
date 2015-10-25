using UnityEngine;
using System.Collections;

public class Corpse : MonoBehaviour {

    // duration after which the corpse will start to decay and disappear.
    public float decayStart;
    public float decayEnd;
    public int decaySlowness;

    // Use this for initialization
    void Start () {
        StartCoroutine(StartDecay(decayStart));
        Destroy(this.gameObject, decayEnd);
    }

    private IEnumerator StartDecay(float time)
    {
        yield return new WaitForSeconds(time);
        this.GetComponent<Rigidbody>().drag = decaySlowness;
        this.GetComponent<Collider>().enabled = false;
    }
}
