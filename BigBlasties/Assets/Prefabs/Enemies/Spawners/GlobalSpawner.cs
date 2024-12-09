using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSpawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] public int numberToSpawn;
    [SerializeField] int objectSpawnTime;
    [SerializeField] Transform[] spawnPos;

    int spawnCount;

    bool startSpawning;
    bool isSpawning;

    void Start()
    {
        startSpawning = true;
        spawnCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && spawnCount < numberToSpawn && !isSpawning && KillRoomDetector.mKillRoomInst.mSpawnEnemies == true)
        {
            StartCoroutine(spawn());
        }

    }

    IEnumerator spawn()
    {
        isSpawning = true;

        int spawnInt = Random.Range(0, spawnPos.Length);
        Instantiate(objectToSpawn, spawnPos[spawnInt].position, spawnPos[spawnInt].rotation);
        spawnCount++;
        KillRoomDetector.mKillRoomInst.mSpawnedEnemies++;
        //GameManager.mInstance.mEnemyCount++;

        yield return new WaitForSeconds(objectSpawnTime);


        isSpawning = false;
    }
}
