using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGeneral : MonoBehaviour
{
    public static SwitchGeneral switchGeneralInst;

    [SerializeField] GameObject objectToActivate;
    [SerializeField] float targetRotation;
    [SerializeField] float rotationAmount;
    [SerializeField] float rotationSpeed;
    [SerializeField] float moveXAxis;

    bool isRotating;
    public void rotateObject()
    {
        if (!isRotating)
        {
            StartCoroutine(Rotator());
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
        isRotating = false;
    }




}
