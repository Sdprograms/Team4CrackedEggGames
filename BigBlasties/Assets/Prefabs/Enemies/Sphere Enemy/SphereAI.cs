using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SphereAI : MonoBehaviour, damageInterface
{
    [SerializeField] float HP;
    [SerializeField] float MaxHP;

    [SerializeField] Transform sightPos;
    [SerializeField] Transform attackPos;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject bullet;
    [SerializeField] float attackRate;
    [SerializeField] int turnSpeed;

    [SerializeField] private Animator animator;
    [SerializeField] ItemDrop dropScript;

    private Dictionary<AudioClip, float> soundCooldowns = new Dictionary<AudioClip, float>();
    [SerializeField] private float soundCooldownTime = 0.3f;

    [SerializeField] AudioClip AudBlast;
    [SerializeField] AudioClip AudRoll;

    bool isAttacking;
    //bool playerInRange;

    //below are for fleeing and healing
    bool isFleeing;
    bool isHealing;

    float distance;

    Vector3 playerPos;

    private EnemyDetection detector; // this is necessary in order for each enemy to have their own bubble,
                                     // otherwise without this all enemies will respond to one enemy bubble and not their own -XB

    private AudioSource audioSource;

    bool hasDied;

    // on start set HP to max HP, saving hp and Max HP seperately for possible 'next level' functionality.
    void Start()
    {
        HP = MaxHP;

        detector = GetComponentInChildren<EnemyDetection>(); // when adding the bubble as a child, the script from each gameobject will put
                                                             // its data into the enemy individuality -XB

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (detector.playerInRange)
        {
            if (animator.GetBool("Roll") == false)
            {
                animator.Play("anim_close");
            }
            PlaySound(AudRoll);
            animator.SetBool("Roll", true);
            playerPos = GameManager.mInstance.mPlayer.transform.position - sightPos.position;
            agent.SetDestination(GameManager.mInstance.mPlayer.transform.position);
            distance = Vector3.Distance(GameManager.mInstance.mPlayer.transform.position, sightPos.position);

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                facetarget();
            }
            if (!isAttacking && distance <= 2)
            {
                StartCoroutine(attack());
            }

            
        }
    }


    //enemy take damage function
    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(hitmarker());
        detector.playerInRange = true;
        if (HP <= 0)
        {
            Instantiate(bullet, attackPos.position, transform.rotation);
            PlayExplode(AudBlast);


            Destroy(gameObject);
            GameManager.mInstance.mEnemyDamageHitmarker.SetActive(false);
            if (!hasDied)
            {
                GameManager.mInstance.UpdateEnemyCount(-1);
                hasDied = true;

                if (dropScript != null)
                {
                    dropScript.Drop();
                }
            }
        }
    }

    //IEnumerator so that when enemy takes damage a hitmarker appears on the UI.
    IEnumerator hitmarker()
    {
        GameManager.mInstance.mEnemyDamageHitmarker.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        GameManager.mInstance.mEnemyDamageHitmarker.SetActive(false);
    }

    IEnumerator attack()
    {
        isAttacking = true;
        Instantiate(bullet, attackPos.position, transform.rotation);
        PlayExplode(AudBlast);
        Destroy(gameObject);


        yield return new WaitForSeconds(attackRate);

        isAttacking = false;
    }

    void facetarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
    }


    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            float currentTime = Time.time;


            if (soundCooldowns.TryGetValue(clip, out float lastPlayTime))
            {
                if (currentTime - lastPlayTime < soundCooldownTime)
                {
                    return;
                }
            }

            audioSource.PlayOneShot(clip);
            soundCooldowns[clip] = currentTime;
        }
    }

    private void PlayExplode(AudioClip clip)
    {
        if (clip != null)
        {
            
            GameObject tempAudio = new GameObject("TempAudio");
            AudioSource tempAudioSource = tempAudio.AddComponent<AudioSource>();

            
            tempAudioSource.clip = clip;
            tempAudioSource.volume = audioSource.volume; 
            tempAudioSource.pitch = audioSource.pitch;   
            tempAudioSource.spatialBlend = audioSource.spatialBlend;
            tempAudioSource.Play();

            
            Destroy(tempAudio, clip.length);
        }
    }
}