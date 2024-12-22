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

    private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {

        if (!GameManager.mInstance.mPaused)
        {
            FloatOnValue();
            //FloatOnRayCast();
        }
    }

    void FloatOnValue()
    {
        timer += Time.deltaTime * floatSpeed;

        float yOffset = Mathf.Sin(timer) * floatHeight;

        transform.position = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);

        transform.Rotate(Vector3.forward * rotationSpeedForward * Time.deltaTime);
        transform.Rotate(Vector3.up * rotationSpeedUp * Time.deltaTime);
    }

    void FloatOnRayCast()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        timer += Time.deltaTime * floatSpeed;

        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject != gameObject)
        {
                float distance = hit.distance;
                float desiredHeight = floatHeight - distance;
                
            if (desiredHeight <= 0f)
            {
                
            }
                float yOffset = Mathf.Sin(timer) * desiredHeight;

            //transform.position = startingPosition + new Vector3(0, floatingY, 0);
            transform.position = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);


        }

        transform.Rotate(Vector3.forward * rotationSpeedForward * Time.deltaTime);
        transform.Rotate(Vector3.up * rotationSpeedUp * Time.deltaTime);
    }
}
