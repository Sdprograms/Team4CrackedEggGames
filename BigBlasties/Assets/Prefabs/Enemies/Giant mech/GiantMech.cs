using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GiantMech : MonoBehaviour, damageInterface
{
    [SerializeField] float HP;
    [SerializeField] float MaxHP;
    [SerializeField] Transform sightPos;
    [SerializeField] Transform attackPos;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject bullet;
    [SerializeField] float attackRate;
    [SerializeField] int turnSpeed;
    [SerializeField] int viewAngle;

    [SerializeField] float flyingHeight; //for flying enemies, the height at which they fly
    [SerializeField] ItemDrop dropScript;

    bool isAttacking;
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
            playerPos = GameManager.mInstance.mPlayer.transform.position - sightPos.position;
            agent.SetDestination(GameManager.mInstance.mPlayer.transform.position);
            Vector3 currentPosition = transform.position;
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                facetarget();
            }
            if (!isAttacking) // add &&  canSeePlayer() if you want to implement the canSeePlayer Bool condition
            {
                StartCoroutine(attack());
            }
        }
    }

    //this helps prevent friendly fire bwtween enemies (some will still shoot each other, they will start aiming when there is line of sight, but if the palyer moves
    // during the attack time, they adjust their aim and they may shoot a friendly
    bool canSeePlayer()
    {
        playerPos = GameManager.mInstance.mPlayer.transform.position - sightPos.position;
        float angleToPlayer = Vector3.Angle(playerPos, transform.forward);
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



    //enemy take damage function
    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(hitmarker());
        detector.playerInRange = true;
        if (HP <= 0)
        {
            if (dropScript != null)
                dropScript.Drop();

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

    public void setHP(float amount)
    {
        HP = amount;
    }
    public float getHP()
    {
        return HP;
    }
}
