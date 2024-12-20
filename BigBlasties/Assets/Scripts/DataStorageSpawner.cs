using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStorageSpawner : MonoBehaviour
{
    [SerializeField] GameObject dataStorage;

    // Start is called before the first frame update
    void Start()
    {
        if (DataStorage.mStorInst == null)
        {
            Instantiate(dataStorage);
        }
    }
}
