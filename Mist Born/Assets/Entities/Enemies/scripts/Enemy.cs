﻿using System.Collections;
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

    public GameObject playerGObj;

    public State previousState;
    public State currentState;

    public float visionRange;
    public float attackRange;

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

    public bool isPlayerVisible()
    {
        return calculateVisibility(playerGObj, this.gameObject);
    }

    public bool calculateVisibility(GameObject gObjToBeSeen, GameObject gObjToLook)
    {
        return true;
    }
}