using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformpath : MonoBehaviour
{
    public Transform GetWP(int WPI) 
    {
        return transform.GetChild(WPI);
    }

    public int GetNextWPI(int currentWPI) 
    {
        int nextWPI = currentWPI + 1;

        if(nextWPI == transform.childCount)
        {
            nextWPI = 0;
        }
        return nextWPI;
    }
}
