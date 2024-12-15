using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGeneral : MonoBehaviour
{
    public static SwitchGeneral switchGeneralInst;
    [SerializeField] GameObject objectToActivate;

    [SerializeField] public AudioSource audioSource;
    [SerializeField] AudioClip audioSwitchClip;

    [Header("----If Rotating-----")]
    
    [SerializeField] float targetRotation;
    [SerializeField] float rotationAmount;
    [SerializeField] float rotationSpeed;
    [SerializeField] float moveXAxis;
    [SerializeField] SwitchGeneral connectedSwitch;


    [Header("-----If Cart-----")]
    [SerializeField] AudioClip completionSound;
    [SerializeField] List<Transform> destinations = new List<Transform>();
    [SerializeField] List<bool> connected = new List<bool>();
    [SerializeField] List<int> connectedIndex = new List<int>();
    [SerializeField] float moveSpeed;
    [SerializeField] float moveTime;
    

    bool isRotating;
    bool isMoving;
    public void rotateObject()
    {
        if (!isRotating)
        {
            audioSource.PlayOneShot(audioSwitchClip);
            StartCoroutine(Rotator());
        }
    }

    public void moveCart()
    {
        if(!isMoving)
        {
            audioSource.PlayOneShot(audioSwitchClip);
            StartCoroutine(cartMover());
        }
    }

    
    IEnumerator Rotator()
    {
        isRotating = true;
        float currentRotation = 0f;

        
        while (currentRotation < targetRotation)
        {
            
            float rotationStep = rotationSpeed * Time.deltaTime;

            
            if (currentRotation + rotationStep > targetRotation)
            {
                rotationStep = targetRotation - currentRotation; 
            }

          
            objectToActivate.transform.Rotate(Vector3.up, rotationStep);

            
            currentRotation += rotationStep;

          
            yield return null;
        }

        if(connectedSwitch != null)
        {
            for (int i = 0; i < connectedIndex.Count; i++)
            {
                if (connectedSwitch.connected[connectedIndex[i]] == true)
                {
                    connectedSwitch.connected[connectedIndex[i]] = false;
                }
                else if (connectedSwitch.connected[connectedIndex[i]] == false)
                {
                    connectedSwitch.connected[connectedIndex[i]] = true;
                }

            }
        }
        isRotating = false;
    }

    IEnumerator cartMover()
    {

        Vector3 startPosition = objectToActivate.transform.position;  // Store the initial position
        float elapsedTime = 0f;

        isMoving = true;

        for(int i = 0; i < destinations.Count; i++)
        {
            if (objectToActivate.transform.position == destinations[i].transform.position)
            {

                if (i++ < destinations.Count && connected[i])
                {
                    if(i == destinations.Count - 1)
                    {
                        audioSource.PlayOneShot(completionSound);
                    }
                    if (i >= destinations.Count)
                    {
                        break;
                    }
                    // Continue moving until the target is reached
                    while (elapsedTime < moveTime)
                    {
                        // Interpolate the position using Lerp, which calculates a smooth position over time
                        objectToActivate.transform.position = Vector3.Lerp(startPosition, destinations[i].position, elapsedTime / moveTime);

                        elapsedTime += Time.deltaTime;  // Increase the elapsed time
                        yield return null;  // Wait until the next frame
                    }
                    objectToActivate.transform.position = destinations[i].transform.position;
                }
            }
        }

        

        yield return null;
        isMoving = false;
    }




}
