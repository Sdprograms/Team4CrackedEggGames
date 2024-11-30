using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interactable : MonoBehaviour
{
    enum interactableType {Door };
    enum doorType {GreyDoor, RedDoor, BlueDoor, GreenDoor, BossDoor};
    [SerializeField] interactableType interactType;
    [SerializeField] doorType typeDoor;
    [SerializeField] bool isOpen;
    [SerializeField] bool isLocked;

    private void Start()
    {
        if (interactType == interactableType.Door)
        {
            isOpen = false;
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetButtonDown("Interact")) //should normally set the key E to an input setting.
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
                    else if (isLocked && typeDoor == doorType.GreyDoor && GameManager.mInstance.mPlayerController.GetKeys() > 0)
                    {
                        transform.Rotate(Vector3.up * 90);
                        isOpen = true;
                        GameManager.mInstance.mPlayerController.RemoveKey();
                        isLocked = false;
                    }
                    else if (isLocked && typeDoor == doorType.GreenDoor && GameManager.mInstance.mPlayerController.getGreenKey() == true)
                    {
                        transform.Rotate(Vector3.up * 90);
                        isOpen = true;
                        isLocked = false;
                    }
                    else if (isLocked && typeDoor == doorType.RedDoor && GameManager.mInstance.mPlayerController.getRedKey() == true)
                    {
                        transform.Rotate(Vector3.up * 90);
                        isOpen = true;
                        isLocked = false;
                    }
                    else if (isLocked && typeDoor == doorType.BlueDoor && GameManager.mInstance.mPlayerController.getBlueKey() == true)
                    {
                        transform.Rotate(Vector3.up * 90);
                        isOpen = true;
                        isLocked = false;
                    }
                    else if (isLocked && typeDoor == doorType.BossDoor && GameManager.mInstance.mPlayerController.getBossKey() == true)
                    {
                        transform.Rotate(Vector3.up * 90);
                        isOpen = true;
                        isLocked = false;
                    }

                }
            }
        }
    }

}
