using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LedgeClimber : MonoBehaviour
{
    public static LedgeClimber mClimbInst;

    public bool mClimbed;

    [SerializeField] GameObject mAboveHead;
    [SerializeField] float mClimbJumpHeight;
    [SerializeField] float mFeetBoostLength;

    private void Start()
    {
        mClimbInst = this;
    }
    private void Update()
    {
        if (playerController.mPlayerInstance.jumpCount == 0 || playerController.mPlayerInstance.characterController.isGrounded)
        {
            mClimbed = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        float originalHeadJump = mClimbJumpHeight;

        if (other.CompareTag("AboveHead") && mClimbed == false)
        {

            //when the head detector encounters a ledge, it will raise the player, almost like jumping.
            playerController.mPlayerInstance.playerVel.y += Mathf.Lerp(0, mClimbJumpHeight, 0.5f);
            mClimbed = true;

            mAboveHead = other.gameObject;

        }

        if (other.CompareTag("Feet") && mClimbed == true)
        {
            //once the feet touch the ledge, then the player will move forward without input from the player(you)
            playerController.mPlayerInstance.playerVel = transform.forward * mFeetBoostLength * Time.deltaTime;
            mClimbed = false;
        }

        mClimbJumpHeight = originalHeadJump;
    }
}
