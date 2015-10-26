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
    //private float volLowRange = .5f;
    //private float volHighRange = 1.0f;

    private AudioClip[] deathSfx = new AudioClip[3];
    private AudioClip[] woodSfx = new AudioClip[3];
    private AudioClip[] punchSfx = new AudioClip[3];
    private AudioClip[] hitSfx = new AudioClip[3];

    private FogOfWar fog;

    // Corpse to leave after the object is dead.
    public GameObject remains;

    // Audio event is basically a GameObject that only has an audio source.
    public GameObject audioEvent;

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

    public void Start()
    {
        fog = FogOfWar.FindExisting(Player.RED);
    }

    public void Damage(int damage, Vector3 locationOfAssailant)
    {
        int n = Random.Range(0, 2);
        float time = 0;

        health -= damage;

        if (health <= 0)
        {
            if (gameObject.tag == "Tree")
            {
                // Play sound of tree falling
                time = (float)(woodSfx[n].length);
                PlaySound(woodSfx[n], 1.0F, time, true);

                // Spawn fallen tree
                if (remains != null)
                {
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                    Vector3 fallDirection = locationOfAssailant / 10; // TODO - this still needs verification
                    fallDirection.y = 0;
                    Instantiate(remains, gameObject.transform.position, Quaternion.Euler(fallDirection));
                    gameObject.SetActive(false);
                }

                // Destroy tree
                Destroy(gameObject, time);

            }
            else if (gameObject.tag == "Gatherer" || gameObject.tag == "Soldier")
            {
                time = (float) (deathSfx[n].length);
                PlaySound(deathSfx[n], 1.0f, 0.0f, false);
                Destroy(gameObject, time);
            }   
        }

        else
        {
            if (gameObject.tag == "Tree")
                PlaySound(punchSfx[n], 1.0f, 0.0f, false);

            else if (gameObject.tag == "Gatherer" || gameObject.tag == "Soldier")
                PlaySound(hitSfx[n], 1.0f, 0.0f, false);
        }
    }

    public void PlaySound(AudioClip sound, float volume, float time, bool separateSource)
    {
        if (!fog.IsFoggy(gameObject.transform.position))
        {
            // Create a separate sound source at object location and play the sound.
            if (separateSource)
            {
                GameObject audioInstance = (GameObject)Instantiate(audioEvent, gameObject.transform.position, Quaternion.identity);
                audioInstance.GetComponent<AudioSource>().PlayOneShot(sound, volume);
                Destroy(audioInstance, time);
            }
            else
            {
                source.PlayOneShot(sound, volume);
            }
        }
    }
}

