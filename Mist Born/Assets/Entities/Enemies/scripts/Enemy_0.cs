using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;
using Pathfinding;
public class Enemy_0 : Enemy
{

    
    public FSM_CharMov playerScript;
    public EntityManager entityManager;

    public float timeSinceLastCombo;
    public float lastComboTime = 0;
    public bool comboFinished = false;
    public bool attackFinished = true;
    public bool distanceAttack = false;


    public Enemy_0(string name_, State initialState, Vector2 initialPos) : base(name_,initialState,initialPos)
    {

    }
   
    private void Awake()
    {
        
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();     
        InvokeRepeating("UpdatePath", 0f, 0.5f);    
        currentState = State.waitManagerOrders;
        speed = 3;

        timeSinceLastCombo = 0f;

        playerVisible = false;

        wanderArround = true;
        chasePlayer = false;
        attackPlayer = false;
        toDie = false;
    }

    void UpdatePath()
    {
        

    }

    void FixedUpdate()
    {
        distanceToPlayer = playerDistance(playerGObj);
        playerVisible = isPlayerVisible();
        timeSinceLastCombo = Time.time - lastComboTime;
        lookToPlayer();

        
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
                    if (distanceAttack)
                    {
                        attackPlayer = true;
                        chasePlayer = false;
                        break;
                    }

                    break;
                }
            case State.attacking:
                {
                    if (!distanceAttack)
                    {
                        meleeAttackPlayerFunc();
                    }
                    if (distanceToPlayer > attackRange && !distanceAttack)
                    {
                        attackPlayer = false;
                        chasePlayer = true;
                    }else if (distanceAttack)
                    {
                        distanceAttackPlayerFunc();
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

   void lookToPlayer()
    {
        if(currentState != State.die || currentState != State.wandering)
        {
            switch (currentState)
            {
                case State.chasing:
                    if (gameObject.transform.position.x > playerGObj.transform.position.x)
                    {
                        gameObject.transform.localScale = new Vector3(2.85f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                    }
                    else
                    {
                        gameObject.transform.localScale = new Vector3(-2.85f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                    }
                    break;
                case State.attacking:
                    if (attackFinished)
                    {
                        if (gameObject.transform.position.x > playerGObj.transform.position.x)
                        {
                            gameObject.transform.localScale = new Vector3(2.85f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                        }
                        else
                        {
                            gameObject.transform.localScale = new Vector3(-2.85f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                        }
                    }
                    break;
            }
            
        }
    }
    public void wanderArroundFunc()
    {
        animator.Play("BOD_idle");
    }

    public void meleeAttackPlayerFunc()
    {   
        if(timeSinceLastCombo > 2)
        {
            lastComboTime = Time.time;
            animator.Play("BOD_attack_melee");
            
            attackFinished = false;//for lookToPlayerFunc
        }
       if( animator.GetCurrentAnimatorStateInfo(0).IsName("BOD_attack_melee") == true && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            attackFinished = true;
            animator.Play("BOD_idle");
        }
    }
    public void distanceAttackPlayerFunc()
    {
        lastComboTime = Time.time;
        animator.Play("BOD_attack_castSpell");
        attackFinished = false;//for lookToPlayerFunc
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BOD_attack_castSpell") == true && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            attackFinished = true;
            distanceAttack = false;
            animator.Play("BOD_idle");
        }
    }
    public void chasePlayerFunc(GameObject player)
    {
        animator.Play("BOD_walk");
        
        Vector2 direction = ((Vector2)playerGObj.transform.position - rb.position);
        direction = direction.normalized;
        Vector2 force = direction * speed;
        force.y = 0;
        rb.linearVelocity = force;
        //rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, (Vector2)playerGObj.transform.position);

        if (timeSinceLastCombo > 4)
        {
            distanceAttack = true;
        }

       
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

