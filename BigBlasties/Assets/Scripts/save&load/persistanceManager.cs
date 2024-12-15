using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class persistanceManager : MonoBehaviour
{
    [Header("File Storage")]

    [SerializeField] private string filename;

    private GameData gameData;

    private List<dataPersistance> DPO;

    private FileHandler fileHandler;


    public static persistanceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("found more the one manager, destroyed oldest");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.fileHandler = new FileHandler(Application.persistentDataPath, filename);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.DPO = FindAllDataPersistanceObject();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        //todo
        this.gameData = fileHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log(" No Load Data");
            return;
        }

        //todo
        foreach(dataPersistance dpo in DPO) 
        {
           dpo.Load(gameData);
        }

    }

    public void SaveGame()
    {
        if(this.gameData == null) 
        {
            Debug.LogWarning("No Data Found.");
            return;
        }

        foreach(dataPersistance dpo in DPO) 
        {
            dpo.Save(ref gameData);
        }

        fileHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<dataPersistance> FindAllDataPersistanceObject()
    {
        IEnumerable<dataPersistance> DPO = FindObjectsOfType<MonoBehaviour>().OfType<dataPersistance>();

        return new List<dataPersistance>(DPO);
    }

    public bool HasGameData() 
    {
        return gameData != null;
    }
}
