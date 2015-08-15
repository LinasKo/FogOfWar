using UnityEngine;
using System.Collections;

public class DamageReceiver : MonoBehaviour
{
    public int health;
    public AudioClip death1, death2, death3;
    public AudioClip wood1, wood2, wood3;
    public AudioClip punch1, punch2, punch3;
    public AudioClip hit1, hit2, hit3;

    private AudioSource source;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;

    private AudioClip[] deathSfx = new AudioClip[3];
    private AudioClip[] woodSfx = new AudioClip[3];
    private AudioClip[] punchSfx = new AudioClip[3];
    private AudioClip[] hitSfx = new AudioClip[3];

    void Awake ()
    {
        source = GetComponent<AudioSource>();
        
        deathSfx[0] = death1;
        deathSfx[1] = death2;
        deathSfx[2] = death3;

        woodSfx[0] = wood1;
        woodSfx[1] = wood2;
        woodSfx[2] = wood3;

        punchSfx[0] = punch1;
        punchSfx[1] = punch2;
        punchSfx[2] = punch3;

        hitSfx[0] = hit1;
        hitSfx[1] = hit2;
        hitSfx[2] = hit3;
    }

    public void Damage(int damage)
    {
        int n = Random.Range(0, 2);
        float time = 0;

        health -= damage;

        if (health <= 0)
        {
            if (gameObject.tag == "Tree")
            {
                time = (float) (woodSfx[n].length);
                source.PlayOneShot(woodSfx[n], 1F);
                Destroy(this.gameObject, time);
            }
            else if (gameObject.tag == "Gatherer" || gameObject.tag == "Soldier")
            {
                time = (float) (deathSfx[n].length);
                source.PlayOneShot(deathSfx[n], 1F);
                Destroy(this.gameObject, time);
            }   
        }

        else
        {
            if (gameObject.tag == "Tree")
                source.PlayOneShot(punchSfx[n], 1F);

            else if (gameObject.tag == "Gatherer" || gameObject.tag == "Soldier")
                source.PlayOneShot(hitSfx[n], 1F);
        }
    }
}

