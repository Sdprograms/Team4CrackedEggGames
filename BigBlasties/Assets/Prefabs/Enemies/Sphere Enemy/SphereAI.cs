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

    bool isAttacking;
    //bool playerInRange;

    //below are for fleeing and healing
    bool isFleeing;
    bool isHealing;

    float distance;

    Vector3 playerPos;

    private EnemyDetection detector; // this is necessary in order for each enemy to have their own bubble,
                                     // otherwise without this all enemies will respond to one enemy bubble and not their own -XB

    // on start set HP to max HP, saving hp and Max HP seperately for possible 'next level' functionality.
    void Start()
    {
        HP = MaxHP;

        detector = GetComponentInChildren<EnemyDetection>(); // when adding the bubble as a child, the script from each gameobject will put
                                                             // its data into the enemy individuality -XB
    }

    void Update()
    {
        if (detector.playerInRange)
        {
            if (animator.GetBool("Roll") == false)
            {
                animator.Play("anim_close");
            }
            animator.SetBool("Roll", true);
            //add if fleeing is implemented, and if healing is implemented
            //isFleeing = false;
            //isHealing = false;
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

            //fleeing property commented out, add if fleeing to be added
            /*if (HP <= MaxHP / 4)
            {
                playerPos = GameManager.mInstance.mPlayer.transform.position - sightPos.position;

                Vector3 fleeDirection = sightPos.position - GameManager.mInstance.mPlayer.transform.position;
                Vector3 fleeTarget = sightPos.position + fleeDirection.normalized * aggroRange; // Move away by aggroRange distance

                agent.SetDestination(fleeTarget);  // Set the agent's destination away from the player

                fleetarget();

                aggroRange = 30;

                if (isFleeing)
                {
                    StopCoroutine(attack());
                }
                distance = Vector3.Distance(GameManager.mInstance.mPlayer.transform.position, sightPos.position);
                aggroRange = 30;
                if (distance >= aggroRange)
                {
                    playerInRange = false;
                }
            }*/


            //healing commented out, add if healing and fleeing to be added
            /*
            else if (isFleeing && !playerInRange && !isHealing)
            {
                StartCoroutine(heal());
                isHealing = true;
                isFleeing = false;
            }*/
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        detector.playerInRange = true;
    //    }
    //}

    //enemy take damage function
    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(hitmarker());
        detector.playerInRange = true;
        if (HP <= 0)
        {
            Instantiate(bullet, attackPos.position, transform.rotation);
            Destroy(gameObject);
            GameManager.mInstance.mEnemyDamageHitmarker.SetActive(false);
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
        Destroy(gameObject);


        yield return new WaitForSeconds(attackRate);

        isAttacking = false;
    }

    void facetarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
    }


    // flee and heal commented out
    /*void fleetarget()
    {
        Quaternion rot = Quaternion.LookRotation(-playerPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
        isFleeing = true;
    }

    IEnumerator heal()
    {
        while (HP < 10)
        {
            HP += 1;
            yield return new WaitForSeconds(2f);
        }
    }*/
}