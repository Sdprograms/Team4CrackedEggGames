using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Manager : MonoBehaviour
{

    [Header("File Config")]

    [SerializeField] private string filename;

    private Gamedata gameData;

    private List<implementData> IdataObjects;

    private Fileing Dhandler;

    public static Manager Instance {get; private set;}

    public void Awake()
    {
        if(Instance != null) 
        {
            Debug.LogError("To many managers");
        }
        Instance = this;
    }

    public void LoadGame() 
    {
        
        this.gameData = Dhandler.Load();

        if(this.gameData == null) 
        {
            Debug.Log("no data found");
        }

      
        foreach(implementData IDO in IdataObjects) 
        {
            IDO.loadData(gameData);
        }

    }

    public void SaveGame() 
    {
        foreach(implementData IDO in IdataObjects) 
        {
            IDO.saveData(ref gameData);
        }

        Dhandler.Save(gameData);

    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<implementData> findIDO() 
    {
        IEnumerable<implementData> IDO = FindObjectsOfType<MonoBehaviour>().OfType<implementData>();

        return new List<implementData>(IDO);
    }

}
