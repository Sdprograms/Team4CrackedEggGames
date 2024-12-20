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

    public bool mAVChanged;

    public bool mSensChanged;

    string mName;

    void Awake()
    { 
        if (mStorInst == null)
        {
            mStorInst = this;
            mStor = GameObject.Find("DataStorage");
            DontDestroyOnLoad(mStor);
        }
        else
        {
            Destroy(mStor);
        }
        mName = SceneManager.GetActiveScene().name;

        mSensVal = 450f;
    }

    // Update is called once per frame
    void Update()
    {

        if (mAVChanged)
        {
            //if (ButtonManager.buttonManager.mMusicVolSlide != null)
            //{ 
            //    //ButtonManager.buttonManager.mMusicVolSlide.value = mAllVol;
            //}

        }

        if (ButtonManager.buttonManager != null)
        {
            if (mSensVal != cameraController.camInstance.GetSensitivity())
            {
                cameraController.camInstance.SetSensitivity((int)mSensVal);
                ButtonManager.buttonManager.mSensitivitySlide.value = mSensVal;
                ButtonManager.buttonManager.mSensText.text = mSensVal.ToString();
                Debug.Log("Made the sensitivity persist");
            }

        }
    }
}
