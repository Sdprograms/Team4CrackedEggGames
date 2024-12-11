using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] float floatSpeed = 1f; // Adjusts the floating speed

    [SerializeField] float floatHeight = 1f; // Controls the maximum height of the float

    [SerializeField] float rotationSpeedForward = 50f;
    [SerializeField] float rotationSpeedUp = 0f;
    private float timer = 0f;




    void Update()

    {
        if (!GameManager.mInstance.mPaused)
        {
            timer += Time.deltaTime * floatSpeed;

            float yOffset = Mathf.Sin(timer) * floatHeight;

            transform.position = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);

            transform.Rotate(Vector3.forward * rotationSpeedForward * Time.deltaTime); 
            transform.Rotate(Vector3.up * rotationSpeedUp * Time.deltaTime);
        }
    }
}
