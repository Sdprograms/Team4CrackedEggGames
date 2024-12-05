using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGeneral : MonoBehaviour
{
    [SerializeField] GameObject objectToActivate;
    [SerializeField] float rotationAmount;

    public void rotateObject()
    {
        objectToActivate.transform.Rotate(Vector3.up, rotationAmount, Space.World);
    }
}
