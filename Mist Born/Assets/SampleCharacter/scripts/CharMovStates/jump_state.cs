﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump_state : FSM_BaseState
{
    private FSM_CharMov my_sm;

    private enum jumpStates {JUMP,JUMP_RISE,JUMP_MID,JUMP_FALL,JUMP_LAND};
    private jumpStates currState;
    private bool doubleJump;
    private bool doubleJumping;
    private bool jumpNow;
    private bool jumping;
    private bool risingStage;
    public bool jumpingMid;
    private bool fallingStage;
  
    public bool rollInputJump = false;//predefined as false, in exit set to false always
    public jump_state(FSM_CharMov myStateMachine) : base("jump_state", myStateMachine)
    {
        my_sm = (FSM_CharMov)stateMachine;
    }


    public override void Enter()
    {
        base.Enter();             

        horizontalInput = 0;
        jumpInput = false;
        dashInput = false;
        rollInput = false;
        lightAttackInput = false;
        heavyAttackInput = false;
      
        currState = jumpStates.JUMP;

        doubleJump = false;
        doubleJumping = false;

        jumpNow = false;
        jumping = false;

        risingStage = false;
        jumpingMid = false;
        fallingStage = false;
        
        handleOtherCases();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (currState == jumpStates.JUMP_MID && my_sm.rigidBody.linearVelocity.y < 0)
        {
            currState = jumpStates.JUMP_FALL;
        }

        horizontalInput = my_sm.inputAction_move.ReadValue<Vector2>().x;

        handleStateInputs();

        //handleOtherCases(); //jumps not always start or end the same way

        handleInternalJumpState();
         
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        Vector2 velDir = my_sm.rigidBody.linearVelocity;
        velDir.x = my_sm.speed * horizontalInput;
        my_sm.rigidBody.linearVelocity = velDir;

        if (jumpNow)
        {
            jump();
        }

    }

    public override void Exit()
    {
        base.Exit();
        jumpNow = false;
        jumping = false;
        rollInputJump = false;
    }

    public void jump()
    {
        Vector2 velDir = my_sm.rigidBody.linearVelocity;
        velDir.y = my_sm.jumpForce;
        my_sm.rigidBody.linearVelocity = velDir;

        jumpNow = false;
        jumping = true;
    }

    public void handleOtherCases()
    {

        //FALL FROM EDGE, NO INPUT FOR A JUMP
        if (my_sm.previousState.jumpInput == false && currState == jumpStates.JUMP)//didn't press jump but in the air ( falling from edge)
        {
            currState = jumpStates.JUMP_FALL;
        }


        //FALL AFTER DASH
        else if (my_sm.previousState == my_sm.dash && !fallingStage)
        {

            currState = jumpStates.JUMP_FALL;

        }
        else if (my_sm.previousState.jumpInput == false && my_sm.previousState == my_sm.roll && !fallingStage)//roll decide that character is not grounded
        {

            currState = jumpStates.JUMP_FALL;

        }else if (rollInputJump && my_sm.previousState == my_sm.roll)//jump pressed while roll
        {

            rollInputJump = false;
            my_sm.previousState.jumpInput = false;
            currState = jumpStates.JUMP;

        }

        if (currState == jumpStates.JUMP_MID && my_sm.grounded && my_sm.previousState.jumpInput == true)//go to land if jump to higher object
        {
            currState = jumpStates.JUMP_LAND;
        }
    }

    public void handleStateInputs()
    {
        //   ### --- ###
        dashInput = dashInput = my_sm.inputAction_dash.triggered;
        if (dashInput)
        {
            stateMachine.ChangeState(my_sm.dash);
        }

        //   ### --- ###
        if ((currState != jumpStates.JUMP_LAND && currState != jumpStates.JUMP) && my_sm.inputAction_jump.triggered)
        {
            if (!doubleJumping && my_sm.energySlider.currValue_>my_sm.doubleJumpEnergy)
            {
                currState = jumpStates.JUMP;
                doubleJump = true;
                my_sm.energySlider.modifyEnergyValue(-my_sm.doubleJumpEnergy);
            }     
        }

        //   ### --- ###
        lightAttackInput = my_sm.inputAction_light_attack.triggered;
        if (lightAttackInput)
        {
            stateMachine.ChangeState(my_sm.attack);
        }

        //   ### --- ###
        heavyAttackInput = my_sm.inputAction_heavy_attack.triggered; //when jumping, heavy attack is smash
        if (heavyAttackInput && my_sm.energySlider.currValue_>my_sm.attack.heavyAttackEnergy)
        {
            stateMachine.ChangeState(my_sm.attack);
            my_sm.attack.typeHeavy = true;
        }
    }

    public void handleInternalJumpState()
    {
        switch (currState)
        {
            case jumpStates.JUMP:
                {
                    //START ANIM
                    if (!jumping || (doubleJump && !doubleJumping))
                    {
                        if (doubleJump)
                        {
                            doubleJumping = true;
                            my_sm.audioSFX.playSound(my_sm.audioSFX.jump);
                        }
                        jumpNow = true;
                        jumping = true;
                        my_sm.animator.Play("jump_rise", 0);
                        
                    }
                    if (my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("jump_rise"))
                    {
                        if (my_sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)//anim ended
                        {
                            currState = jumpStates.JUMP_RISE;
                        }
                    }
                    break;
                }
            case jumpStates.JUMP_RISE:
                {
                    if (!risingStage)
                    {
                        risingStage = true;
                        my_sm.animator.Play("jump_rise", 0);
                    }

                    if (my_sm.rigidBody.linearVelocity.y < my_sm.jumpForce / 5)
                    {
                        currState = jumpStates.JUMP_MID;

                    }

                    break;
                }
            case jumpStates.JUMP_MID:
                {
                    if (!jumpingMid)
                    {
                        risingStage = false;
                        my_sm.animator.Play("jump_mid", 0);
                        jumpingMid = true;
                    }

                    if (Mathf.Abs(my_sm.rigidBody.linearVelocity.y) > my_sm.jumpForce / 2 && !my_sm.grounded)
                    {
                        currState = jumpStates.JUMP_FALL;
                    }
                    break;
                }
            case jumpStates.JUMP_FALL:
                {
                    if (!fallingStage)
                    {
                        fallingStage = true;
                    }

                    if (horizontalInput == 0)
                    {
                        fallingStage = true;
                        my_sm.animator.Play("jump_mid"); // falling vertically
                    }
                    else if (horizontalInput != 0)
                    {
                        fallingStage = true;
                        my_sm.animator.Play("jump_fall");// falling foreward
                    }

                    if (my_sm.grounded)
                    {
                        currState = jumpStates.JUMP_LAND;
                    }

                    break;
                }
            case jumpStates.JUMP_LAND:
                {

                    my_sm.animator.Play("jump_land");

                    if (my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("jump_land"))
                    {
                        if (my_sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                        {
                            horizontalInput = Input.GetAxis("Horizontal");
                            if (Mathf.Abs(horizontalInput) > Mathf.Epsilon)
                            {
                                stateMachine.ChangeState(my_sm.run);
                            }
                            else
                            {
                                stateMachine.ChangeState(my_sm.idle);
                            }
                        }
                    }

                    break;
                }

        }
    }
}
