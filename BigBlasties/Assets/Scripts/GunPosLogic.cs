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

    Vector3 CurrTrans;
    Quaternion CurrRot;

    public void Awake()
    {
        gunPosInst = this;
        CurrTrans = this.transform.localPosition;
        CurrRot = this.transform.localRotation;
    }

    //adjusts the GunPos to an appropriate position and rotation
    public IEnumerator OrientGunMesh(Mesh currMesh)
    {
        if (currMesh == mGrenadeLauncher)
        {
            this.gameObject.transform.localPosition = new Vector3(0.7f, -0.3f, 1.2f);
            this.gameObject.transform.localRotation = Quaternion.Euler(180, 0, 0);
        }

        else if (currMesh == mAutoLaserPistol)
        {
            this.gameObject.transform.localPosition = CurrTrans;
            this.gameObject.transform.localRotation = CurrRot;
        }

        else if (currMesh == mRocketLauncher)
        {
            this.gameObject.transform.localPosition = CurrTrans;
            this.gameObject.transform.localRotation = CurrRot;
        }

        else if (currMesh == mHighPoweredLaser)
        {
            this.gameObject.transform.localPosition = new Vector3(0.6f, -0.45f, 1.4f);
            this.gameObject.transform.localRotation = Quaternion.Euler(0, -90, 90);

        }
        yield return null;
    }

}
