using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolume : MonoBehaviour
{

    [SerializeField] Slider volumeS;

    void Start()
    {
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeS.value;
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
