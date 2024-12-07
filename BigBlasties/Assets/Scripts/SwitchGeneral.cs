using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGeneral : MonoBehaviour
{
    public static SwitchGeneral switchGeneralInst;

    [SerializeField] GameObject objectToActivate;
    [SerializeField] float rotationAmount;
    [SerializeField] float moveXAxis;

   

    private void Start()
    {

    }
    public void rotateObject()
    {
        objectToActivate.transform.Rotate(Vector3.up, rotationAmount, Space.World);
    }



}
