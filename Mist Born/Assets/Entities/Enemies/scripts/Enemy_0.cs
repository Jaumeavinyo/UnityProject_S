using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class Enemy_0 : Enemy
{

    
    public FSM_CharMov playerScript;
    public EntityManager entityManager;

    public Enemy_0(string name_, State initialState, Vector2 initialPos) : base(name_,initialState,initialPos)
    {

    }
   
    private void Awake()
    {
        
    }
    void Start()
    {
        currentState = State.waitManagerOrders;

        playerVisible = false;

        wanderArround = true;
        chasePlayer = false;
        attackPlayer = false;
        toDie = false;
    }

   
    void Update()
    {
        distanceToPlayer = playerDistance(playerGObj);
        playerVisible = isPlayerVisible();

        switch (currentState)
        {
            case State.wandering:
                {
                    wanderArroundFunc();
                    if (distanceToPlayer < visionRange)
                    {
                        if (playerVisible)
                        {
                            wanderArround = false;
                            chasePlayer = true;
                            break;
                        }
                    }

                    break;
                }
            case State.chasing:
                {
                    chasePlayerFunc(playerGObj);
                    if (distanceToPlayer > visionRange)
                    {
                        chasePlayer = false;
                        wanderArround = true;
                        break;
                    }
                    if (distanceToPlayer < attackRange)
                    {
                        attackPlayer = true;
                        chasePlayer = false;
                        break;
                    }

                    break;
                }
            case State.attacking:
                {

                    if (distanceToPlayer > attackRange)
                    {
                        attackPlayer = false;
                        chasePlayer = true;
                    }

                    if (playerScript.playerHP <= 0)
                    {
                        wanderArround = true;
                        attackPlayer = false;
                    }

                    break;
                }
            case State.die:
                {
                    
                    break;
                }
            case State.waitManagerOrders:
                
                {
                    break;
                }
        }


        handleCurrentState();
        

    }

    public void wanderArroundFunc()
    {
        animator.Play("BOD_idle");
    }

    public void chasePlayerFunc(GameObject player)
    {
        animator.Play("BOD_walk");
    }
    public void handleCurrentState()
    {
        if (wanderArround == true && currentState != State.wandering)
        {
            entityManager.requestStateChange(State.wandering, this);
        }
        else if (chasePlayer == true && currentState != State.chasing)
        {
            entityManager.requestStateChange(State.chasing, this);
        }else if(attackPlayer == true && currentState != State.attacking)
        {
            entityManager.requestStateChange(State.attacking, this);
        }else if(toDie == true && currentState != State.die)
        {
            entityManager.requestStateChange(State.die, this);
        }
    }

    private void OnGUI()
    {
        string string_ = "null1";
        string string2 = "null2";
        if (currentState == State.wandering)
        {
             string_ = "Enemy_0 State = State.wandering";
        }
        if (currentState == State.chasing)
        {
             string_ = "Enemy_0 State = State.chasing";
        }
        if (currentState == State.attacking)
        {
             string_ = "Enemy_0 State = State.attacking";
        }
        if (currentState == State.waitManagerOrders)
        {
             string_ = "Enemy_0 State = State.waitManagerOrders";
        }
        if (currentState == State.die)
        {
             string_ = "Enemy_0 State = State.die";
        }

        if (previousState == State.wandering)
        {
            string2 = "Enemy_0 prevState = State.wandering";
        }
        if (previousState == State.chasing)
        {
            string2 = "Enemy_0 prevState = State.chasing";
        }
        if (previousState == State.attacking)
        {
            string2 = "Enemy_0 prevState = State.attacking";
        }
        if (previousState == State.waitManagerOrders)
        {
            string2 = "Enemy_0 prevState = State.waitManagerOrders";
        }
        if (previousState == State.die)
        {
            string2 = "Enemy_0 prevState = State.die";
        }


        GUILayout.Label(string2+"    "+string_);

        
    }
}

