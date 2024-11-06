using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileSpawn : MonoBehaviour
{

    [SerializeField] float spawnTime;
    [SerializeField] GameObject projectile;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;

        if (spawnTime >= 1)
        {
            Instantiate(projectile, transform.position, transform.rotation);
            spawnTime = 0;
        }
        //StartCoroutine(spawnProjectile());
    }

    IEnumerator spawnProjectile()
    {
        Instantiate(projectile, transform.position, transform.rotation);
        yield return new WaitForSeconds(spawnTime);
    }
}
