using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotation : MonoBehaviour
{
    static public GunRotation mGunRotInst;

    [SerializeField] LayerMask mLayerMask;

    [SerializeField] GameObject mRotationPoint;
    [SerializeField] string mTag;

    [SerializeField] float mMoveTime;

    [SerializeField] float mRayDistance;
    Vector3 mOrigTrans;
    Vector3 mHeldTrans;
    Vector3 mLoweredTrans;

    Quaternion mOrigRot;
    Quaternion mHeldRot;

    RaycastHit mHit;

    public bool mCanFire;

    // Start is called before the first frame update
    void Start()
    {
        mGunRotInst = this;

        mOrigTrans = mRotationPoint.transform.localPosition;
        mOrigRot = mRotationPoint.transform.localRotation;

        mHeldRot = Quaternion.Euler(9f, -42f, 7f);
        mHeldTrans = new Vector3(0.6f, -0.8f, 0.6f);

        mCanFire = true; // Add this bool to the playerController to dictate shooting condition
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(cameraController.camInstance.transform.position, cameraController.camInstance.transform.forward, Color.red);
        CheckWalls();
    }

    void CheckWalls()
    {

        if (Physics.Raycast(cameraController.camInstance.transform.position, cameraController.camInstance.transform.forward, out mHit, mRayDistance, mLayerMask))
        {
            Debug.Log($"The Tag gotten is {mHit.collider.name}");

            StartCoroutine(ToBody());
            Debug.Log("The ray hit where it should rotate");

            mCanFire = false;
        }
        else
        {

            StartCoroutine(ToAim());
            Debug.Log("Rot orig");

            mCanFire = true;
        }
        
    }

    IEnumerator ToBody()
    {
        mRotationPoint.transform.SetLocalPositionAndRotation(mHeldTrans, mHeldRot);
        yield return new WaitForSeconds(mMoveTime);
    }

    IEnumerator ToAim()
    {
        mRotationPoint.transform.SetLocalPositionAndRotation(mOrigTrans, mOrigRot);
        yield return new WaitForSeconds(mMoveTime);
    }
}
