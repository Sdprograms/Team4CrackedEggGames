using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUp : MonoBehaviour
{
   enum pickupType {gun, HP, Ammo,Key };
    enum keyType {greyKey, redKey, blueKey, greenKey, bossKey };
    [SerializeField] pickupType type;
    [SerializeField] keyType keyColor;
    [SerializeField] Renderer model;
    [SerializeField] gunStats gun;//guns
    [SerializeField] int healthRestored;

    private void Start()
    {
        if(type == pickupType.gun) //if gun
        {
            gun.ammoCurrent = gun.ammoMax;
            gun.ammoReserve = gun.ammoMax;
        }
        else if (type == pickupType.Ammo) //if gun
        {
            
        }
        else if (type == pickupType.Key)
        {
            if (keyColor == keyType.greyKey)
            {
                model.material.color = Color.grey;
            }
            else if (keyColor == keyType.redKey)
            {
                model.material.color = Color.red;
            }
            else if (keyColor == keyType.blueKey)
            {
                model.material.color = Color.blue;
            }
            else if (keyColor == keyType.greenKey)
            {
                model.material.color = Color.green;
            }
            else if (keyColor == keyType.bossKey)
            {
                model.material.color = Color.yellow;
            }
        }
        //else if keys and respective colors to float and flash their respective colors. -SD
        //NEED UI ELEMENT FOR COLOR KEYS -SD
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
            else if (type == pickupType.HP)
            {
                GameManager.mInstance.mPlayerController.restoreHealth(healthRestored);
                Destroy(gameObject);
            }
            else if (type == pickupType.Key)
            {

                if (keyColor == keyType.greyKey)
                {
                    GameManager.mInstance.mPlayerController.AddKey();
                    Destroy(gameObject);
                }
                else if (keyColor == keyType.redKey)
                {
                    GameManager.mInstance.mPlayerController.setRedKey(true);
                    NotificationManager.mNotiManagrInst.ShowRedKey();
                    Destroy(gameObject);
                }
                else if (keyColor == keyType.blueKey)
                {
                    GameManager.mInstance.mPlayerController.setBlueKey(true);
                    NotificationManager.mNotiManagrInst.ShowBlueKey();
                    Destroy(gameObject);
                }
                else if (keyColor == keyType.greenKey)
                {
                    GameManager.mInstance.mPlayerController.setGreenKey(true);
                    NotificationManager.mNotiManagrInst.ShowGreenKey();
                    Destroy(gameObject);
                }
                else if (keyColor == keyType.bossKey)
                {
                    GameManager.mInstance.mPlayerController.setBossKey(true);
                    NotificationManager.mNotiManagrInst.ShowBossKey();
                    Destroy(gameObject);
                }
            }
        }
    }
}
