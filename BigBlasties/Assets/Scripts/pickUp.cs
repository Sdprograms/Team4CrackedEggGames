using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class pickUp : MonoBehaviour
{
    enum pickupType { gun, HP, Ammo, Key, Note };
    enum keyType { greyKey, redKey, blueKey, greenKey, bossKey };
    [SerializeField] pickupType type;
    [SerializeField] keyType keyColor;
    [SerializeField] Renderer model;
    [SerializeField] gunStats gun;//guns
    [SerializeField] int healthRestored;

    [Header("Note Info")]
    [SerializeField] int noteNum;
    [SerializeField] TMP_Text noteTitle;
    [SerializeField] TMP_Text noteBody;

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
            else if (type == pickupType.Note)
            {
                //if pausing while freezing, then movement can resume without turning off the note
                GameManager.mInstance.FreezeGame();

                if (noteNum == 0)
                {
                    noteTitle.text = "CONGRATULATIONS!!!";
                    noteBody.text = "You just won the immense pleasure of reading a note.\n\n" +
                        "These will be scattered about each of the levels and will provide you with some information, each of their usefulness will be up to you..\n\n" +
                        "Press 'E' in order to close this window and keep playing.";
                }
                else if (noteNum == 1)
                {
                    noteTitle.text = "A Crash and a Tumble";
                    noteBody.text = "After falling into a sinkhole and landing right in the front entrance of the enemy mining operation, you lost all of your weapons but your trusty auto laser pistol." +
                        "Taking the time and resources to patch yourself up, you know that getting out of this place is your only option, and lucky for you, there's an evac ship outside.\n\n" +
                        "If the intel you were briefed on earlier serves correct, the way forward involves activating an elevating platform controled by some energy cargo.";
                }
                else if (noteNum == 2)
                {
                    noteTitle.text = "Trouble in Operations";
                    noteBody.text = "Well that was a major jitter in the smoothly running plans we had. Not too long ago the ceiling collapsed, and that is'nt too bad for the mining area, but because we have a spare room in the back " +
                        "and an office in the center, I can't get into either of them. I suppose it doesn't really matter too much though as I lost my key durring the ceiling collapse.\n\n" +
                        "It honestly ruins my day, there was a gun in the office that I was tasked with testing...";
                }
                else if (noteNum == 3)
                {
                    noteTitle.text = "Power on the Engine";
                    noteBody.text = "Alright new guy,\n\nI've explained how this machine works multiple times now, so instead of hearing you ask again, I'm writing it down.\n\nThe mag-lev is easy to use, and rather fool-proof. The arrows move " +
                        "each key to a preset position, you can rotate them too, but I wouldn't reccomend that unless you know what you're doing. Then there's switching between the blasted things. Get all three of them in their docks and then power is restored." +
                        " \n\nIf you need anything else, you know where to find me, but please for the love of... just figure out this thing soon.";
                }
                else if(noteNum == 4)
                {
                    noteTitle.text = "New Mech Design in Report";
                    noteBody.text = "Progress: Alpha\n\nComplete: So far, the mech we've been designing for the past five years has made considerable leaps and bounds, (not physically). It's slew of ranged attacks are up to standards, and the plans are fully underway.\n\n" +
                        "Deficits: The mech has had issues with power in the past, and those issues were mitigated, yet it's not yet ready for the battlefield as it tends to destroy the extra batteries. Further texting is necessary...";
                }
                else if (noteNum == 5)
                {
                    noteTitle.text = "The Unmanned Mech";
                    noteBody.text = "Despite the project not being fully complete, it has taken the reins into its own metallic hands. Attaining a rogue AI model has done disasterous deeds to the facility, yet it wasn't the cause of the mess." +
                        "\n\nYour training shouldn't fail you, take it down like the rest of them.";
                }
                else if (noteNum == 6)
                {
                    noteTitle.text = "Guarded Escape";
                    noteBody.text = "Freedom is on the horizon, but on the forefront is an army of guards blocking your path. Evac is holding their position, but can't get any further in. " +
                        "\n\nIf you want to get out alive, you'll have to earn it.";
                }

                Destroy(gameObject);

                GameManager.mInstance.notePage.SetActive(true);  
                GameManager.mInstance.mNoteActive = true;
            }
        }
    }
}
