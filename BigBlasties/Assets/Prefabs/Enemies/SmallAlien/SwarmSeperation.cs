using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmSeperation : MonoBehaviour
{
    GameObject[] AI;
    [SerializeField] float SpaceBetween;
    

    void Start()
    {
        AI = GameObject.FindGameObjectsWithTag("Swarmer");
        SeparateAI();
    }

    // Update is called once per frame
    void Update()
    {
        AI = GameObject.FindGameObjectsWithTag("Swarmer");
        SeparateAI();
    }

    void SeparateAI()
    {
        for (int i = 0; i < AI.Length; i++)
        {
            for (int j = i + 1; j < AI.Length; j++)
            {
                Vector3 direction = AI[j].transform.position - AI[i].transform.position;
                float distance = direction.magnitude;

                if (distance < SpaceBetween)
                {
                    Vector3 separation = direction.normalized * (SpaceBetween - distance) / 2;
                    AI[i].transform.position -= separation;
                    AI[j].transform.position += separation;
                }
            }
        }
    }
}
