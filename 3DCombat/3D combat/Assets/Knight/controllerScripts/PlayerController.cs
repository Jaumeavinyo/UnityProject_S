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

    CharacterController characterController;
    Rigidbody           rb;
    Animator            animator;
    public Camera       camera;



    States currentState;


    Vector3 moveDirection;
    Vector3 rotateDirection;

    public float movementSpeed = 4;
    public float rotationSpeed = 0.3f;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        rb                  = GetComponent<Rigidbody>();
        animator            = GetComponent<Animator>();
        //camera              = GetComponent<Camera>();
        inputHandler        = GetComponent<InputHandler>();
    }
    void Start()
    {
        currentState = States.idle;
    }

    void Update()
    {
        
        changeStates();
        changeAnimVars();
        
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

    void changeAnimVars()
    {
        if(currentState == States.idle)
        {
            
        }else if(currentState != States.idle)
        {

        }

        if(currentState == States.run)
        {
            animator.SetBool("stateRun", true);
            
        }else if(currentState != States.run)
        {
            animator.SetBool("stateRun", false);
            
        }
    }
}
