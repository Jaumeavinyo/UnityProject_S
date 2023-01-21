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

    CharacterController characterController;
    Rigidbody rb;
    Animator animator;

    States currentState;
    
    public InputAction inputAction_move;
    public Vector2 moveDir;
    public Vector2 lookDir;
    public float moveSpeed;

    public PlayerInput playerInputs;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        currentState = States.idle;

        moveSpeed = 0.1f;
    }

    void Update()
    {
        moveDir = inputAction_move.ReadValue<Vector2>();
        changeStates();
        changeAnimVars();
        
    }

    private void FixedUpdate()
    {
        
        characterController.Move(new Vector3(moveDir.x * moveSpeed, 0, moveDir.y * moveSpeed));

        if(moveDir.magnitude != 0)
        {
            lookDir = new Vector3(moveDir.x, 0, moveDir.y);
        }

        characterController.transform.rotation = Quaternion.LookRotation(lookDir);
        //this.transform.rotation = Quaternion.LookRotation(lookDir);
        Debug.Log("x = " + lookDir.x + "y = " + lookDir.y);
    }

    private void OnEnable()
    {
        inputAction_move.Enable();
    }
    private void OnDisable()
    {
        inputAction_move.Disable();
    }


    void changeStates()
    {
        if (moveDir.magnitude == 0)
        {
            currentState = States.idle;
        }
        else if (moveDir.magnitude != 0)
        {
            currentState = States.run;
        }
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
