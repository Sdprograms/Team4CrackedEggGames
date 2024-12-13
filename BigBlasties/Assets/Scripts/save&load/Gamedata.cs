using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Gamedata
{
    public int Deaths;

    public Vector3 position;

    public Gamedata() 
    {
        this.Deaths = 0;
        position = Vector3.zero;
    }

}
