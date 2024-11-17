using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LedgeClimber : MonoBehaviour
{
    public static LedgeClimber mClimbInst;

    public bool mClimbed;

    [SerializeField] float mClimbJumpHeight;
    [SerializeField] float mFeetBoostLength;

    private void Start()
    {
        mClimbInst = this;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        float originalHeadJump = mClimbJumpHeight;

        if (playerController.mPlayerInstance.playerVel.y <= 0)
        {
            //if you arent jumping, then the values are increased as normally it accounts for your jump velocity as well
            //mClimbJumpHeight *= 2.5f;
        }

        if (other.CompareTag("AboveHead") && mClimbed == false)
        {

            //when the head detector encounters a ledge, it will raise the player, almost like jumping.
            playerController.mPlayerInstance.playerVel.y += Mathf.Lerp(0, mClimbJumpHeight, 0.5f);
            mClimbed = true;

        }

        if (other.CompareTag("Feet") && mClimbed == true)
        {
            //once the feet touch the ledge, then the player will move forward without input from the player(you)
            playerController.mPlayerInstance.playerVel = Camera.main.transform.position * mFeetBoostLength * Time.deltaTime;
            mClimbed = false;
        }

        if (playerController.mPlayerInstance.jumpCount == 0)
        {
            mClimbed = false;
        }

        mClimbJumpHeight = originalHeadJump;
    }
}
