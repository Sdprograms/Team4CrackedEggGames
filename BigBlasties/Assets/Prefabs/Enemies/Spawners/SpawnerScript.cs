using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] int numberToSpawn;
    [SerializeField] int objectSpawnTime;
    [SerializeField] Transform[] spawnPos;

    int spawnCount;

    bool startSpawning;
    bool isSpawning;

    void Start()
    {
        spawnCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && spawnCount < numberToSpawn && !isSpawning)
        {
            StartCoroutine(spawn());
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }

    IEnumerator spawn()
    {
        isSpawning = true;

        int spawnInt = Random.Range(0, spawnPos.Length);
        Instantiate(objectToSpawn, spawnPos[spawnInt].position, spawnPos[spawnInt].rotation);
        spawnCount++;

        yield return new WaitForSeconds(objectSpawnTime);
        

        isSpawning = false;
    }
}
