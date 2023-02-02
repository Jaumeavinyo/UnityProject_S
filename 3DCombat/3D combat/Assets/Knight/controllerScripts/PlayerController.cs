using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

enum States
{
    idle,
    run
}

public class PlayerController : MonoBehaviour
{

    InputHandler inputHandler;
    AnimatorHandler animatorHandler;

    CharacterController characterController;
    Rigidbody           rb;
    Animator            animator;
    public Camera       camera;



    States currentState;


    Vector3 moveDirection;
    Vector3 rotateDirection;


    private float walkSpeed = 3;
    private float runSpeed = 6;
    public float movementSpeed = 0;
    public float rotationSpeed = 0.1f;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        rb                  = GetComponent<Rigidbody>();
        animator            = GetComponent<Animator>();
        //camera              = GetComponent<Camera>();
        inputHandler        = GetComponent<InputHandler>();
        animatorHandler     = GetComponent<AnimatorHandler>();
    }
    void Start()
    {
        currentState = States.idle;
    }

    void Update()
    {
        if(inputHandler.moveAmount > 0.55f)
        {
            movementSpeed = runSpeed;
        }else if(inputHandler.moveAmount < 0.55f)
        {
            movementSpeed = walkSpeed;
        }
        
        changeStates();
  
        
    }

    private void FixedUpdate()
    {
        moveRB();
        rotateRB();

    }

    public void moveRB()
    {
        moveDirection = camera.transform.forward * inputHandler.verticalInput;
        moveDirection += camera.transform.right * inputHandler.horizontalInput;
        moveDirection.Normalize();

        moveDirection.y = 0;

        Vector3 moveVelocity = moveDirection * movementSpeed;
        rb.velocity = moveVelocity;
    }

    public void rotateRB()
    {
        rotateDirection = camera.transform.forward * inputHandler.verticalInput;
        rotateDirection += camera.transform.right * inputHandler.horizontalInput;
        rotateDirection.Normalize();
        rotateDirection.y = 0;

        if(rotateDirection == Vector3.zero)
        {
            rotateDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(rotateDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed*Time.deltaTime);
        transform.rotation = playerRotation;
    }

    private void OnGUI()
    {
        //string lookDirStr = "x = "+lookDir.x+" y = "+ lookDir.y;
        //GUILayout.Label(lookDirStr);
    }

  


    void changeStates()
    {
        
    }

    
}
