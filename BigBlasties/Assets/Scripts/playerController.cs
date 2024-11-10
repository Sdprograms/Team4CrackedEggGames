using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, damageInterface
{
    [SerializeField] LayerMask maskToIgnore;
    [SerializeField] CharacterController characterController;

    [SerializeField] int HP;
    [SerializeField] int MaxHP;

    [SerializeField] GameObject ammoTypeLaser;
    [SerializeField] float laserShootRate;
    [SerializeField] GameObject ammoTypeExplosive;
    [SerializeField] float explosiveShootRate;

    [SerializeField] Transform shootPos;

    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] float shootDistance;

    [SerializeField] int speed;
    [SerializeField] int dodgeSpeed;
    [SerializeField] float dodgeTime;
    [SerializeField] int sprintMult;
    [SerializeField] int jumpMax;
    [SerializeField] public int jumpCount; // public by XB
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;

    public static playerController mPlayerInstance; // -XB

    public Vector3 moveDirection; // public by XB
    public Vector3 playerVel; // public by XB

    bool isSprinting;
    public bool isShooting; //public by -XB

    int speedOriginal;

    [SerializeField] string weaponType;

    // Start is called before the first frame update
    void Start()
    {
        weaponType = "Laser";
        speedOriginal = speed;
        HP = MaxHP;

        mPlayerInstance = this; // -XB
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
        weaponSwap();
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

        jump();

        characterController.Move(playerVel * Time.deltaTime);

        playerVel.y -= gravity * Time.deltaTime;

        if(Input.GetButton("Fire1") && !isShooting) //if leftclick
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
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMult;
            isSprinting = false;
        }
    }


    public IEnumerator shoot() // public by -XB
    {

        //Ensures that the game is unpaused to shoot, I would like to recommend, have this check in future in-game (unpaused) actions -XB
        if (GameManager.mInstance.mPaused == false)
        {
            isShooting = true;

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

            if (weaponType == "Laser")
            {
                //Laser

                Instantiate(ammoTypeLaser, shootPos.position, Camera.main.transform.rotation);
                shootRate = laserShootRate;
                SoundEffects.noiseMaker.LaserSound(); // -XB
            }
            else if (weaponType == "Grenade")
            {
                //Explosive

                Instantiate(ammoTypeExplosive, shootPos.position, Camera.main.transform.rotation);
                shootRate = explosiveShootRate;
                SoundEffects.noiseMaker.GrenadeShotSound(); // -XB
            }


            yield return new WaitForSeconds(shootRate);
            
            isShooting = false;
        }
    }

    public void weaponSwap()
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
    }
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
    }

    IEnumerator FlashDamage() // -XB
    {
        GameManager.mInstance.mPlayerDamageTaken.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        GameManager.mInstance.mPlayerDamageTaken.SetActive(false);
    }
}
