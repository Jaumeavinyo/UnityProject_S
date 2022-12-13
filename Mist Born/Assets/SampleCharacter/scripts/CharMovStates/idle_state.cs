using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idle_state : FSM_BaseState
{
    private FSM_CharMov my_sm;

    public idle_state(FSM_CharMov myStateMachine) : base("idle_state", myStateMachine) {

        my_sm = (FSM_CharMov)stateMachine;
    }
   

    public override void Enter()
    {
        base.Enter();

        horizontalInput =   0;
        jumpInput =         false;
        dashInput =         false;
        rollInput =         false;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        my_sm.animator.Play("idle");

        handleStateInputs();

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        my_sm.rigidBody.velocity = my_sm.rigidBody.velocity * 0;//avoid moving in idle by error
    }
    public override void Exit()
    {
        base.Exit();
    }

    public void handleStateInputs()
    {
        //   ### --- ###
        horizontalInput = my_sm.inputAction_move.ReadValue<Vector2>().x;
        if (Mathf.Abs(horizontalInput) > Mathf.Abs(0.1f))
        {
            stateMachine.ChangeState(my_sm.run);
        }

        //   ### --- ###
        jumpInput = my_sm.inputAction_jump.triggered;
        if (jumpInput == true && my_sm.isGrounded())
        {
            stateMachine.ChangeState(my_sm.jump);
        }

        //   ### --- ###
        dashInput = my_sm.inputAction_dash.triggered;
        if (dashInput)
        {
            stateMachine.ChangeState(my_sm.dash);
        }

        //   ### --- ###
        rollInput = my_sm.inputAction_roll.triggered;
        if (rollInput)
        {
            stateMachine.ChangeState(my_sm.roll);
        }

        //   ### --- ###
        lightAttackInput = my_sm.inputAction_light_attack.triggered;
        if (lightAttackInput)
        {
            stateMachine.ChangeState(my_sm.attack);
        }

        //   ### --- ###
        heavyAttackInput = my_sm.inputAction_heavy_attack.triggered;
        if (heavyAttackInput)
        {
            stateMachine.ChangeState(my_sm.attack);
            my_sm.attack.typeHeavy = true;
        }
    }


}
