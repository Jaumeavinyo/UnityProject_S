using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FSM_BaseState 
{

    public string   name;
    protected FSM   stateMachine;
    public int      internalCase;

    public float    horizontalInput;
    public bool     jumpInput;
    public bool     dashInput;
    public bool     rollInput;
    public bool     slideInput;
    public bool     lightAttackInput;
    public bool     heavyAttackInput;

   
    public FSM_BaseState(string name, FSM stateMachine,int case_ = 0)
    {
        this.name = name;
        this.stateMachine = stateMachine;
        this.internalCase = case_;
    }

   

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }

   
}
