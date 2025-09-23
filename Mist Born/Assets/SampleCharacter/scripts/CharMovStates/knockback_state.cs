using UnityEngine;

public class knockback_state : FSM_BaseState
{
    private FSM_CharMov my_sm;

    bool dead;
    bool knocking;


    public knockback_state(FSM_CharMov myStateMachine) : base("knockback_state", myStateMachine)
    {

        my_sm = (FSM_CharMov)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        knocking = false;
        dead = !my_sm.Alive;
        if (my_sm.attackGameObj.activeSelf)
        {
            my_sm.attackGameObj.SetActive(false);//when attack anim gets cut, animevent to deactivate collider does not trigger
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!dead)
        {
            if (my_sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("Knockback"))
            {
                my_sm.ChangeState(my_sm.idle);               
            }

            if (!knocking)
            {
                knocking = true;
                my_sm.animator.Play("Knockback");
                correctAnimOrientation();
            }
            else if (knocking)
            {
                makeKnockback();
            }
          
        }
        else if (dead)
        {
            if (!knocking)
            {
                knocking = true;
                my_sm.animator.Play("Death");
                correctAnimOrientation();
                Debug.Log("Play anim Death");

            }else if (knocking)
            {
                 makeDeathKnockback();                          
            }
        }

        
    }
    public override void Exit()
    {
        base.Exit();
    }

    void makeKnockback()
    {
        
        Vector2 vel = my_sm.rigidBody.linearVelocity;
        vel.x = my_sm.KnockbackVelocity * my_sm.attackingEnemyDir;
        my_sm.rigidBody.linearVelocity = vel;
    }

    void makeDeathKnockback()
    {
        float t = my_sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if(t<0.7)
        {
            Vector2 vel = my_sm.rigidBody.linearVelocity;
            vel.x = my_sm.deathKnockbackVelocity * my_sm.attackingEnemyDir;
            my_sm.rigidBody.linearVelocity = vel;
        }
        else
        {
            my_sm.rigidBody.linearVelocity = Vector2.zero;
        }        
    }

    void correctAnimOrientation()
    {
        if (my_sm.attackingEnemyDir == -1)
        {
            my_sm.directionInput = 1;
        }
        else if (my_sm.attackingEnemyDir == 1)
        {
            my_sm.directionInput = -1;
        }
    }
}
