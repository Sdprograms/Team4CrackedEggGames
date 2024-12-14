using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class manager : MonoBehaviour
{
    public Transform transform;

    public int health = 100;

    public void Save() 
    {
        data playerdata = new data();
        playerdata.position = new float[] { transform.position.x, transform.position.y, transform.position.z };
        playerdata.health = health;

        string friday = JsonUtility.ToJson(playerdata);
        string path = Application.persistentDataPath + "/playerdata.friday";
        System.IO.File.WriteAllText(path, friday);
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/playerdata.friday";
        if(File.Exists(path)) 
        {
            string friday = System.IO.File.ReadAllText(path);
            data load = JsonUtility.FromJson<data>(friday);

            //update players position
            transform.position = new Vector3(load.position[0], load.position[1], load.position[2]);
            Vector3 loadposition = new Vector3(load.position[0], load.position[1], load.position[2]);

            // load all the values from player
            transform.position += loadposition;
            health = load.health;
        }
        else 
        {
            Debug.LogWarning("file not found");
        }

    }


}
