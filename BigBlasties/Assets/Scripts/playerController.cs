using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, damageInterface
{
    [SerializeField] LayerMask maskToIgnore;
    [SerializeField] CharacterController characterController;

    [SerializeField] int HP;
    [SerializeField] int MaxHP;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;

    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] float shootDistance;

    [SerializeField] int speed;
    [SerializeField] int sprintMult;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpCount;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;

    Vector3 moveDirection;
    Vector3 playerVel;

    bool isSprinting;
    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
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
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
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

    IEnumerator shoot()
    {
        isShooting = true;

        //BULLET
        Instantiate(bullet, shootPos.position, transform.rotation);

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
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0) //Lose condition
        {
            
        }
    }
}
