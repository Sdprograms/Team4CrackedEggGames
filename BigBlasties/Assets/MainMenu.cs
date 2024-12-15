using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button LoadButton;


    private void Start()
    {
        if (!persistanceManager.instance.HasGameData()) 
        {
            LoadButton.interactable = false;
        }
    }

    public void Play()
    {
       persistanceManager.instance.NewGame();
        SceneManager.LoadSceneAsync("zSLevel 2 Mining Facility");

    }

    public void LoadGameClicked() 
    {
        SceneManager.LoadSceneAsync("zSLevel 2 Mining Facility");
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
