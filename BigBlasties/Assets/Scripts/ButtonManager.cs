using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ButtonManager : MonoBehaviour
{
    ButtonManager buttonManager;
    [SerializeField] Slider mSensitivitySlide;
    [SerializeField] Slider mMusicVolSlide;
    [SerializeField] Slider mGeneralVolSlide;
    [SerializeField] TMP_Text mSensText;
    [SerializeField] TMP_Text mMusText;
    [SerializeField] TMP_Text mGenText;
    [SerializeField] Toggle mInvertY;

    private void Start()
    {
        buttonManager = this;

        //properly assigns the toggle, could potentially delete the game object after its located to save memory
        GameObject mToggleHolder = GameObject.Find("UI(official)/Menus/SettingsScreen/InvertLook/InvertLookToggle");

        if (mToggleHolder != null)
        {
            mInvertY = mToggleHolder.GetComponent<Toggle>(); mInvertY = mToggleHolder.GetComponent<Toggle>();
        }
    }

    public void Resume()
    {
        //unpauses the game
        GameManager.mInstance.StateUnpaused();
    }

    //public void RespawnPlayer()
    //{
    //    GameManager.mInstance.mPlayerController.Respawn();
    //    GameManager.mInstance.StateUnpaused();
    //}

    public void ToSettings()
    {
        GameManager.mInstance.StateSettingsOn();
    }

    public void NewLookSensitivity()
    {
        int sensVal = ((int)mSensitivitySlide.value);
        string newVal = mSensitivitySlide.value.ToString(); // grabs the values from the slider
        mSensText.text = newVal; // sets the value of the text

        cameraController.camInstance.lookSensitivity = sensVal;
    }

    public void NewMusicVolume()
    {
        float musVal = (mMusicVolSlide.value * 0.1f);
        string newVal = mMusicVolSlide.value.ToString(); // grabs the values from the slider
        mMusText.text = newVal; // sets the value of the text

        if (SoundEffects.noiseMaker != null)
        {
            SoundEffects.noiseMaker.UnPausedVol = musVal;
            SoundEffects.noiseMaker.PausedVol = musVal / 3;
        }
    }

    public void NewGeneralVolume()
    {
        float genVal = (mMusicVolSlide.value * 0.1f);
        string newVal = mMusicVolSlide.value.ToString(); // grabs the values from the slider
        mGenText.text = newVal; // sets the value of the text

        //SoundEffects.noiseMaker.UnPausedVol = musVal;
        if (playerController.mPlayerInstance != null)
        {
            playerController.mPlayerInstance.weaponAudioSource.volume = genVal;
            playerController.mPlayerInstance.reloadAudioSource.volume = genVal;
            playerController.mPlayerInstance.mStepsSource.volume = genVal;
        }
        if (DestructableObject.instance != null)
        {
            DestructableObject.instance.audioSource.volume = genVal;
        }
        if (AddedSound.instance != null)
        {
            AddedSound.instance.audioSource.volume = genVal;
        }
        if (Checkpoint.instance != null)
        {
            Checkpoint.instance.audioSource.volume = genVal;
        }
        if (Interactable.instance != null)
        {
            Interactable.instance.audioSource.volume = genVal;
        }
        if (SoundEffects.noiseMaker != null)
        {
            SoundEffects.noiseMaker.weaponSource.volume = genVal;
            SoundEffects.noiseMaker.swapWeaponSouce.volume = genVal;
            SoundEffects.noiseMaker.levelSoundSource.volume = genVal;
        }
        if (SwitchGeneral.switchGeneralInst != null)
        {

            SwitchGeneral.switchGeneralInst.audioSource.volume = genVal;
        }
    }




    public void InvertLook()
    {
        cameraController.camInstance.invertY = mInvertY.isOn;
    }

    public void ReturnToPause()
    {
        //turns off the settings menu and on the pause menu
        GameManager.mInstance.SetActiveMenu(GameManager.mInstance.GetMenuPause());
    }

    public void Reset()
    {
        //resets the game to the current scene(level) at its default and unpauses the game
        GameManager.mInstance.StateUnpaused();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (GameManager.mInstance.mListOfKillRoomDets.Count != 0)
        {
            KillRoomDetector.mKillRoomInst.mDeadMansDoor.transform.position = KillRoomDetector.mKillRoomInst.mDoorOrigPos;
        }
    }

    public void Quit()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
