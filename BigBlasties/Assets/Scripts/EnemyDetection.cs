using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    //this is all thats needed for the sphere collider
    public static EnemyDetection mEnemyDetInst;


    public bool playerInRange;

    void Start()
    {
        mEnemyDetInst = this;
    }

    public void OnTriggerEnter(Collider other) // Checks if the player enters a collider, specifically the detection sphere
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
}
