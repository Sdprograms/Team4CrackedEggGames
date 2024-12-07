using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] LayerMask IgnoreMask; //-XB Allows the 
    //[SerializeField] GameObject positionAim; //-XB DEBUG
    [SerializeField] GameObject shootPos; //-XB
    public static cameraController camInstance; //-XB
    [SerializeField] int mShootDistance; //-XB
                                         //Provides a max range for the raycast to target to, after aiming past this distance,
                                         //all shots will be a little to the right, as tracking will be null

    [SerializeField] public int lookSensitivity;
    [SerializeField] int lockVerticalmin, lockVerticalmax;
    [SerializeField] public bool invertY = false;

    float rotX;

    Quaternion mShootRotOrig;

    //simple getters/setters -XB
    public int GetSensitivity() { return lookSensitivity; }
    public void SetSensitivity(int newSens) { lookSensitivity = newSens; }

    // Start is called before the first frame update
    void Start()
    {
        camInstance = this; //-XB
        Cursor.visible = false; //Cursor settings on start
        Cursor.lockState = CursorLockMode.Locked;

        mShootRotOrig = shootPos.transform.localRotation;
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

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * mShootDistance, Color.green);
        Debug.DrawRay(shootPos.transform.position, shootPos.transform.forward * mShootDistance, Color.red);
        //^ was causing an error with gun pickup.
        FireOnCursor();
    }
 
    public void FireOnCursor() //Makes the weapons fire on the cursors position, giving a delay in aim on side-to-side movement.
    {
        Vector3 rayStart = Camera.main.transform.position + Camera.main.transform.forward * 2.25f; // Offset the raycast so it starts a infront of player
        //Debug.DrawRay(rayStart, Camera.main.transform.up, Color.blue);

        RaycastHit hit;
        //if the raycast hits anything, then itll rotate the shootPos to the hit position
        if (Physics.Raycast(rayStart, Camera.main.transform.forward, out hit, mShootDistance, ~IgnoreMask))
        {
                //positionAim.transform.position = hit.point;
                Vector3 aim = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            shootPos.transform.LookAt(hit.point); //in debugging, change hit.point to positionAim.transform
        }
        //otherwise itll be deadpan forward
        else
        {
            shootPos.transform.localRotation = mShootRotOrig;
            //Debug.Log("Look at nothing");
        }
    }
}
