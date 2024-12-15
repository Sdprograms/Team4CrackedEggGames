using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileHandler : MonoBehaviour
{

    private string path = "";
    private string name = "";

    public FileHandler(string path, string name) 
    {
        this.path = path;
        this.name = name;
    }

    public GameData Load() 
    {
        string fullpath = Path.Combine(path, name);
        GameData loaded = null;
        if (File.Exists(fullpath)) 
        {
            try 
            {
                string ToLoad = "";
                using (FileStream stream = new FileStream(fullpath, FileMode.Open)) 
                {
                    using ( StreamReader reader = new StreamReader(stream))
                    {
                        ToLoad = reader.ReadToEnd();
                    }
                }

                loaded = JsonUtility.FromJson<GameData>(ToLoad);
            }
            catch(Exception e) 
            {
                Debug.LogError("problem loading data" + fullpath + "\n" + e);
            }
        }
        return loaded;
    }

    public void Save(GameData data) 
    {
        string fullpath = Path.Combine(path, name);
        try 
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullpath));

            string stored = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullpath, FileMode.Create)) 
            {
                using (StreamWriter writer = new StreamWriter(stream)) 
                {
                    writer.Write(stored);
                }
            }
        }
        catch (Exception e) 
        {
            Debug.LogError("problem saving data" + fullpath + "\n" + e);
        }
    }
}
