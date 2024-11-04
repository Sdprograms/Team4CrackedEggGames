using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void Resume()
    {
        //unpauses the game
        GameManager.mInstance.StateUnpaused();
    }

    public void ToSettings()
    {
        GameManager.mInstance.StateSettingsOn();
    }

    public void ReturnToPause()
    {
        //turns off the settings menu and on the pause menu
        GameManager.mInstance.StateSettingsOff();
    }

}
