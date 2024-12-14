using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunRotation : MonoBehaviour
{
    static public GunRotation mGunRotInst;

    [SerializeField] LayerMask mLayerMask;

    [SerializeField] GameObject mRotationPoint;
    //[SerializeField] string mTag;

    [SerializeField] float mMoveTime;

    [SerializeField] float mRayDistance;
    Vector3 mOrigTrans;
    Vector3 mHeldTrans;
    Vector3 mLoweredTrans;

    Quaternion mOrigRot;
    Quaternion mHeldRot;

    public RaycastHit mHit;

    public bool mCanFire;

    // Start is called before the first frame update
    void Start()
    {
        mGunRotInst = this;

        mOrigTrans = mRotationPoint.transform.localPosition;
        mOrigRot = mRotationPoint.transform.localRotation;


        mHeldRot = Quaternion.Euler(20.392f, -36.438f, -14.426f);
        mHeldTrans = new Vector3(0.264f, -0.232f, 0.347f);

        mCanFire = true; // Add this bool to the playerController to dictate shooting condition
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(cameraController.camInstance.transform.position, cameraController.camInstance.transform.forward, Color.red);
        //CheckWalls();
    }

    void CheckWalls()
    {

        if (Physics.Raycast(cameraController.camInstance.transform.position, cameraController.camInstance.transform.forward, out mHit, mRayDistance, mLayerMask))
        {
            Debug.Log($"The Tag gotten is {mHit.collider.name}");
            StartCoroutine(ToBody());
            Debug.Log("The ray hit where it should rotate");


            if (mHit.collider.CompareTag("Switch"))
            {
                GameManager.mInstance.mShowNoti = true;
            }
        }
        else
        {
            StartCoroutine(ToAim());
           // Debug.Log("Rot orig");

            GameManager.mInstance.mShowNoti = false;

        }
        
    }

    IEnumerator ToBody()
    {
        //mRotationPoint.transform.SetLocalPositionAndRotation(mHeldTrans, mHeldRot);
        mRotationPoint.transform.localPosition = Vector3.Lerp(mHeldTrans, mOrigTrans, Time.deltaTime * mMoveTime);
        mRotationPoint.transform.localRotation = Quaternion.Slerp(mHeldRot, mOrigRot, Time.deltaTime * mMoveTime);
        mCanFire = false;
        yield return null;
    }

    IEnumerator ToAim()
    {
        // mRotationPoint.transform.SetLocalPositionAndRotation(mOrigTrans, mOrigRot);
        mRotationPoint.transform.localPosition = Vector3.Lerp(mOrigTrans, mHeldTrans, Time.deltaTime * mMoveTime);
        mRotationPoint.transform.localRotation = Quaternion.Slerp(mOrigRot, mHeldRot, Time.deltaTime * mMoveTime);
        mCanFire = true;
        yield return null;
    }
}
