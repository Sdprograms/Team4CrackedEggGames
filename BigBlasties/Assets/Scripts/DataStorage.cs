using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataStorage : MonoBehaviour
{
    public static DataStorage mStorInst;
    [SerializeField] GameObject mStor;
    // Start is called before the first frame update

    public float mAllVol;
    public float mMusVol;
    public float mGenVol;

    public float mSensVal;

    string mName;

    void Start()
    { 
        if (mStor == null)
        {
            mStorInst = this;
            mStor = GameObject.Find("DataStorage");
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        mName = SceneManager.GetActiveScene().name;

        mSensVal = 450f;
        mAllVol = 0.5f;
        mMusVol = 0.5f;
        mGenVol = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {

        if (ButtonManager.buttonManager != null)
        {
            if (mSensVal != cameraController.camInstance.GetSensitivity())
            {
                cameraController.camInstance.SetSensitivity((int)mSensVal);
                ButtonManager.buttonManager.mSensitivitySlide.value = mSensVal;
                ButtonManager.buttonManager.mSensText.text = mSensVal.ToString();
            }

            if (mMusVol == mAllVol && mGenVol == mAllVol)
            {
                ButtonManager.buttonManager.NewGeneralVolume(mAllVol);
                ButtonManager.buttonManager.NewMusicVolume(mAllVol);
            }
            else
            {
                ButtonManager.buttonManager.NewGeneralVolume(mGenVol);
                ButtonManager.buttonManager.NewMusicVolume(mMusVol);
            }
        }
    }
}
