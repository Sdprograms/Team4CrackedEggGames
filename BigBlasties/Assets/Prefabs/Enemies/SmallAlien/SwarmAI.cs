using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwarmAI : MonoBehaviour, damageInterface
{
    [SerializeField] float HP;
    [SerializeField] float MaxHP;

    [SerializeField] Transform sightPos;
    [SerializeField] Transform attackPos;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject bullet;
    [SerializeField] float attackRate;
    [SerializeField] int turnSpeed;

    [SerializeField] ParticleSystem deathEffect; //-SD

    bool isAttacking;
    Vector3 playerPos;


    // on start set HP to max HP, saving hp and Max HP seperately for possible 'next level' functionality.
    void Start()
    {
        HP = MaxHP;
    }

    void Update()
    {
            playerPos = GameManager.mInstance.mPlayer.transform.position - sightPos.position;
            agent.SetDestination(GameManager.mInstance.mPlayer.transform.position);

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                facetarget();
            }
            if (!isAttacking && canSeePlayer())
            {
                StartCoroutine(attack());
            }
    }

    bool canSeePlayer()
    {
        playerPos = GameManager.mInstance.mPlayer.transform.position - sightPos.position;
        //Debug.DrawRay(sightPos.position, playerPos);

        RaycastHit hit;
        if (Physics.Raycast(sightPos.position, playerPos, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(hitmarker());
        if (HP <= 0)
        {
            Instantiate(deathEffect, transform.position, transform.rotation); // -SD
            Destroy(gameObject);
            GameManager.mInstance.mEnemyDamageHitmarker.SetActive(false);
        }
    }

    IEnumerator hitmarker()
    {
        GameManager.mInstance.mEnemyDamageHitmarker.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        GameManager.mInstance.mEnemyDamageHitmarker.SetActive(false);
    }

    IEnumerator attack()
    {
        isAttacking = true;
        Vector3 directionToPlayer = (GameManager.mInstance.mPlayer.transform.position - attackPos.position).normalized;
        Instantiate(bullet, attackPos.position, Quaternion.LookRotation(directionToPlayer));
        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }

    void facetarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
    }
}
