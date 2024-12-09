using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour, damageInterface
{
    [SerializeField] LayerMask maskToIgnore;
    [SerializeField] public CharacterController characterController;

    [SerializeField] int HP;
    [SerializeField] int MaxHP;

    [SerializeField] int numKeys;
    [SerializeField] bool redKey;
    [SerializeField] bool greenKey;
    [SerializeField] bool blueKey;
    [SerializeField] bool bossKey;

    //[SerializeField] GameObject ammoTypeLaser; --SD commented out for new weapon system
    //[SerializeField] float laserShootRate;
    //[SerializeField] GameObject ammoTypeExplosive;
    //[SerializeField] float explosiveShootRate;

    [SerializeField] Transform shootPos;

    //GUNS 
    [SerializeField] List<gunStats> gunInventory = new List<gunStats>();
    [SerializeField] GameObject gunModel;
    [SerializeField] AudioClip reloadAudioClip;
    [SerializeField] GameObject projectile;
    [SerializeField] AudioClip projectileAudio;
    [SerializeField] AudioClip mStepSound; // -XB
    [SerializeField] AudioClip healthRestoreClip;
    [SerializeField] AudioClip keyGatheredClip;
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] float shootDistance;
    [SerializeField] int ammoCurrent;
    [SerializeField] int ammoMax;
    [SerializeField] int ammoReserve;
    int selectedGun;

    //OTHER STATS
    [SerializeField] int speed;
    [SerializeField] int dodgeSpeed;
    [SerializeField] float dodgeTime;
    [SerializeField] int sprintMult;
    [SerializeField] int jumpMax;
    [SerializeField] public int jumpCount; // public by XB
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;

    public static playerController mPlayerInstance; // -XB

    public AudioSource weaponAudioSource;
    public AudioSource reloadAudioSource;
    public AudioSource mStepsSource; // -XB

    public Vector3 moveDirection; // public by XB
    public Vector3 playerVel; // public by XB

    //bool isSprinting;
    public bool isShooting; //public by -XB
    //bool mGunSwapped; // used along with the dampener for weapon swap -XB

    int speedOriginal;

   // [SerializeField] string weaponType;

    private void Awake()
    {
        mStepsSource.clip = mStepSound;
        Debug.Log("Clip set");
    }

    // Start is called before the first frame update
    void Start()
    {
        //weaponType = "Laser";
        speedOriginal = speed;
        HP = MaxHP;

        weaponAudioSource = GetComponent<AudioSource>();
        reloadAudioSource = GetComponent<AudioSource>();
        reloadAudioSource.clip = reloadAudioClip;

        mPlayerInstance = this; // -XB
        Respawn();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
        selectGun();
        reload();
    }

    void movement()
    {
        if ((characterController.isGrounded)) //If character is on ground
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        moveDirection = transform.forward * Input.GetAxis("Vertical") //Establish move Direction
            + transform.right * Input.GetAxis("Horizontal");

        characterController.Move(moveDirection * speed * Time.deltaTime);

        if (jumpCount == 0 && moveDirection != Vector3.zero && GameManager.mInstance.mPaused == false)
        {
            if (!mStepsSource.isPlaying)
            {
                //Debug.Log("Plays Steps");
                mStepsSource.Play();
            }
        }
        else
        {
            if (mStepsSource.isPlaying) 
            {
                //Debug.Log("Stops Steps");
                mStepsSource.Stop();
            }
        }

        jump();

        characterController.Move(playerVel * Time.deltaTime);

        playerVel.y -= gravity * Time.deltaTime;

        if(Input.GetButton("Fire1") && !isShooting && GunRotation.mGunRotInst.mCanFire) //if leftclick
        {
            StartCoroutine(shoot()); //shoot
        }
        else if (Input.GetAxis("Fire1") > 0.5f && !isShooting && GunRotation.mGunRotInst.mCanFire) //Controller Supported Fire -XB
        {
            StartCoroutine(shoot()); //shoot
        }

    }

    void jump()
    {
        if (Input.GetButton("Jump") && Input.GetButton("Horizontal"))
        { //Dodge / Boost function.
            StartCoroutine(dodge());
        }
        else if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMult;
            //isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMult;
            //isSprinting = false;
        }
    }


    public IEnumerator shoot() // public by -XB
    {

        //Ensures that the game is unpaused to shoot, I would like to recommend, have this check in future in-game (unpaused) actions -XB
        if (GameManager.mInstance.mPaused == false && gunInventory.Count > 0 && gunInventory[selectedGun].ammoCurrent > 0)
        {
            isShooting = true;

            gunInventory[selectedGun].ammoCurrent--;

            Instantiate(projectile, shootPos.position, shootPos.transform.rotation);//projectile

            weaponAudioSource.clip = projectileAudio;
            weaponAudioSource.Play(); //sound


            /*RaycastHit whatsHit; //RAY CAST

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out whatsHit, shootDistance, ~maskToIgnore)) //
            {
                damageInterface dmg = whatsHit.collider.GetComponent<damageInterface>();
                Debug.Log(whatsHit.collider.name);
                if(dmg != null)
                {
                    dmg.takeDamage(shootDamage);
                }
            }*/

            /*if (weaponType == "Laser")
            {
                //Laser

                Instantiate(ammoTypeLaser, shootPos.position, shootPos.transform.rotation);
                shootRate = laserShootRate;
                SoundEffects.noiseMaker.LaserSound(); // -XB
            }
            else if (weaponType == "Grenade")
            {
                //Explosive

                Instantiate(ammoTypeExplosive, shootPos.position, shootPos.transform.rotation);
                shootRate = explosiveShootRate;
                SoundEffects.noiseMaker.GrenadeShotSound(); // -XB
            }*/

            yield return new WaitForSeconds(shootRate);
            Debug.Log("Waited to fire");
            isShooting = false;
            Debug.Log("IsShooting is false");
        }
    }

    /*public void weaponSwap() 
    {
        if (Input.GetButtonDown("Weapon Switch"))
        {
            if (weaponType == "Laser")
            {
                weaponType = "Grenade";
                SoundEffects.noiseMaker.GrenadeLaunceherSwap(); // -XB
            }
            else if (weaponType == "Grenade")
            {
                weaponType = "Laser";
                SoundEffects.noiseMaker.LaserSwap(); // -XB
            }
        }
    }*/
    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(FlashDamage()); // -XB
        UpdateUI(); // -XB

        if (HP <= 0) //Lose condition
        {
            GameManager.mInstance.GameOver();
        }
    }

    IEnumerator dodge()
    {
        speed += dodgeSpeed;

        yield return new WaitForSeconds(dodgeTime);

        speed = speedOriginal;
    }

    public void UpdateUI() // -XB
    {
        GameManager.mInstance.mPlayerHealth.fillAmount = (float)HP / MaxHP;
        if(gunInventory.Count > 0)
        {
            GameManager.mInstance.mAmmoCurrent.text = ammoCurrent.ToString();
            GameManager.mInstance.mAmmoReserve.text = ammoReserve.ToString();
        }
        if(mStepsSource.clip == null)
        {
            mStepsSource.clip = mStepSound;
            Debug.Log("StepSound was null...");
        }
        GameManager.mInstance.mKeyAmount.text = numKeys.ToString();
    }

    IEnumerator FlashDamage() // -XB
    {
        GameManager.mInstance.mPlayerDamageTaken.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        GameManager.mInstance.mPlayerDamageTaken.SetActive(false);
    }

    public void getGunStats(gunStats gun)
    {
        gunInventory.Add(gun);
        selectedGun = gunInventory.Count - 1;
        projectile = gunInventory[selectedGun].projectile;
        projectileAudio = gunInventory[selectedGun].gunSound;
        shootRate = gunInventory[selectedGun].shootRate;
        ammoCurrent = gunInventory[selectedGun].ammoCurrent;
        ammoMax = gunInventory[selectedGun].ammoMax;
        ammoReserve = gunInventory[selectedGun].ammoReserve;

        //this gun model is getting the gun model from 'gun'
        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.gunModel.GetComponent<MeshFilter>().sharedMesh;
        //same thing but for material.
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.gunModel.GetComponent<MeshRenderer>().sharedMaterial;

        StartCoroutine(GunPosLogic.gunPosInst.OrientGunMesh(gunInventory[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh));// -XB

        reloadAudioSource.PlayOneShot(reloadAudioClip);
    }

    void changeGun()
    {
        shootRate = gunInventory[selectedGun].shootRate;
        projectile = gunInventory[selectedGun].projectile;
        projectileAudio = gunInventory[selectedGun].gunSound;
        ammoCurrent = gunInventory[(selectedGun)].ammoCurrent;
        ammoReserve = gunInventory[selectedGun].ammoReserve;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunInventory[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh; //gives mesh
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunInventory[selectedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial; //gives shader
        StartCoroutine(GunPosLogic.gunPosInst.OrientGunMesh(gunInventory[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh));// -XB
        UpdateUI();
    }

    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunInventory.Count - 1)
        {
            selectedGun++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            changeGun();
        }
    }


    //this is primarily to fix a controller issue, it affects mouse and keyboard the same way as controller, making them indistinguishable from each other
    //void selectGun()
    //{
    //    if (!mGunSwapped)
    //    {
    //        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 && selectedGun < gunInventory.Count - 1)
    //        {
    //            selectedGun++;
    //            changeGun();
    //            mGunSwapped = true;
    //        }
    //        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 && selectedGun > 0)
    //        {
    //            selectedGun--;
    //            changeGun();
    //            mGunSwapped = true;

    //        }
    //    }
    //    else  if (mGunSwapped) { StartCoroutine(damperSelectGun()); }

    //}

    //IEnumerator damperSelectGun()
    //{
    //    if (mGunSwapped)
    //    {
    //        yield return new WaitForSeconds(0.1f);
    //        mGunSwapped = false;
    //    }
    //}

    void reload()
    {
        int ammoDifference;
        if (Input.GetButtonDown("Reload") && gunInventory.Count > 0)
        {
            reloadAudioSource.PlayOneShot(reloadAudioClip);
            if (ammoReserve >= ammoMax)
            {
                ammoDifference = ammoMax - ammoCurrent;
                gunInventory[selectedGun].ammoCurrent = ammoMax;
                gunInventory[selectedGun].ammoReserve -= ammoDifference;

            }
            else if (ammoReserve <= ammoMax)
            {
                gunInventory[selectedGun].ammoCurrent += ammoReserve;
                gunInventory[selectedGun].ammoReserve = 0;
            }
        }
        if (gunInventory.Count != 0) // Checks if the gun inventory has something in it before assigning values, otherwise theyll be out of range -XB
        {
            ammoCurrent = gunInventory[selectedGun].ammoCurrent;
            ammoReserve = gunInventory[selectedGun].ammoReserve;
            ammoMax = gunInventory[selectedGun].ammoMax;
        }
        
        UpdateUI();
    }

    public void ammoPickup()
    {
        if(gunInventory.Count > 0)
        {
            for (int i = 0; i < gunInventory.Count; i++)
            {
                gunInventory[i].ammoReserve += gunInventory[i].ammoMax;
            }
        }

        reloadAudioSource.PlayOneShot(reloadAudioClip);
    }

    public void Respawn()
    {
        characterController.enabled = false;//disables input from the player

        transform.position = GameManager.mInstance.mPlayerSpawnPos.transform.position;//sets the player to the spawn positions location,
                                                                                      //this could also be used to move between levels with tweaks and a different spawner per level
        characterController.enabled = true;

        HP = MaxHP;
        UpdateUI();
    }

    public void restoreHealth(int healthRestored)
    {
        if (HP < MaxHP)
        {
            reloadAudioSource.PlayOneShot(healthRestoreClip);
            HP += healthRestored;

            if (HP > MaxHP)
            {
                HP = MaxHP;
            }

            UpdateUI();
        }
    }

    public int GetKeys()
    {
        return numKeys;
    }
    public void SetKeys(int keyAmount)
    {
        numKeys = keyAmount;
        UpdateUI();
    }
    public void AddKey()
    {
        numKeys++;
        reloadAudioSource.PlayOneShot(keyGatheredClip);
        UpdateUI();
    }
    public void RemoveKey() 
    { 
        numKeys--;
        UpdateUI();
    }

    public bool getRedKey()
    {
        return redKey;
    }
    public void setRedKey(bool have)
    {
        redKey = have;
        reloadAudioSource.PlayOneShot(keyGatheredClip);
    }
    public bool getBlueKey()
    {
        return blueKey;
    }
    public void setBlueKey(bool have)
    {
        blueKey = have;
        reloadAudioSource.PlayOneShot(keyGatheredClip);
    }
    public bool getGreenKey()
    {
        return greenKey;
    }
    public void setGreenKey(bool have)
    {
        greenKey = have;
        reloadAudioSource.PlayOneShot(keyGatheredClip);
    }
    public bool getBossKey()
    {
        return bossKey;
    }
    public void setBossKey(bool have)
    {
        bossKey = have;
        reloadAudioSource.PlayOneShot(keyGatheredClip);
    }
}

