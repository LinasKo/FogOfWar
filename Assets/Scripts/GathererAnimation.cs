using UnityEngine;
using System.Collections;

public class GathererAnimation : MonoBehaviour {

    Animator anim;
    int runHash = Animator.StringToHash("Base Layer.Run");
    int walkHash = Animator.StringToHash("Base Layer.Walk");
    int idleHash = Animator.StringToHash("Base Layer.Idle");
    int punchHash = Animator.StringToHash("Base Layer.Punch");

    void Start () {
        anim = GetComponent<Animator>();
	}
	
	void Update () {
        if (anim.GetFloat("Speed") == 0 && anim.GetBool("Cutting") == false)
            anim.Play(idleHash);
        else if (anim.GetFloat("Speed") == 3 && anim.GetBool("Cutting") == false)
            anim.Play(walkHash);
        else if (anim.GetFloat("Speed") == 5 && anim.GetBool("Cutting") == false)
            anim.Play(runHash);
        else if (anim.GetBool("Cutting") == true)
            anim.Play(punchHash);
    }
}
