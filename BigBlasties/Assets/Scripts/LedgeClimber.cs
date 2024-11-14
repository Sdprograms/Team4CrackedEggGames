using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LedgeClimber : MonoBehaviour
{
    public static LedgeClimber mClimbInst;

    [SerializeField] GameObject mAboveHead;
    [SerializeField] GameObject mWaist;
    [SerializeField] GameObject mFeet;

    public bool mClimbed;
    public bool mReached;

    [SerializeField] int mAbovHeadJumpHeight;
    [SerializeField] int mWaistJumpHeight;
    [SerializeField] int mFeetBoostLength;

    private void Start()
    {
        mClimbInst = this;
    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Ledge"))
    //    {
    //        playerController.mPlayerInstance.playerVel.y = mAbovHeadJumpHeight;
    //        yield return new WaitForSeconds(0.5f);

    //    }
    //}
    private void OnCollisionEnter(Collision collision)
    {
        {
            if (mAboveHead.CompareTag("AboveHead"))
            {
                //playerController.mPlayerInstance.playerVel.y = mAbovHeadJumpHeight;
                mClimbed = true;
                mReached = true;
            }
            else if (mWaist.CompareTag("Waist") && mReached == false)
            {
                playerController.mPlayerInstance.playerVel.y = mWaistJumpHeight;
                mClimbed = true;
            }

            if (mFeet.CompareTag("Feet") && mClimbed == true)
            {
                playerController.mPlayerInstance.playerVel.z = mFeetBoostLength;
                mClimbed = false;
                mReached = false;
            }

        }
    }
}
