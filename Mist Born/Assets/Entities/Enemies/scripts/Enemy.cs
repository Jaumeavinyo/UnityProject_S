using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum State
{
    wandering,
    chasing,
    attacking,
    waitManagerOrders,
    die
}
public class Enemy : Entity
{

    public float speed;

    public GameObject playerGObj;
    public Rigidbody2D rb;

    public State previousState;
    public State currentState;

    public float visionRange;
    public float attackRange;

    public float  playerRelativePos;

   

    public bool playerVisible;

    public bool wanderArround;
    public bool chasePlayer;
    public bool attackPlayer;
    public bool toDie;
    public Enemy(string name_, State initialState, Vector2 initialPos) : base(initialPos)
    {
        this.name = name_;
        this.currentState = initialState;
        this.previousState = currentState;
    }

    


    public float playerDistance(GameObject player)
    {
        return Vector2.Distance(player.transform.position, this.transform.position);
    }

    public bool isPlayerVisible(float distance_)
    {
        return calculateVisibility(playerGObj, this.gameObject,distance_);
    }

    public bool calculateVisibility(GameObject gObjToBeSeen, GameObject gObjToLook, float distance_)
    {
        bool ret = true;

        if(distance_ > visionRange)
        {
            ret = false;
        }

        //here create lines or maybe better rectangle to see if enemy can see you (a line could barely fit a hole and tell the enemy that can see u)

        return true;
    }
}
