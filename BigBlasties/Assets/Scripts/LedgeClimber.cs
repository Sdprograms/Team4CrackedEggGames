using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LedgeClimber : MonoBehaviour
{
    public static LedgeClimber mClimbInst;

    public bool mClimbed;

   // [SerializeField] GameObject mAboveHead;
    [SerializeField] float mClimbJumpHeight;
    [SerializeField] float mFeetBoostLength;

    private void Start()
    {
        mClimbInst = this;
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("AboveHead") && playerController.mPlayerInstance.playerVel.y < -.05f)
        {
            playerController.mPlayerInstance.playerVel.y = 0;
        }

        if (other.CompareTag("AboveHead"))
        {
            if (mClimbed == true)
            {
                mClimbed = false;
                Debug.Log("mClimbed returned to false");
            }

            if (mClimbed == false)
            {
                Debug.Log("Has touched wall");
                //when the head detector encounters a ledge, it will raise the player, almost like jumping.
                playerController.mPlayerInstance.playerVel.y += Mathf.Lerp(0, mClimbJumpHeight, 0.5f);
                Debug.Log("Has climbed");
                mClimbed = true;
                Debug.Log("mClimbed is true");
            }
        }

        if (other.CompareTag("Feet") && mClimbed == true)
        {
            Debug.Log("Reached Feet");
            //once the feet touch the ledge, then the player will move forward without input from the player(you)
            playerController.mPlayerInstance.playerVel += Camera.main.transform.forward * mFeetBoostLength * Time.deltaTime;
            playerController.mPlayerInstance.playerVel.y = 0;
            mClimbed = false;
        }


    }
}
