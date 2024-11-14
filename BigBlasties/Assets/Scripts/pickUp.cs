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
            gun.ammoReserve += gun.ammoMax;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.mInstance.mPlayerController.getGunStats(gun);
            Destroy(gameObject);
        }
    }
}
