using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dash_state : FSM_BaseState
{
    private FSM_CharMov my_sm;

    public int horizontalDash;
    public int dashEnergy;

    public bool dashing;
    public dash_state(FSM_CharMov myStateMachine) : base("dash_state", myStateMachine)
    {

        my_sm = (FSM_CharMov)stateMachine;
    }


    public override void Enter()
    {
        base.Enter();
        horizontalInput = 0;
        dashing = false;
        dashEnergy = 200;
        //dash can not change its direction. we decide direction at entrance of the state
        if (my_sm.lastDirectionInput > 0)
        {
            horizontalDash = 1;
        }else if (my_sm.lastDirectionInput < 0)
        {
            horizontalDash = -1;
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!dashing && my_sm.energySlider.currValue_>dashEnergy)
        {
            dashing = true;
            dash(horizontalDash);
            my_sm.animator.Play("dash");
            my_sm.energySlider.modifyEnergyValue(-dashEnergy);
            my_sm.audioSFX.playSound(my_sm.audioSFX.dash1);
        }
        else if(!dashing && my_sm.energySlider.currValue_ < dashEnergy)
        {
            my_sm.ChangeState(my_sm.idle);
        }

        if (dashing && my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("dash") && my_sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.5)
        {
            chooseStateAfterDash();
        }
                    
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        
    }
    public override void Exit()
    {
        base.Exit();
    }

    public void dash(int dir)
    {
        Vector2 velDir = my_sm.rigidBody.velocity;
        velDir.x = my_sm.dashSpeed * dir;
        my_sm.rigidBody.velocity = velDir;
    }

    public void chooseStateAfterDash()
    {
        if (horizontalInput != 0)
        {
            if (!my_sm.grounded)
            {
                my_sm.ChangeState(my_sm.jump);
            }
            else
            {
                my_sm.ChangeState(my_sm.run);
            }
        }
        else if (horizontalInput == 0)
        {
            if (!my_sm.grounded)
            {
                my_sm.ChangeState(my_sm.jump);
            }
            else
            {
                my_sm.ChangeState(my_sm.idle);
            }
        }
    }

}
