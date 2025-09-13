using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections;
using UnityEngine;

public enum BossState
{
    NONE,
    WANDERING,
    CHASE,
    ATTACK,
    JUMP
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

    public BoxCollider2D groundSlamCollider;
    public float groundSlamColliderPosX;

    public BoxCollider2D fireThrowerCollider;
    public float fireThrowerColliderPosX;

    public BoxCollider2D getDamageCollider;
    public float getDamageCollOffsetGroundSlamX; //+1 +-
    public Material whiteDamageMat;
    Material originalBossMat;

    public BossHealthSlider healthSlider;

    Vector3 spawnPos;

    float moveDir;
    bool bMoveDir;
    
    public float runAnimationSpeedCorrectionLegPull;
    public float runAnimationSpeedCorrectionLegLand;
    public float spotDistance;
    public float meleeAttackDistance;
    public float chaseSpeed;

    float lastAttackTime;
    bool bAttackNowMelee;

    //jump attack
    bool jumping;
    bool falling;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        prevBossState = BossState.NONE;
        currBossState = BossState.WANDERING;

        spawnPos = gameObject.transform.position;

        bAttackNowMelee = false;
        bMoveDir = true;

        groundSlamCollider.enabled = false;
        fireThrowerCollider.enabled = false;

        jumping = false;
        falling = false;
    }

    void Update()
    {
        //if (bMoveDir)
        //{
        //    SetSpriteDirection();
        //}

       
        SetCurrentAnim();
        
    }

    void FixedUpdate()
    {

        float runAnimTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;

        switch (currBossState)
        {
            case BossState.WANDERING:
                {
                    SetSpriteDirection();
                    checkBossState();
                    break;
                }
            case BossState.CHASE:
                {
                    //groundSlamCollider.enabled = false;
                    //fireThrowerCollider.enabled = false;
                    SetSpriteDirection();
                   
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
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_LegSlam") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_FireThrower") && !bAttackNowMelee)
                    {
                        bAttackNowMelee = true;
                        bMoveDir = false;
                    }
                    else if ((animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_LegSlam") || animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_FireThrower")) && runAnimTime >= 0.99f)
                    {
                        checkBossState();
                        if (currBossState == BossState.CHASE)
                        {
                            bMoveDir = true;
                        }
                        else
                        {
                            animator.Play("Boss_Idle");
                        }

                    }         

                    break;
                }
            case BossState.JUMP:
                {

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
                        prevBossState = currBossState;
                        currBossState = BossState.CHASE;
                        saveLastAttackTime();
                    }
                    break;
                }
            case BossState.CHASE:
                {

                    if (Mathf.Abs(player.transform.position.x - this.transform.position.x) < meleeAttackDistance)
                    {
                        prevBossState = currBossState;
                        currBossState = BossState.ATTACK;
                        Vector2 velDir = rb.linearVelocity;
                        velDir.x = 0.0f;
                        rb.linearVelocity = velDir;
                    }

                    if(UnityEngine.Time.time - lastAttackTime > 4.0)
                    {
                        //here the jump
                    }

                    break;
                }
            case BossState.ATTACK:
                {
                    if(Mathf.Abs(player.transform.position.x - this.transform.position.x) > meleeAttackDistance && !animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_Idle") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >=0.99)
                    {
                        prevBossState = currBossState;
                        currBossState = BossState.CHASE;
                        saveLastAttackTime();
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
                    if (bAttackNowMelee)
                    {
                        float rand = UnityEngine.Random.Range(1, 10);
                        if (rand > 5)
                        {
                            animator.Play("Boss_FireThrower");
                            bAttackNowMelee = false;
                        }
                        else
                        {
                            animator.Play("Boss_LegSlam");
                            bAttackNowMelee = false;
                        }                                              
                    }
                    break;
                }
            //case BossState.JUMP:
            //    {
            //        //enters once at the beggining
            //        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_JumpUp") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_JumpDown") && !jumping)
            //        {
            //            animator.Play("Boss_JumpUp");
            //            jumping = true;
            //        }else if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_JumpDown") && !falling )
            //        break;
            //    }
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
            fireThrowerColliderPosX = this.transform.position.x - 3.0f;
        }
        else
        {
            // Facing right
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            moveDir = 1;
            groundSlamColliderPosX = this.transform.position.x + 3.0f;
            fireThrowerColliderPosX = this.transform.position.x + 3.0f;
        }

        float SlamColliderY = transform.position.y - bossBoxCollider.bounds.size.y + groundSlamCollider.GetComponent<BoxCollider2D>().bounds.size.y;
        groundSlamCollider.transform.position = new Vector3(groundSlamColliderPosX, groundSlamCollider.GetComponent<BoxCollider2D>().transform.position.y, this.transform.position.z);

        float fireColliderY = transform.position.y - (bossBoxCollider.bounds.size.y / 2) + fireThrowerCollider.GetComponent<BoxCollider2D>().bounds.size.y;
        fireThrowerCollider.transform.position = new Vector3(fireThrowerColliderPosX, fireThrowerCollider.GetComponent<BoxCollider2D>().transform.position.y, this.transform.position.z);
    }

    public void SetDamageColliderOffsetChange(float offsetX)
    {
        getDamageCollider.offset = new Vector2(offsetX, getDamageCollider.offset.y);
    }

    public void HitWhiteFeedBack(Vector3 color)
    {
        

        
    }

    public IEnumerator SpriteWhiteFlash(float t)
    {       
        originalBossMat = spriteRenderer.sharedMaterial;     
        spriteRenderer.material = whiteDamageMat;
        yield return new WaitForSeconds(t);
        spriteRenderer.material = originalBossMat;
    }

    public IEnumerator FreezeAnimation(float t)
    {
        float originalSpeed = animator.speed;
        animator.speed = 0f;

        yield return new WaitForSeconds(t);

        animator.speed = originalSpeed;
    }

    void saveLastAttackTime()
    {
        lastAttackTime = UnityEngine.Time.time;
    }

    public void Die()
    {

    }
}
