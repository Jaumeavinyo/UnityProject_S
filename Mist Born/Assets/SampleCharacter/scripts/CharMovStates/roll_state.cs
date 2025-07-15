using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roll_state : FSM_BaseState
{
    private FSM_CharMov my_sm;

    public bool rolling;

    public int horizontalRoll;

    public roll_state(FSM_CharMov myStateMachine) : base("roll_state", myStateMachine)
    {

        my_sm = (FSM_CharMov)stateMachine;
    }


    public override void Enter()
    {
        base.Enter();
        rolling = false;
        jumpInput = false;
        //rolling can not change its direction. we decide direction at entrance of the state
        if (my_sm.lastDirectionInput > 0)
        {
            horizontalRoll = 1;
        }
        else if (my_sm.lastDirectionInput < 0)
        {
            horizontalRoll = -1;
        }

        
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!rolling)
        {
            rolling = true;
            roll(horizontalRoll);
            my_sm.animator.Play("roll");
            my_sm.energySlider.growFactor = 0;
        }

        if (rolling)// size y0.5 offset 0.25
        {
            my_sm.setInvulnerability(rolling);
        }

        if (my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("roll") && my_sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) // size: 0.4,1.1 || offset = x,0.61
        {
            rolling = false;
            my_sm.setInvulnerability(rolling);

            chooseStateAfterRoll();
            my_sm.energySlider.growFactor = my_sm.energySlider.originalGrowFactor;
        }

        handleStateInputs();

    }
    public override void Exit()
    {
        base.Exit();
    }

    public void roll(int dir)
    {
        Vector2 velDir = my_sm.rigidBody.linearVelocity;
        velDir.x = my_sm.rollSpeed * dir;
        my_sm.rigidBody.linearVelocity = velDir;
    }

    public void chooseStateAfterRoll()
    {
        if (horizontalInput != 0)
        {
            if (!my_sm.grounded)//fall after roll over edge
            {
                my_sm.ChangeState(my_sm.jump);
            }
            else//run after roll
            {
                my_sm.ChangeState(my_sm.run);
            }
        }
        else if (horizontalInput == 0)
        {
            if (!my_sm.grounded) //fall after roll over edge
            {
                my_sm.ChangeState(my_sm.jump);
            }
            else //idle after roll
            {
                my_sm.ChangeState(my_sm.idle);
            }
        }

        
    }

    public void handleStateInputs()//some inputs will be stored until anim roll ends?
    {
        //   ### --- ###
        horizontalInput = my_sm.inputAction_move.ReadValue<Vector2>().x;

        //   ### --- ###
        if (my_sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.3)
        {
            //   ### --- ###
            jumpInput = my_sm.inputAction_jump.triggered;
            if (jumpInput)
            {
                rolling = false;
                my_sm.setInvulnerability(rolling);
                my_sm.energySlider.growFactor = my_sm.energySlider.originalGrowFactor;

                stateMachine.ChangeState(my_sm.jump);
                my_sm.jump.rollInputJump = true;
            }
            //   ### --- ###
            dashInput = my_sm.inputAction_dash.triggered;
            if (dashInput)
            {
                rolling = false;
                my_sm.setInvulnerability(rolling);
                my_sm.energySlider.growFactor = my_sm.energySlider.originalGrowFactor;

                stateMachine.ChangeState(my_sm.dash);
            }
            //   ### --- ###
            lightAttackInput = my_sm.inputAction_light_attack.triggered;
            if (lightAttackInput)
            {
                rolling = false;
                my_sm.setInvulnerability(rolling);
                stateMachine.ChangeState(my_sm.attack);
            }
            //   ### --- ###
            heavyAttackInput = my_sm.inputAction_heavy_attack.triggered;
            if (heavyAttackInput)
            {
                rolling = false;
                my_sm.setInvulnerability(rolling);
                my_sm.attack.typeHeavy = true;
                stateMachine.ChangeState(my_sm.attack);
            }
        }       
        
       
        
       
    }
}
