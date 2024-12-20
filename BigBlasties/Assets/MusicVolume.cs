using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolume : MonoBehaviour
{
    public static MusicVolume mInst;
    [SerializeField] public Slider volumeS;

    void Start()
    {
        mInst = this;
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", 0.5f);
            Load();
        }
        else
        {
            PlayerPrefs.SetFloat("volume", 0.5f);
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeS.value;
        DataStorage.mStorInst.mAllVol = volumeS.value;
        DataStorage.mStorInst.mMusVol = volumeS.value;
        DataStorage.mStorInst.mGenVol = volumeS.value;
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("volume", volumeS.value);
    }

    public void Load()
    {
        volumeS.value = PlayerPrefs.GetFloat("volume");
    }

}
