using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileSpawn : MonoBehaviour
{

    float spawnTimer;
    [SerializeField] float spawnDelay;
    [SerializeField] GameObject projectile;
    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.mInstance.mPaused == false)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnDelay)
            {
                Instantiate(projectile, transform.position, transform.rotation);
                spawnTimer = 0;
            }
        }
            //StartCoroutine(spawnProjectile());
        }

        /* IEnumerator spawnProjectile()
         {
             Instantiate(projectile, transform.position, transform.rotation);
             yield return new WaitForSeconds(spawnTime);
         }*/
    }
