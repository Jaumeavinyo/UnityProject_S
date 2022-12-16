using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public Animator animator;

    public string name;
    public Vector2 Position;

    protected float distanceToPlayer;

    public Entity(Vector2 initialPos)
    {
        this.Position = initialPos;
    }

    void Start()
    {
       distanceToPlayer = 0;
    }


    void Update()
    {

    }



}
