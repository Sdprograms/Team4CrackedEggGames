using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SniperAI : MonoBehaviour, damageInterface
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


    //the time the enemy will stand still while attacking.
    [SerializeField] float attackTime;

    //the time the enemy will stand still while reloading
    [SerializeField] float ReloadTime;

    bool isAttacking;
    //bool playerInRange;

    
    bool isFleeing;
    public float distance;

    Vector3 playerPos;

    float originalSpeed;

    //to equip and drop game object
    public Transform rightGunBone;
    public Transform leftGunBone;
    public Arsenal[] arsenal;

    private Animator animator;

    private EnemyDetection detector; // this is necessary in order for each enemy to have their own bubble,
                                     // otherwise without this all enemies will respond to one enemy bubble and not their own -XB

    void Start()
    {
        HP = MaxHP;
        animator = GetComponent<Animator>();
        if (arsenal.Length > 0)
        {
            SetArsenal(arsenal[0].name);
        }

        detector = GetComponentInChildren<EnemyDetection>(); // when adding the bubble as a child, the script from each gameobject will put
                                                             // its data into the enemy individuality -XB
    }

    void Update()
    {
        if (detector.playerInRange)
        {
            distance = Vector3.Distance(GameManager.mInstance.mPlayer.transform.position, sightPos.position);
            playerPos = GameManager.mInstance.mPlayer.transform.position - sightPos.position;

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                facetarget();
            }

            if (!isAttacking && canSeePlayer())
            {
                StartCoroutine(attack());
            }

            //TO RETREAT IF PLAYER TOO CLOSE
            if (distance < 30)
            {

                Vector3 fleeDirection = sightPos.position - GameManager.mInstance.mPlayer.transform.position;
                Vector3 fleePosition = sightPos.position + fleeDirection.normalized * 30;
                agent.SetDestination(fleePosition);
                fleetarget();

            }
        }
    }

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

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        detector.playerInRange = true;
    //    }
    //}

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(hitmarker());
        aggroRange = 1000;
        detector.playerInRange = true;
        if (HP <= 0)
        {
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
        originalSpeed = agent.speed;
        agent.speed = 0;

        StartCoroutine(facetargetTimed(attackTime));
        yield return new WaitForSeconds(attackTime);

        Instantiate(bullet, attackPos.position, transform.rotation);
        yield return new WaitForSeconds(ReloadTime);

        agent.speed = originalSpeed;
        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
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

    public void SetArsenal(string name)
    {
        foreach (Arsenal hand in arsenal)
        {
            if (hand.name == name)
            {
                if (rightGunBone.childCount > 0)
                    Destroy(rightGunBone.GetChild(0).gameObject);
                if (leftGunBone.childCount > 0)
                    Destroy(leftGunBone.GetChild(0).gameObject);
                if (hand.rightGun != null)
                {
                    GameObject newRightGun = (GameObject)Instantiate(hand.rightGun);
                    newRightGun.transform.parent = rightGunBone;
                    newRightGun.transform.localPosition = Vector3.zero;
                    newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                }
                if (hand.leftGun != null)
                {
                    GameObject newLeftGun = (GameObject)Instantiate(hand.leftGun);
                    newLeftGun.transform.parent = leftGunBone;
                    newLeftGun.transform.localPosition = Vector3.zero;
                    newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                }
                animator.runtimeAnimatorController = hand.controller;
                return;
            }
        }
    }

    [System.Serializable]
    public struct Arsenal
    {
        public string name;
        public GameObject rightGun;
        public GameObject leftGun;
        public RuntimeAnimatorController controller;
    }
}
