using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LedgeClimber : MonoBehaviour
{
    public static LedgeClimber mClimbInst;

    public bool mClimbed;
    public bool mReached;

    [SerializeField] float mAbovHeadJumpHeight;
    [SerializeField] float mWaistJumpHeight;
    [SerializeField] float mFeetBoostLength;

    private void Start()
    {
        mClimbInst = this;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        float originalHeadJump = mAbovHeadJumpHeight;
        float originalWaistJump = mWaistJumpHeight;

        if (playerController.mPlayerInstance.playerVel.y <= 0)
        {
            //if you arent jumping, then the values are increased as normally it accounts for your jump velocity as well
            mAbovHeadJumpHeight *= 2.5f;
            mWaistJumpHeight *= 2.5f;
        }
        
            if (other.CompareTag("AboveHead"))
            {
            //when the head detector encounters a ledge, it will raise the player, almost like jumping.
                playerController.mPlayerInstance.playerVel.y += Mathf.Lerp(0, mAbovHeadJumpHeight, 0.5f);
                mClimbed = true;
                mReached = true;
            }
            else if (other.CompareTag("Waist") && mReached == false)
            {
            //and if the waist is triggered before the above head trigger, then it will do a similar action
                playerController.mPlayerInstance.playerVel.y += Mathf.Lerp(0, mWaistJumpHeight, 0.5f);
                mClimbed = true;
            }

            if (other.CompareTag("Feet") && mClimbed == true)
            {
            //once the feet touch the ledge, then the player will move forward without input from the player(you)
            playerController.mPlayerInstance.playerVel = Camera.main.transform.position * Mathf.Lerp(0, mFeetBoostLength, 0.5f) * Time.deltaTime;
                mClimbed = false;
                mReached = false;
            }

        mAbovHeadJumpHeight = originalHeadJump;
        mWaistJumpHeight = originalWaistJump;
    }
}
