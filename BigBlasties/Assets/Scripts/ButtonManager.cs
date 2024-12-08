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
    [SerializeField] TMP_Text mSensText;
    [SerializeField] Toggle mInvertY;

    private void Start()
    {
        buttonManager = this;
        Instantiate(mInvertY);
        //mInvertY = GetComponent<UnityEngine.UI.Toggle>();
    }

    public void Resume()
    {
        //unpauses the game
        GameManager.mInstance.StateUnpaused();
    }

    public void RespawnPlayer()
    {
        GameManager.mInstance.mPlayerController.Respawn();
        GameManager.mInstance.StateUnpaused();
    }

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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
           //only works in a build
            Application.Quit();
#endif
    }
}
