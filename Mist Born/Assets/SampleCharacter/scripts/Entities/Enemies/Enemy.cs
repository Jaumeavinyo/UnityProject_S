using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    wandering,
    following,
    attacking,
    diying
}

public class Enemy : Entity
{

    public string name;
    protected State currentState;

    public void changeState(State newState)
    {
        currentState = newState;
    }
    // Start is called before the first frame update

    public Enemy(string name_, State initialState, Vector2 initialPos) : base(initialPos)
    {
        this.name = name_;
        this.currentState = initialState;
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
