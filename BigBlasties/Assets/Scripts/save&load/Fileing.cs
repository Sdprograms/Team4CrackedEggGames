using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Fileing
{

    private string DirPath = "";
    private string name = "";

    public Fileing(string DirPath, string name) 
    {
        this.DirPath = DirPath;
        this.name = name;
    }

    public Gamedata Load() 
    {
        string path = Path.Combine(DirPath, name);
        Gamedata Ldata = null;
        if (File.Exists(path)) 
        {
            try 
            {
                string ToLoad = "";
                using(FileStream stream = new FileStream(path, FileMode.Open)) 
                {
                    using(StreamReader reader = new StreamReader(stream)) 
                    {
                        ToLoad = reader.ReadToEnd();
                    }
                }
                Ldata = JsonUtility.FromJson<Gamedata>(ToLoad);
            }
            catch (Exception e) 
            {
                Debug.LogError("Problem loading" + path + "\n" + e);
            }
        }
        return Ldata;
    }

    public void Save(Gamedata data) 
    {
        string path = Path.Combine(DirPath, name);
        try 
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string ToStore = JsonUtility.ToJson(data, true);
            using (FileStream stream = new FileStream(path, FileMode.Create)) 
            {
               using(StreamWriter writer = new StreamWriter(stream)) 
                {
                    writer.Write(ToStore);
                }
            }
        }
        catch (Exception e) 
        {
            Debug.LogError("Problem Saving" + path + "\n" + e);
        }
    }

}
