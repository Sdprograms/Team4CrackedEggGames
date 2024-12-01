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

    [SerializeField] Renderer model;

    Color originalColor;
    private void Start()
    {
        //originalColor = model.material.color;

        if (interactType == interactableType.Door) 
        {
            isOpen = false;

            if (typeDoor == doorType.GreyDoor) // So far, solid colors. Might make flash instead.
            {
                model.material.color = Color.grey;
            }
            else if (typeDoor == doorType.RedDoor)
            {
                model.material.color = Color.red;
            }
            else if (typeDoor == doorType.BlueDoor)
            {
                model.material.color = Color.blue;
            }
            else if (typeDoor == doorType.GreenDoor)
            {
                model.material.color = Color.green;
            }
            else if (typeDoor == doorType.BossDoor) //Need something different than a color for boss door.
            {
                model.material.color = Color.yellow;
            }
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
