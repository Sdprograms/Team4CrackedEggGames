using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToggleComponent : MonoBehaviour
{
    enum Activator {Player, Cart};
    [SerializeField] Activator m_Activator;
    [SerializeField] MonoBehaviour objectToToggleOn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && m_Activator == Activator.Player)
        {
            if (objectToToggleOn != null)
            {
                objectToToggleOn.enabled = true;
            }

        }
        else if(other.CompareTag("Cart") && m_Activator == Activator.Cart)
        {
            if (objectToToggleOn != null)
            {
                objectToToggleOn.enabled = true;
            }
        }

    }


}
