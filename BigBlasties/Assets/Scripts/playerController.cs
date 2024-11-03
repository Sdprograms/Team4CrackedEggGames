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
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;

    Vector3 moveDirection;
    Vector3 playerVel;


    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        movement();
    }

    void movement()
    {
        moveDirection = transform.forward * Input.GetAxis("Vertical") //Establish move Direction
            + transform.right * Input.GetAxis("Horizontal");

        characterController.Move(moveDirection * speed * Time.deltaTime);

    }
}
