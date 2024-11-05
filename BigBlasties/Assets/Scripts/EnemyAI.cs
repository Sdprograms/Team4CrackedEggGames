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
    [SerializeField] int turnSpeed;

    bool isAttacking;
    bool playerInRange;
    bool isFleeing;
    bool isHealing;

    float distance;

    Vector3 playerPos;

    Color orginalColor;
   

    // on start set HP to max HP, saving hp and Max HP seperately for possible 'next level' functionality.
    void Start()
    {
        HP = MaxHP;
    }

    void Update()
    {
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
            Destroy(gameObject);
        }
    }

    //IEnumerator so that when enemy takes damage a hitmarker appears on the UI.
    IEnumerator hitmarker()
    {
        //Gamemanager.instance.playerhitmarker.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        //Gamemanager.instance.playerhitmarker.SetActive(false);
        
    }

    IEnumerator attack()
    {
        isAttacking = true;
        Instantiate(bullet, attackPos.position, transform.rotation);

        yield return new WaitForSeconds(attackRate);
        isAttacking=false;
    }

    void facetarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
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
