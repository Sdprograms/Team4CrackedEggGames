using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{

    [SerializeField] int lookSensitivity;
    [SerializeField] int lockVerticalmin, lockVerticalmax;
    [SerializeField] bool invertY = false;

    float rotX;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false; //Cursor settings on start
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;

        if (invertY) //If Mouse invert is selected
        {
            rotX += mouseY;
        }
        else
        {
            rotX -= mouseY;
        }

        rotX = Mathf.Clamp(rotX, lockVerticalmin, lockVerticalmax); //Camera clamp

        transform.localRotation = Quaternion.Euler(rotX, 0, 0); //Rotate on x-axis

        transform.parent.Rotate(Vector3.up * mouseX); //Rotate on Y-axis
    }
}
