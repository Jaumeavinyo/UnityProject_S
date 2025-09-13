using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class attack_state : FSM_BaseState
{
    private FSM_CharMov my_sm;

    public bool typeHeavy;
    public int currCombo1Attack;
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

        currCombo1Attack = 0;
        newAttackRequested = false;

        lightAttackEnergy = 180;
        heavyAttackEnergy = 300;
    }
   
    
    public override void UpdateLogic()
    {

        handleStateInputs();

        if (typeHeavy)
        {
            currCombo1Attack = 2; //automatic heavy atatck, so anim event that will activate collider knows the attackinfo
            if(my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("heavy_attack") == false && my_sm.energySlider.currValue_ > heavyAttackEnergy)//instant change to anim heavy attack
            {
                my_sm.animator.Play("heavy_attack");
                my_sm.energySlider.modifyEnergyValue(-heavyAttackEnergy);
                my_sm.audioSFX.playSound(my_sm.audioSFX.swordSlash1);
            }
            else if(my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("heavy_attack") == true && my_sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                chooseStateAfterAttack();
                //my_sm.audioSFX.playSound(my_sm.audioSFX.errorAttack);
            }
            else if(my_sm.energySlider.currValue_ < heavyAttackEnergy && my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("heavy_attack") == false)//should never enter here, state input handle already see this condition
            {
                chooseStateAfterAttack();
                //my_sm.audioSFX.playSound(my_sm.audioSFX.errorAttack);
            }
        }
        else if(!typeHeavy)
        {
            
            if(my_sm.energySlider.currValue_ > lightAttackEnergy)
            {
                if ((my_sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f && my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("combo_attack_1")))//prev attack not finished
                {

                    lightAttack(currCombo1Attack);
                }else if (!my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("combo_attack_1"))//instant change to anim light attack
                {
                    lightAttack(currCombo1Attack);
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
        float runAnimTime = my_sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        switch (currAttack)
        {
            case 0:
                {
                    if (my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("combo_attack_1") != true)
                    {
                        my_sm.animator.Play("combo_attack_1");
                        my_sm.energySlider.modifyEnergyValue(-lightAttackEnergy);
                        my_sm.audioSFX.playSound(my_sm.audioSFX.swordSlash3);
                        Vector2 velDir = my_sm.rigidBody.linearVelocity;
                        velDir.x = my_sm.speed * my_sm.lastDirectionInput;
                        my_sm.rigidBody.linearVelocity = velDir;
                    }
                    else if (my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("combo_attack_1") == true)
                    {
                    
                        if (runAnimTime >= 1 && newAttackRequested == true)
                        {
                            currCombo1Attack++;
                            newAttackRequested = false;
                        }
                        if (runAnimTime >= 1 && newAttackRequested == false && currCombo1Attack == 0)
                        {
                            chooseStateAfterAttack();
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
                        Vector2 velDir = my_sm.rigidBody.linearVelocity;
                        velDir.x = my_sm.speed * my_sm.lastDirectionInput;
                        my_sm.rigidBody.linearVelocity = velDir;
                    }
                    else if (my_sm.animator.GetCurrentAnimatorStateInfo(0).IsName("combo_attack_2") == true)
                    {

                        if (runAnimTime >= 1 && newAttackRequested == true)
                        {
                            currCombo1Attack++;
                            newAttackRequested = false;
                        }
                        if (runAnimTime >= 1 && newAttackRequested == false && currCombo1Attack == 1)
                        {

                            chooseStateAfterAttack();
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
                        Vector2 velDir = my_sm.rigidBody.linearVelocity;
                        velDir.x = my_sm.speed * my_sm.lastDirectionInput;
                        my_sm.rigidBody.linearVelocity = velDir;
                    }
                    else
                    {

                        if (runAnimTime >= 1)
                        {
                            chooseStateAfterAttack();
                        }
                    }
                    break;
                }
        }
    }

    Attack getAttackInfo(int currcomboattac)//currcomboattack num is the same as type
    {
        foreach (Attack element in my_sm.attackDefinitions)
        {          
            if((int)element.type == currcomboattac)
            {
                return element;
            }
        }
        return my_sm.attackDefinitions[0];
    }
    // This can be called by animation events
    public void ActivateAttackCollider()
    {
        Attack attack = getAttackInfo(currCombo1Attack);

        Vector2 AttackDirectionCorrection;

    
        AttackDirectionCorrection = new Vector2(attack.collider.offset.x, attack.collider.offset.y);
        

        my_sm.attackCollider2D.offset = AttackDirectionCorrection;      
        my_sm.attackCollider2D.size = attack.collider.size;

        my_sm.attackGameObj.SetActive(true);
       
        
    }

    public void DeactivateAttackCollider()
    {
        my_sm.attackGameObj.SetActive(false);
    }
    void chooseStateAfterAttack()
    {
        if(my_sm.lastDirectionInput == 0.0f)
        {
            my_sm.attackGameObj.SetActive(false);//in case anim is cut by enemy attack or somth
            my_sm.ChangeState(my_sm.idle);
        }else if(my_sm.lastDirectionInput != 0.0f)
        {
            my_sm.attackGameObj.SetActive(false);//in case anim is cut by enemy attack or somth
            my_sm.ChangeState(my_sm.run);
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
