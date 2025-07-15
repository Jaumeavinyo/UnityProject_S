using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallJumping_state : FSM_BaseState
{
    private FSM_CharMov my_sm;

  

    public wallJumping_state(FSM_CharMov myStateMachine) : base("wallJumping_state", myStateMachine)
    { 
        my_sm = (FSM_CharMov)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        horizontalInput = 0;
        jumpInput = false;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        Vector2 velDir = my_sm.rigidBody.linearVelocity;
        //velDir.y;
    }

}
