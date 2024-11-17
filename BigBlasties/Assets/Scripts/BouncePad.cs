using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    BouncePad mBounceInst;
    [SerializeField] int boostVelocity;
    public bool isEntered;

    private int jumpCountPostBoost;

    private void Awake()
    {
        mBounceInst = this;
        jumpCountPostBoost = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEntered = true;
        }
        if (isEntered)
        {
            playerController.mPlayerInstance.playerVel.y = boostVelocity;
            playerController.mPlayerInstance.jumpCount = jumpCountPostBoost;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEntered = false;
        }
    }
}
