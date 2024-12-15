using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Interactable : MonoBehaviour
{
    enum interactableType {Door, Switch };
    enum doorType {GreyDoor, RedDoor, BlueDoor, GreenDoor, BossDoor};
    enum switchType {Rotate, Railroad }
    [SerializeField] interactableType interactType;

    [Header("---If Door ---")]
    [SerializeField] doorType typeDoor;
    [SerializeField] bool isOpen;
    [SerializeField] bool isLocked;
    [SerializeField] Renderer model;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip lockClip;
    [SerializeField] AudioClip openClip;
    [SerializeField] float openSpeed;
    private float currentRot;

    [Header("---If Switch---")]
    [SerializeField] SwitchGeneral switchScript;
    [SerializeField] switchType typeSwitch;

    bool activateable;
    Color originalColor;
    private void Start()
    {
        //originalColor = model.material.color;
        activateable = true;

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
        GameManager.mInstance.mIsLocked = isLocked;
        GameManager.mInstance.mShowNoti = true;
        if (other.CompareTag("Player") && Input.GetButton("Interact") && activateable == true) //should normally set the key E to an input setting.
        {
            
            if(interactType == interactableType.Door)
            {
                if (!isOpen)
                {
                    if (!isLocked)
                    {
                        //transform.Rotate(Vector3.up * 90);
                        StartCoroutine(OpenDoor());
                        audioSource.PlayOneShot(openClip);
                        isOpen = true;
                    }
                    else if (isLocked && typeDoor == doorType.GreyDoor && GameManager.mInstance.mPlayerController.GetKeys() > 0)
                    {
                        //transform.Rotate(Vector3.up * 90);
                        StartCoroutine(OpenDoor());
                        audioSource.PlayOneShot(openClip);
                        isOpen = true;
                        GameManager.mInstance.mPlayerController.RemoveKey();
                        isLocked = false;
                    }
                    else if (isLocked && typeDoor == doorType.GreenDoor && GameManager.mInstance.mPlayerController.getGreenKey() == true)
                    {
                        //transform.Rotate(Vector3.up * 90);
                        StartCoroutine(OpenDoor());
                        audioSource.PlayOneShot(openClip);
                        isOpen = true;
                        isLocked = false;
                    }
                    else if (isLocked && typeDoor == doorType.RedDoor && GameManager.mInstance.mPlayerController.getRedKey() == true)
                    {
                        //transform.Rotate(Vector3.up * 90);
                        StartCoroutine(OpenDoor());
                        audioSource.PlayOneShot(openClip);
                        isOpen = true;
                        isLocked = false;
                    }
                    else if (isLocked && typeDoor == doorType.BlueDoor && GameManager.mInstance.mPlayerController.getBlueKey() == true)
                    {
                        //transform.Rotate(Vector3.up * 90);
                        StartCoroutine(OpenDoor());
                        audioSource.PlayOneShot(openClip);
                        isOpen = true;
                        isLocked = false;
                    }
                    else if (isLocked && typeDoor == doorType.BossDoor && GameManager.mInstance.mPlayerController.getBossKey() == true)
                    {
                        //transform.Rotate(Vector3.up * 90);
                        StartCoroutine(OpenDoor());
                        audioSource.PlayOneShot(openClip);
                        isOpen = true;
                        isLocked = false;
                    }
                    else
                    {
                        if (audioSource != null && lockClip != null)
                        {
                            audioSource.time = 0.5f;
                            audioSource.PlayOneShot(lockClip);
                        }
                    }

                }
            }

            if(interactType == interactableType.Switch && typeSwitch == switchType.Rotate)
            {
                if (switchScript != null)
                {
                    switchScript.rotateObject();
                }
            }
            else if (interactType == interactableType.Switch && typeSwitch == switchType.Railroad)
            {
                switchScript.moveCart();
            }

            StartCoroutine(ActivateOff());


        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isLocked)
        {
            isLocked = false;
            GameManager.mInstance.mIsLocked = isLocked;

        }
    }

    IEnumerator ActivateOff()
    {
        activateable = false;
        yield return new WaitForSeconds(1);
        activateable = true;
    }

    private IEnumerator OpenDoor()
    {
        currentRot = 0;

        while (currentRot < 90f)
        {
           
            float rotationThisFrame = openSpeed * Time.deltaTime;
            currentRot += rotationThisFrame;

           
            transform.Rotate(Vector3.up * rotationThisFrame);

          
            yield return null;
        }

    }    
}
