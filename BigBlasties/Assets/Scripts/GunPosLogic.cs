using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunPosLogic : MonoBehaviour
{
    static public GunPosLogic gunPosInst;
    [SerializeField] Mesh mAutoLaserPistol;
    [SerializeField] Mesh mHighPoweredLaser;
    [SerializeField] Mesh mRocketLauncher;
    [SerializeField] Mesh mGrenadeLauncher;

    [SerializeField] GameObject mShootPos;

    Vector3 CurrTrans;
    Quaternion CurrRot;

    Vector3 GLTransform;
    Vector3 HPLTransform;

    public void Awake()
    {
        gunPosInst = this;
        CurrTrans = this.transform.localPosition;
        CurrRot = this.transform.localRotation;

        GLTransform = new Vector3(0.065f, 0.473f, 0.685f);
        HPLTransform = new Vector3(0f, 0.21f, .95f);
    }

    //adjusts the GunPos to an appropriate position and rotation
    public IEnumerator OrientGunMesh(Mesh currMesh)
    {
        if (currMesh == mGrenadeLauncher)
        {
            this.gameObject.transform.localPosition = GLTransform;
            this.gameObject.transform.localRotation = Quaternion.Euler(180, 0, 0);
            mShootPos.transform.localPosition = new Vector3(0.0009f, 0.0204f, -0.1212f);
        }

        else if (currMesh == mAutoLaserPistol)
        {
            this.gameObject.transform.localPosition = CurrTrans;
            this.gameObject.transform.localRotation = CurrRot;
            mShootPos.transform.localPosition = new Vector3(-0.0092f, -0.0566f, 0.0166f);
        }

        else if (currMesh == mRocketLauncher)
        {
            this.gameObject.transform.localPosition = CurrTrans;
            this.gameObject.transform.localRotation = CurrRot;
            mShootPos.transform.localPosition = new Vector3(0.0015f, -0.157f, 0.0186f);
        }

        else if (currMesh == mHighPoweredLaser)
        {
            this.gameObject.transform.localPosition = HPLTransform;
            this.gameObject.transform.localRotation = Quaternion.Euler(0, -90, 90);
            mShootPos.transform.localPosition = new Vector3(0.0011f, -0.0208f, 0.0004f);
        }
        yield return null;
    }

}
