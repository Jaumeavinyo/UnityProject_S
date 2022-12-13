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

    // Update is called once per frame
    void Update()
    {
        
    }
}
