using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    enum interactableType {Door };
    [SerializeField] interactableType interactType;
    [SerializeField] bool isOpen;
    [SerializeField] bool isLocked;
    private void Start()
    {
        if (interactType == interactableType.Door)
        {
            isOpen = false;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetButton("Interact")) //should normally set the key E to an input setting.
        {
            if(interactType == interactableType.Door)
            {
                if (!isOpen)
                {
                    if (!isLocked)
                    {
                        transform.Rotate(Vector3.up * 90);
                        isOpen = true;
                    }
                    else if (isLocked && GameManager.mInstance.mPlayerController.GetKeys() > 0) 
                    {
                        transform.Rotate(Vector3.up * 90);
                        isOpen = true;
                        GameManager.mInstance.mPlayerController.RemoveKey();
                    }

                }
            }
            
        }
    }
}
