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
    [SerializeField] Transform attackPos2;
    [SerializeField] Transform attackPos3;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject rocket;
    [SerializeField] GameObject grenademotor;
    [SerializeField] float attackRate;
    [SerializeField] float MachineGunattackRate;
    [SerializeField] float MotorattackRate;
    [SerializeField] float BlastattackRate;
    [SerializeField] int turnSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] GameObject sheild;
    [SerializeField] GameObject batterycells;
    [SerializeField] Transform ArenaCenter;
    [SerializeField] float spawnRadius = 50f;
    [SerializeField] ItemDrop dropScript;

    private List<GameObject> activeBatteryCells = new List<GameObject>();
    [SerializeField] private Animator animator;
    bool isAttacking;
    bool batteryspawned;
    Vector3 playerPos;
    private EnemyDetection detector; // this is necessary in order for each enemy to have their own bubble,
                                     // otherwise without this all enemies will respond to one enemy bubble and not their own -XB

    private enum AttackType
    {
        Motor,
        Blast,
        MachineGun
    }

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
        if (HP <= MaxHP / 2 && !batteryspawned)
        {
            sheild.SetActive(true);
            // Spawn battery cells
            for (int i = 0; i < 5; i++)
            {
                Quaternion batteryRotation = Quaternion.Euler(-90f, 0f, 0f); // Set -90 degrees on the X-axis
                Vector3 randomPos = GetRandomPositionAroundCenter();
                GameObject battery = Instantiate(batterycells, randomPos, batteryRotation);

                battery.GetComponent<BatteryScript>().Initialize(this);

                activeBatteryCells.Add(battery); // Track battery cells
            }

            batteryspawned = true;
        }
    }
    Vector3 GetRandomPositionAroundCenter()
    {
        float angle = Random.Range(0f, Mathf.PI * 2); // Random angle in radians
        float radius = Random.Range(0f, spawnRadius); // Random distance from the center
        float xOffset = Mathf.Cos(angle) * radius;
        float zOffset = Mathf.Sin(angle) * radius;

        Vector3 centerPosition = ArenaCenter.position;
        return new Vector3(centerPosition.x + xOffset, centerPosition.y, centerPosition.z + zOffset);
    }

    public void OnBatteryCellDestroyed(GameObject batteryCell)
    {
        activeBatteryCells.Remove(batteryCell);
        Destroy(batteryCell);

        if (activeBatteryCells.Count == 0)
        {
            sheild.SetActive(false);
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
        float attackDuration = 5f;
        float startTime = Time.time;
        float maxDeviationAngle = 15f;
        isAttacking = true;
        AttackType attackType = (AttackType)Random.Range(0, 3);
        while (Time.time < startTime + attackDuration)
        {
            Vector3 directionToPlayer = (GameManager.mInstance.mPlayer.transform.position - attackPos.position).normalized;
            float randomDeviation = Random.Range(-maxDeviationAngle, maxDeviationAngle);
            Quaternion deviationRotation = Quaternion.Euler(0, randomDeviation, 0);
            Vector3 deviatedDirection = deviationRotation * directionToPlayer;
            switch (attackType)
            {
                case AttackType.Motor:
                    int projectileCount = 10;
                    for (int i = 0; i < projectileCount; i++)
                    {
                        Vector3 launchDirection = new Vector3(deviatedDirection.x, 1f, deviatedDirection.z).normalized;
                        float launchHeight = 2.0f;
                        float launchForce = Random.Range(1f, 50f);
                        GameObject projectile = Instantiate(grenademotor, attackPos.position, Quaternion.LookRotation(launchDirection));
                        Rigidbody rb = projectile.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.AddForce(launchDirection * launchForce + Vector3.up * launchHeight, ForceMode.Impulse);
                        }

                        yield return new WaitForSeconds(0.1f);
                    }
                    yield return new WaitForSeconds(MotorattackRate);
                    break;

                case AttackType.Blast:
                    Instantiate(rocket, attackPos2.position, Quaternion.LookRotation(deviatedDirection));
                    yield return new WaitForSeconds(BlastattackRate);
                    break;

                case AttackType.MachineGun:
                    Instantiate(bullet, attackPos.position, Quaternion.LookRotation(deviatedDirection));
                    yield return new WaitForSeconds(MachineGunattackRate);
                    break;
            }
        }
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
