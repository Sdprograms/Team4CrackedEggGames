using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] CharacterController characterController;

    [SerializeField] int HP;
    [SerializeField] int MaxHP;

    [SerializeField] int speed;
    [SerializeField] int sprintMult;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpCount;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;

    Vector3 moveDirection;
    Vector3 playerVel;

    bool isSprinting;

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
        if ((characterController.isGrounded))
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
}
