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

    Quaternion CurrRot;

    Vector3 PistolTransform;
    Vector3 GLTransform;
    Vector3 RLTransform;
    Vector3 HPLTransform;

    public Quaternion mShootPosRot;
    public Quaternion mShootPosGrenadeRot;

    public bool isGrenadeLauncher;
    public bool isRocketLauncher;
    public bool isAutoLaserPistol;
    public bool isHighPoweredLauncher;

    public void Awake()
    {
        gunPosInst = this;
        CurrRot = Quaternion.Euler(0f, 0f, 0f);

        PistolTransform = new Vector3(0.21f, -0.52f, 0.86f);
        GLTransform = new Vector3(0f, -0.17f, 1.33f);
        RLTransform = new Vector3(0f, -0.17f, 0.92f);
        HPLTransform = new Vector3(0f, -0.52f, 1.1f);

        mShootPosRot = Quaternion.Euler(0f, 0f, 0f);
        mShootPosGrenadeRot = Quaternion.Euler(180f, 0f, 0f);
    }

    //adjusts the GunPos to an appropriate position and rotation
    public IEnumerator OrientGunMesh(Mesh currMesh)
    {
        isGrenadeLauncher = false;
        isRocketLauncher = false;
        isHighPoweredLauncher = false;
        isAutoLaserPistol = false;


        if (currMesh == mGrenadeLauncher)
        {
            isGrenadeLauncher = true;
            this.gameObject.transform.localPosition = GLTransform;
            this.gameObject.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            mShootPos.transform.localPosition = new Vector3(0.001f, 0.0194f, -0.1251f);
            mShootPos.transform.localRotation = mShootPosGrenadeRot;
        }

        else if (currMesh == mAutoLaserPistol)
        {
            isAutoLaserPistol = true;
            this.gameObject.transform.localPosition = PistolTransform;
            this.gameObject.transform.localRotation = CurrRot;
            mShootPos.transform.localPosition = new Vector3(-0.0063f, -0.0515f, 0.0196f);
            mShootPos.transform.localRotation = mShootPosRot;
        }

        else if (currMesh == mRocketLauncher)
        {
            isRocketLauncher = true;
            this.gameObject.transform.localPosition = RLTransform;
            this.gameObject.transform.localRotation = CurrRot;
            mShootPos.transform.localPosition = new Vector3(0.001f, -0.148f, 0.02f);
            mShootPos.transform.localRotation = mShootPosRot;
        }

        else if (currMesh == mHighPoweredLaser)
        {
            isHighPoweredLauncher = true;
            this.gameObject.transform.localPosition = HPLTransform;
            this.gameObject.transform.localRotation = Quaternion.Euler(0, -90, 0);
            mShootPos.transform.localPosition = new Vector3(0.001f, -0.024f, -0.001f);
            mShootPos.transform.localRotation = mShootPosRot;
        }
        yield return null;
    }

}
