using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUp : MonoBehaviour
{
   enum pickupType {gun, HP, Ammo };
    [SerializeField] pickupType type;
    [SerializeField] gunStats gun;//guns

    private void Start()
    {
        if(type == pickupType.gun) //if gun
        {
            gun.ammoCurrent = gun.ammoMax;
        }
        else if (type == pickupType.Ammo) //if gun
        {
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (type == pickupType.gun)
            {
                GameManager.mInstance.mPlayerController.getGunStats(gun);
                Destroy(gameObject);
            }
            else if (type == pickupType.Ammo)
            {

                GameManager.mInstance.mPlayerController.ammoPickup();
                Destroy(gameObject);
            }
        }
    }
}
