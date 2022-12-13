using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_state : FSM_BaseState
{
    private FSM_CharMov my_sm;

    public bool typeHeavy;
    private int currComboAttack;
    private bool newAttackRequested;

    public int lightAttackEnergy;
    public int heavyAttackEnergy;
    public attack_state(FSM_CharMov myStateMachine) : base("attack_state", myStateMachine)
    {
        my_sm = (FSM_CharMov)stateMachine;
        
    }


    public override void Enter()
    {
        base.Enter();

        typeHeavy = false;

        currComboAttack = 0;
        newAttackRequested = false;

        lightAttackEnergy = 180;
        heavyAttackEnergy = 300;
    }
   
    
    public override void UpdateLogic()
    {

        handleStateInputs();

        if (typeHeavy)
        {            
            if(my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("heavy_attack") == false && my_sm.energySlider.currValue_ > heavyAttackEnergy)
            {
                my_sm.animator.Play("heavy_attack");
                my_sm.energySlider.modifyEnergyValue(-heavyAttackEnergy);
                my_sm.audioSFX.playSound(my_sm.audioSFX.swordSlash1);
            }
            else if(my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("heavy_attack") == true && my_sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                my_sm.ChangeState(my_sm.idle);
                //my_sm.audioSFX.playSound(my_sm.audioSFX.errorAttack);
            }else if(my_sm.energySlider.currValue_ < heavyAttackEnergy && my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("heavy_attack") == false)
            {
                my_sm.ChangeState(my_sm.idle);
               //my_sm.audioSFX.playSound(my_sm.audioSFX.errorAttack);
            }
        }
        else if(!typeHeavy)
        {
            
            if(my_sm.energySlider.currValue_ > lightAttackEnergy)
            {
                if (my_sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
                {

                    lightAttack(currComboAttack);
                }
            }
            else if(my_sm.energySlider.currValue_ < lightAttackEnergy && my_sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                my_sm.ChangeState(my_sm.idle);
                //my_sm.audioSFX.playSound(my_sm.audioSFX.errorAttack);
            }                      
        }
    }

    void lightAttack(int currAttack)
    {
        switch (currAttack)
        {
            case 0:
                {
                    if (my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("combo_attack_1") != true)
                    {
                        my_sm.animator.Play("combo_attack_1");
                        my_sm.energySlider.modifyEnergyValue(-lightAttackEnergy);
                        my_sm.audioSFX.playSound(my_sm.audioSFX.swordSlash3);
                        Vector2 velDir = my_sm.rigidBody.velocity;
                        velDir.x = my_sm.speed * my_sm.lastDirectionInput;
                        my_sm.rigidBody.velocity = velDir;
                    }
                    else if (my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("combo_attack_1") == true)
                    {
                        if (my_sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && newAttackRequested == true)
                        {
                            currComboAttack++;
                            newAttackRequested = false;
                        }
                        if (my_sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && newAttackRequested == false && currComboAttack == 0)
                        {
                           
                            my_sm.ChangeState(my_sm.idle);  
                        }
                    }
                    break;
                }
            case 1:
                {
                   
                    if (my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("combo_attack_2") != true)
                    {
                        my_sm.animator.Play("combo_attack_2");
                        my_sm.energySlider.modifyEnergyValue(-lightAttackEnergy);
                        my_sm.audioSFX.playSound(my_sm.audioSFX.swordSlash2);
                        Vector2 velDir = my_sm.rigidBody.velocity;
                        velDir.x = my_sm.speed * my_sm.lastDirectionInput;
                        my_sm.rigidBody.velocity = velDir;
                    }
                    else if (my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("combo_attack_2") == true)
                    {
                        if (my_sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && newAttackRequested == true)
                        {
                            currComboAttack++;
                            newAttackRequested = false;
                        }
                        if (my_sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && newAttackRequested == false && currComboAttack == 1)
                        {

                            my_sm.ChangeState(my_sm.idle);
                        }
                    }
                    break;
                }
            case 2:
                {

                    if (my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("heavy_attack") != true)
                    {
                        my_sm.animator.Play("heavy_attack");
                        my_sm.energySlider.modifyEnergyValue(-lightAttackEnergy);
                        my_sm.audioSFX.playSound(my_sm.audioSFX.swordSlash1);
                        Vector2 velDir = my_sm.rigidBody.velocity;
                        velDir.x = my_sm.speed * my_sm.lastDirectionInput;
                        my_sm.rigidBody.velocity = velDir;
                    }
                    else
                    {
                        if (my_sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                        {
                            my_sm.ChangeState(my_sm.idle);
                        }
                    }
                    break;
                }
        }
    }


    public void handleStateInputs()
    {
      
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
        if (lightAttackInput && my_sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2)
        {
            newAttackRequested = true;
        }       
    }
}
