using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float floatSpeed = 1f; // Adjusts the floating speed

    public float floatHeight = 1f; // Controls the maximum height of the float



    private float timer = 0f;



    void Update()

    {
        if (!GameManager.mInstance.mPaused)
        {
            timer += Time.deltaTime * floatSpeed;

            float yOffset = Mathf.Sin(timer) * floatHeight;

            transform.position = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
        }
    }
}
