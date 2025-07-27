using JetBrains.Annotations;
using UnityEngine;

public enum BossState
{
    NONE,
    WANDERING,
    CHASE,
    ATTACK
}


public class EnemyBoss : MonoBehaviour
{
    private BossState currBossState;
    private BossState prevBossState;

    public FSM_CharMov player;

    public SpriteRenderer spriteRenderer;

    Vector3 spawnPos;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        prevBossState = BossState.NONE;
        currBossState = BossState.WANDERING;

        spawnPos = gameObject.transform.position;
    }

    void Update()
    {
        SetSpriteDirection();//should not be called if in the middle of a attack or action
        
        
    }

    void FixedUpdate()
    {
        switch (currBossState)
        {
            case BossState.WANDERING:
                {                                     

                    break;
                }
            case BossState.CHASE:
                {



                    break;
                }
            case BossState.ATTACK:
                {



                    break;
                }        
        }
    }


    void SetSpriteDirection()
    {     
        if ((this.transform.position.x - player.transform.position.x) > 0.0)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
