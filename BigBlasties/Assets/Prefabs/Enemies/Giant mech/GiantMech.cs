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
    [SerializeField] GameObject beserk;
    [SerializeField] GameObject batterycells;
    [SerializeField] Transform ArenaCenter;
    [SerializeField] float spawnRadius = 50f;
    [SerializeField] ItemDrop dropScript;
    [SerializeField] HealthBar healthbar;

    private List<GameObject> activeBatteryCells = new List<GameObject>();
    [SerializeField] private Animator animator;
    bool isAttacking;
    bool batteryspawned;
    bool isBeserk;
    Vector3 playerPos;
    private EnemyDetection detector; // this is necessary in order for each enemy to have their own bubble,
                                     // otherwise without this all enemies will respond to one enemy bubble and not their own -XB

    private enum AttackType
    {
        Motor,
        Blast,
        MachineGun
    }

    private Coroutine currentAttackCoroutine;

    // on start set HP to max HP, saving hp and Max HP seperately for possible 'next level' functionality.
    void Start()
    {
        HP = MaxHP;
        detector = GetComponentInChildren<EnemyDetection>();
        agent.updateRotation = false;
        healthbar = GetComponentInChildren<HealthBar>();
        isBeserk = false;
    }

    void Update()
    {
        if (detector.playerInRange)
        {
            facetarget();
            playerPos = GameManager.mInstance.mPlayer.transform.position - sightPos.position;
            agent.SetDestination(GameManager.mInstance.mPlayer.transform.position);
            Vector3 currentPosition = transform.position;
            if (!isAttacking) // add &&  canSeePlayer() if you want to implement the canSeePlayer Bool condition
            {
                currentAttackCoroutine = StartCoroutine(attack());
            }
        }
        if (HP <= MaxHP / 2 && !batteryspawned)
        {
            if (currentAttackCoroutine != null)
            {
                StopCoroutine(currentAttackCoroutine);
                currentAttackCoroutine = null;
                isAttacking = false;
            }

            sheild.SetActive(true);
            // Spawn battery cells
            for (int i = 0; i < 5; i++)
            {
                Quaternion batteryRotation = Quaternion.Euler(-90f, 0f, 0f);
                Vector3 randomPos = GetRandomPositionAroundCenter();
                GameObject battery = Instantiate(batterycells, randomPos, batteryRotation);

                battery.GetComponent<BatteryScript>().Initialize(this);

                activeBatteryCells.Add(battery); 
            }
            HP = MaxHP/2;
            batteryspawned = true;
        }
        if (HP <= MaxHP / 3 && !isBeserk)
        {
            if (currentAttackCoroutine != null)
            {
                StopCoroutine(currentAttackCoroutine);
                currentAttackCoroutine = null;
                isAttacking = false;
            }
            isBeserk = true;
        }
    }
    Vector3 GetRandomPositionAroundCenter()
    {
        float angle = Random.Range(0f, Mathf.PI * 2); 
        float radius = Random.Range(10f, spawnRadius); 
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
        healthbar.UpdateHealthBar(HP, MaxHP);
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
        float healthPercentage = HP / MaxHP;

        if (healthPercentage < 0.4f) 
        {
            isBeserk = true;
            beserk.SetActive(true);
            StartCoroutine(MotorAttack());
            StartCoroutine(BlastAttack());
            StartCoroutine(MachineGunAttack2());
        }
        else if (healthPercentage == 0.5f)
        {
            StartCoroutine(LaserAttack());
        }
        else
        {
            float attackDuration = 5f;
            float startTime = Time.time;
            float maxDeviationAngle = 5f;

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
                        yield return StartCoroutine(MotorAttack());
                        break;

                    case AttackType.Blast:
                        yield return StartCoroutine(BlastAttack());
                        break;

                    case AttackType.MachineGun:
                        yield return StartCoroutine(MachineGunAttack());
                        break;
                }
            }
        }

        yield return new WaitForSeconds(attackRate);
        currentAttackCoroutine = null;
        isAttacking = false;
    }
    private IEnumerator MotorAttack()
    {
        float maxDeviationAngle = 5f;
        Vector3 directionToPlayer = (GameManager.mInstance.mPlayer.transform.position - attackPos.position).normalized;
        float randomDeviation = Random.Range(-maxDeviationAngle, maxDeviationAngle);
        Quaternion deviationRotation = Quaternion.Euler(0, randomDeviation, 0);
        Vector3 deviatedDirection = deviationRotation * directionToPlayer;
        int projectileCount = 10;
        for (int i = 0; i < projectileCount; i++)
        {
            
            Vector3 launchDirection = new Vector3(deviatedDirection.x, 1f, deviatedDirection.z).normalized;
            float launchHeight = 2.0f;
            float launchForce = Random.Range(1f, 100f);
            GameObject projectile = Instantiate(grenademotor, attackPos3.position, Quaternion.LookRotation(launchDirection));
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(launchDirection * launchForce + Vector3.up * launchHeight, ForceMode.Impulse);
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(MotorattackRate);
    }

    private IEnumerator BlastAttack()
    {
        float maxDeviationAngle = 15f;
        Vector3 directionToPlayer = (GameManager.mInstance.mPlayer.transform.position - attackPos.position).normalized;
        float randomDeviation = Random.Range(-maxDeviationAngle, maxDeviationAngle);
        Quaternion deviationRotation = Quaternion.Euler(0, randomDeviation, 0);
        Vector3 deviatedDirection = deviationRotation * directionToPlayer;
        int shotgunProjectileCount = 5;
        for (int i = 0; i < shotgunProjectileCount; i++)
        {

            float randomDeviation2 = Random.Range(-maxDeviationAngle, maxDeviationAngle);
            float randomVerticalDeviation = Random.Range(-maxDeviationAngle, maxDeviationAngle);
            Quaternion deviationRotation2 = Quaternion.Euler(randomVerticalDeviation, randomDeviation2, 0);
            Vector3 deviatedDirection2 = deviationRotation2 * directionToPlayer;

            GameObject rocketProjectile = Instantiate(rocket, attackPos2.position, Quaternion.LookRotation(deviatedDirection2));
            Rigidbody rbRocket = rocketProjectile.GetComponent<Rigidbody>();
            if (rbRocket != null)
            {
                float launchForce = 30f; // Adjust launch force as needed
                rbRocket.AddForce(deviatedDirection * launchForce, ForceMode.Impulse);
            }
        }
        yield return new WaitForSeconds(BlastattackRate);
    }

    private IEnumerator MachineGunAttack2()
    {
        float maxDeviationAngle = 5f;
        Vector3 directionToPlayer = (GameManager.mInstance.mPlayer.transform.position - attackPos.position).normalized;
        float randomDeviation = Random.Range(-maxDeviationAngle, maxDeviationAngle);
        Quaternion deviationRotation = Quaternion.Euler(0, randomDeviation, 0);
        int ProjectileCount = 30;
        for (int i = 0; i < ProjectileCount; i++)
        {
            float randomDeviation2 = Random.Range(-maxDeviationAngle, maxDeviationAngle);
            float randomVerticalDeviation = Random.Range(-maxDeviationAngle, maxDeviationAngle);
            Quaternion deviationRotation2 = Quaternion.Euler(randomVerticalDeviation, randomDeviation2, 0);
            Vector3 deviatedDirection2 = deviationRotation2 * directionToPlayer;
            Instantiate(bullet, attackPos.position, Quaternion.LookRotation(deviatedDirection2));
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(MachineGunattackRate);
    }

    private IEnumerator MachineGunAttack()
    {
        Vector3 directionToPlayer = (GameManager.mInstance.mPlayer.transform.position - attackPos.position).normalized;
        float randomDeviation = Random.Range(-15f, 15f); // Adjust deviation angle as necessary
        Quaternion deviationRotation = Quaternion.Euler(0, randomDeviation, 0);
        Vector3 deviatedDirection = deviationRotation * directionToPlayer;
        Instantiate(bullet, attackPos.position, Quaternion.LookRotation(deviatedDirection));
        yield return new WaitForSeconds(MachineGunattackRate);
    }

    private IEnumerator LaserAttack()
    {
        Vector3 directionToPlayer = (GameManager.mInstance.mPlayer.transform.position - attackPos.position).normalized;
        int ProjectileCount = 30;
        for (int i = 0; i < ProjectileCount; i++)
        {
            Instantiate(bullet, attackPos.position, Quaternion.LookRotation(directionToPlayer));
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(MachineGunattackRate);
    }


    void facetarget()
    {
        
        Vector3 direction = (GameManager.mInstance.mPlayer.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
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
