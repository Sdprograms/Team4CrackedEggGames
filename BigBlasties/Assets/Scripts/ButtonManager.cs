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

    public void NewLookSensitivity(int newSens)
    {
       // cameraController.camInstance.SetSensitivity(newSens);
    }

    public void ReturnToPause()
    {
        //turns off the settings menu and on the pause menu
        GameManager.mInstance.SetActiveMenu(GameManager.mInstance.GetMenuPause());
    }

    public void Reset()
    {
        //resets the game to the current scene(level) at its default and unpauses the game
      //  GameManager.mInstance.UnfreezeGame();
      
        GameManager.mInstance.StateUnpaused();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
