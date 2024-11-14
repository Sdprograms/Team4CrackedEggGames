using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class RobotAI : MonoBehaviour, damageInterface
{
    [SerializeField] float HP;
    [SerializeField] float MaxHP;
    [SerializeField] int aggroRange;

    [SerializeField] Transform sightPos;
    [SerializeField] Transform attackPos;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject longbullet;
    [SerializeField] float attackRate;

    //the time the enemy will stand still while attacking.
    [SerializeField] float attackTime;

    //the time the enemy will stand still while reloading
    [SerializeField] float ReloadTime;


    [SerializeField] int turnSpeed;


    //for animations
    private Animator animator;

    bool isAttacking;
    //bool playerInRange;
    bool isFleeing;
    bool isHealing;

    float distance;

    Vector3 playerPos;

    float originalSpeed;

    public float mVelocity;

    private EnemyDetection detector; // this is necessary in order for each enemy to have their own bubble,
                                     // otherwise without this all enemies will respond to one enemy bubble and not their own

    // on start set HP to max HP, saving hp and Max HP seperately for possible 'next level' functionality.
    void Start()
    {
        HP = MaxHP;
        animator = GetComponent<Animator>();
        animator.SetBool("IsLoadedAnim", true);

        detector = GetComponentInChildren<EnemyDetection>(); // when adding the bubble as a child, the script from each gameobject will put
                                                             // its data into the enemy individuality -XB
    }

    void Update()
    {
        //Debug.DrawRay(sightPos.position, playerPos);
        //changed agent.speed to velocity, because speed is a serialized int. 
        if (agent.velocity.magnitude > 0.1f) 
        {
            animator.SetBool("IsRunAnim", true);
        }
        else
        {
            animator.SetBool("IsRunAnim", false);
        }
       
        mVelocity = agent.velocity.magnitude;

        if (detector.playerInRange)// the proper way to access the enemies range
        {
            //adding fleeing functionality to enemies when on low hp.
            if (HP <= MaxHP / 4)
            {
                playerPos = GameManager.mInstance.mPlayer.transform.position - sightPos.position;

                Vector3 fleeDirection = sightPos.position - GameManager.mInstance.mPlayer.transform.position;
                Vector3 fleeTarget = sightPos.position + fleeDirection.normalized * aggroRange; // Move away by aggroRange distance -XB

                agent.SetDestination(fleeTarget);  // Set the agent's destination away from the player -XB

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
                    detector.playerInRange = false;
                }
            }
            else
            {
                isHealing = false;
                animator.SetBool("IsHealAnim", false);
                isFleeing = false;
                playerPos = GameManager.mInstance.mPlayer.transform.position - sightPos.position;
                agent.SetDestination(GameManager.mInstance.mPlayer.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    facetarget();
                }
                if (!isAttacking)
                {
                    StartCoroutine(attack());
                }
            }
        }
        else if (isFleeing  && !detector.playerInRange && !isHealing && !animator.GetBool("IsRunAnim"))// and example to reference the false version of the bool -XB
        {
            animator.SetBool("IsAlarmedAnim", false);
            animator.SetBool("IsHealAnim", true);
            StartCoroutine(heal());
            isHealing = true;
            isFleeing = false;
        }

        Entered(detector.playerInRange);
    }

    //private void Entered(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        animator.SetBool("IsAlarmedAnim", true);
    //        //EnemyDetection.mEnemyDetInst.playerInRange = true;// this line is unnecessary as the EnemyDetection class handles that bool
    //    }
    //}

    private void Entered(bool isPlayerInRange) // this replaces the functionality of the method commented out above -XB
    {
        if (detector.playerInRange == true)
        {
            animator.SetBool("IsAlarmedAnim", true);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    other = EnemyDetection.mEnemyDetInst.mOther;
    //    if (other.CompareTag("Player"))
    //    {
    //        animator.SetBool("IsAlarmedAnim", true);
    //        //EnemyDetection.mEnemyDetInst.playerInRange = true;
    //    }
    //}

    //enemy take damage function
    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(hitmarker());
        aggroRange = 1000;
         detector.playerInRange = true;
        animator.SetBool("IsAlarmedAnim", true);
        animator.SetBool("IsHealAnim", false);
        if (HP <= 0)
        {
            animator.Play("Die");
            agent.speed = 0;
            Destroy(gameObject, 2f);
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

        //to save the enemys move speed
        originalSpeed = agent.speed;

        //enemy stops to shoot, plays shooting animation
        agent.speed = 0;
        animator.SetBool("IsShootAnim", true);
        animator.Play("Shoot_SingleShot_AR");

        //pausing for the attackTime
        StartCoroutine(facetargetTimed(attackTime));
        yield return new WaitForSeconds(attackTime);

        if (agent.remainingDistance > 30)
        {
            Instantiate(longbullet, attackPos.position, transform.rotation);
            animator.SetBool("IsShootAnim", false);
            animator.SetBool("IsLoadedAnim", false);
        }
        else
        {
            Instantiate(bullet, attackPos.position, transform.rotation);
            animator.SetBool("IsShootAnim", false);
            animator.SetBool("IsLoadedAnim", false);
        }
        

        //after pausing for the reload time, the enemy moves again
        yield return new WaitForSeconds(ReloadTime);
        agent.speed = originalSpeed;


        yield return new WaitForSeconds(attackRate);
        isAttacking=false;
        animator.SetBool("IsLoadedAnim", true);
    }

    void facetarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
    }

    IEnumerator facetargetTimed(float duration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            Quaternion rot = Quaternion.LookRotation(playerPos);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * turnSpeed);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    void fleetarget()
    {
        Quaternion rot = Quaternion.LookRotation(-playerPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
        isFleeing = true;
    }

    IEnumerator heal()
    {
        while (HP < MaxHP)
        {
            if (animator.GetBool("IsAlarmedAnim")) 
            {
                break; 
            }         
            HP += 1;
            yield return new WaitForSeconds(2f);
        }
        animator.SetBool("IsHealAnim", false);
    }
}