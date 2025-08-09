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
    public Animator animator;
    public Rigidbody2D rb;
    public BoxCollider2D bossBoxCollider;

    public GameObject groundSlamCollider;
    public float groundSlamColliderPosX;

    Vector3 spawnPos;

    float moveDir;
    bool bAttackNow;
    public float runAnimationSpeedCorrectionLegPull;
    public float runAnimationSpeedCorrectionLegLand;
    public float spotDistance;
    public float meleeAttackDistance;
    public float chaseSpeed;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        prevBossState = BossState.NONE;
        currBossState = BossState.WANDERING;

        spawnPos = gameObject.transform.position;

        bAttackNow = false;
    }

    void Update()
    {
        SetSpriteDirection();//should not be called if in the middle of a attack or action
        SetCurrentAnim();
        
    }

    void FixedUpdate()
    {

        float runAnimTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;

        switch (currBossState)
        {
            case BossState.WANDERING:
                {
                    checkBossState();
                    break;
                }
            case BossState.CHASE:
                {
                    //total num of frames: 11
                    //leg pulls from: 8- 11
                    //leg lands in 6-7
                    float speedmultiplyer = 1.0f;
                    
                    //when leg pulling
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_Run") && runAnimTime > 0.72f)//8/100*8(the frame)
                    {
                        speedmultiplyer = runAnimationSpeedCorrectionLegPull;

                        //when leg in air or ground
                    }else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_Run") && runAnimTime < 0.72f){
                        speedmultiplyer = runAnimationSpeedCorrectionLegLand;
                    }
                    Vector2 velDir = rb.linearVelocity;
                    velDir.x = chaseSpeed * moveDir*speedmultiplyer;
                    rb.linearVelocity = velDir;

                    checkBossState();

                    break;
                }
            case BossState.ATTACK:
                {
                    

                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_LegSlam"))
                    {
                        bAttackNow = true;
                    }else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_LegSlam") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        checkBossState();
                    }

                    if(animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_LegSlam") && (runAnimTime > 0.28f && runAnimTime < 0.45f))//17 frames, toca suelo en el 5
                    {
                        groundSlamCollider.SetActive(true);
                    }else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_LegSlam") && (runAnimTime < 0.28f || runAnimTime > 0.45f))
                    {
                        groundSlamCollider.SetActive(false);
                    }

                    break;
                }        
        }
    }

    //CHECK FOR STATE CHANGES
    void checkBossState()
    {
        switch (currBossState)
        {
            case BossState.WANDERING:
                {
                    if(Mathf.Abs(player.transform.position.x - this.transform.position.x) < spotDistance)
                    {
                        currBossState = BossState.CHASE;
                    }
                    break;
                }
            case BossState.CHASE:
                {

                    if (Mathf.Abs(player.transform.position.x - this.transform.position.x) < meleeAttackDistance)
                    {
                        currBossState = BossState.ATTACK;
                        Vector2 velDir = rb.linearVelocity;
                        velDir.x = 0.0f;
                        rb.linearVelocity = velDir;
                    }

                    break;
                }
            case BossState.ATTACK:
                {
                    if(Mathf.Abs(player.transform.position.x - this.transform.position.x) > meleeAttackDistance)
                    {
                        currBossState = BossState.CHASE;
                    }


                    break;
                }
        }
    }

    //CHECK FOR ANIM STATES
    void SetCurrentAnim()
    {
        switch (currBossState)
        {
            case BossState.WANDERING:
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_Idle"))
                    {
                        animator.Play("Boss_Idle");
                    }
                    break;
                }
            case BossState.CHASE:
                {
                   
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_Run"))
                    {
                        animator.Play("Boss_Run");
                    }

                    break;
                }
            case BossState.ATTACK:
                {
                    if (bAttackNow)
                    {
                        animator.Play("Boss_LegSlam");                       
                        bAttackNow = false;
                    }


                    break;
                }
        }
    }

    void SetSpriteDirection()
    {
        if ((this.transform.position.x - player.transform.position.x) > 0.0f)
        {
            // Facing left
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            moveDir = -1;
            groundSlamColliderPosX = this.transform.position.x - 3.0f;
        }
        else
        {
            // Facing right
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            moveDir = 1;
            groundSlamColliderPosX = this.transform.position.x + 3.0f;
        }

        float SlamColliderY = transform.position.y - bossBoxCollider.bounds.size.y + groundSlamCollider.GetComponent<BoxCollider2D>().bounds.size.y;
        groundSlamCollider.transform.position = new Vector3(groundSlamColliderPosX, SlamColliderY, this.transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.healthSlider.UpdateSliderValue(30);
            
            //info for the character knockback
            if(gameObject.transform.position.x - player.gameObject.transform.position.x > 0)//boss is right to the player
            {
                player.attackingEnemyDir = -1.0f;
            }
            else
            {
                player.attackingEnemyDir = 1.0f;
            }
            
            player.ChangeState(player.knockback);
            
            
        }
    }
}
