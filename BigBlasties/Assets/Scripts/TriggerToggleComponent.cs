using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToggleComponent : MonoBehaviour
{
    [SerializeField] MonoBehaviour objectToToggleOn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (objectToToggleOn != null)
            {
                objectToToggleOn.enabled = true;
            }

        }
    }


}
