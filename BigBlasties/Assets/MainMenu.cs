using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadSceneAsync("zSLevel 1 Tutorial");

    }

    public void Return() 
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }

    public void Credits() 
    {
        SceneManager.LoadSceneAsync("Credits");
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
