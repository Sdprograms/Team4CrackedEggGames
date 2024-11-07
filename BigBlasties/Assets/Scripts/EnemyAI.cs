using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, damageInterface
{
    [SerializeField] float HP;
    [SerializeField] float MaxHP;
    [SerializeField] int aggroRange;

    [SerializeField] Transform sightPos;
    [SerializeField] Transform attackPos;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject bullet;
    [SerializeField] float attackRate;

    //the time the enemy will stand still while attacking.
    [SerializeField] float attackTime;

    //the time the enemy will stand still while reloading
    [SerializeField] float ReloadTime;


    [SerializeField] int turnSpeed;


    //for animations
    private Animator animator;

    bool isAttacking;
    bool playerInRange;
    bool isFleeing;
    bool isHealing;

    float distance;

    Vector3 playerPos;

    float originalSpeed;

   

    // on start set HP to max HP, saving hp and Max HP seperately for possible 'next level' functionality.
    void Start()
    {
        HP = MaxHP;
        animator = GetComponent<Animator>();
        animator.SetBool("IsLoadedAnim", true);
    }

    void Update()
    {
        //if the agent is moving, run anim is true
        if (agent.speed > 0)
        {
            animator.SetBool("IsRunAnim", true);
        }
        else
        {
            animator.SetBool("IsRunAnim", false);
        }

        if (playerInRange)
        {
            //adding fleeing functionality to enemies when on low hp.
            if (HP <= MaxHP / 4)
            {
                playerPos = GameManager.mInstance.mPlayer.transform.position - sightPos.position;
                agent.SetDestination(-GameManager.mInstance.mPlayer.transform.position);
                fleetarget();

                if (isFleeing)
                {
                    StopCoroutine(attack());
                }
                distance = Vector3.Distance(GameManager.mInstance.mPlayer.transform.position, sightPos.position);
                if (distance >= aggroRange)
                {
                    playerInRange = false;
                }
            }
            else
            {
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
        else if (isFleeing  && !playerInRange && !isHealing)
        {
            StartCoroutine(heal());
            isHealing = true;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("IsAlarmedAnim", true);
            playerInRange = true;
        }
    }

    //enemy take damage function
    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(hitmarker());
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

        //pausing for the attackTime
        StartCoroutine(facetargetTimed(attackTime));
        yield return new WaitForSeconds(attackTime);

        Instantiate(bullet, attackPos.position, transform.rotation);
        animator.SetBool("IsShootAnim", false);
        animator.SetBool("IsLoadedAnim", false);

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
        while (HP < 10)
        {
            HP += 1;
            yield return new WaitForSeconds(2f);
        }
    }
}
