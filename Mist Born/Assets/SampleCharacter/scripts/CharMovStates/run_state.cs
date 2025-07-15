using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class run_state : FSM_BaseState
{
    private FSM_CharMov my_sm;

    public run_state(FSM_CharMov myStateMachine) : base("run_state", myStateMachine) {
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
       
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        handleStateInputs();

        my_sm.animator.Play("run");

        if (!my_sm.grounded)
        {
            my_sm.ChangeState(my_sm.jump);
        }        
        
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        Vector2 velDir = my_sm.rigidBody.linearVelocity;
        velDir.x = my_sm.speed*horizontalInput;
        my_sm.rigidBody.linearVelocity = velDir;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public void handleStateInputs()
    {
        //   ### --- ###
        horizontalInput = my_sm.inputAction_move.ReadValue<Vector2>().x;
        if (Mathf.Abs(horizontalInput) < Mathf.Epsilon)
        {
            stateMachine.ChangeState(my_sm.idle);
        }

        //   ### --- ###
        jumpInput = my_sm.inputAction_jump.triggered;
        if (jumpInput == true && my_sm.isGrounded())
        {
            stateMachine.ChangeState(my_sm.jump);
        }

        //   ### --- ###
        dashInput = dashInput = my_sm.inputAction_dash.triggered;
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
        if (lightAttackInput && my_sm.energySlider.currValue_>my_sm.attack.lightAttackEnergy)
        {
            stateMachine.ChangeState(my_sm.attack);
        }

        //   ### --- ###
        heavyAttackInput = my_sm.inputAction_heavy_attack.triggered;
        if (heavyAttackInput && my_sm.energySlider.currValue_ > my_sm.attack.heavyAttackEnergy)
        {
            stateMachine.ChangeState(my_sm.attack);
            my_sm.attack.typeHeavy = true;
        }
    }
}
