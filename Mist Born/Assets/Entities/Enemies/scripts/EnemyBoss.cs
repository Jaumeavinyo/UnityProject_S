using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections;
using System.Threading;
using UnityEngine;

public enum BossState
{
    NONE,
    WANDERING,
    CHASE,
    ATTACK,
    JUMP,
    FALL_SLAM,
    DEAD
}


public class EnemyBoss : MonoBehaviour
{
    public float bossScale;

    public BossState currBossState;
    private BossState prevBossState;

    public FSM_CharMov player;
    public SmartCamera camera;

    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Rigidbody2D rb;
    public BoxCollider2D bossBoxCollider;

    public BoxCollider2D groundSlamCollider;
    public float groundSlamColliderPosX;

    public BoxCollider2D fireThrowerCollider;
    public float fireThrowerColliderPosX;

    public BoxCollider2D jumpSlamCollider;

    public BoxCollider2D getDamageCollider;
    public float getDamageCollOffsetGroundSlamX; //+1 +-
    public Material whiteDamageMat;
    Material originalBossMat;

    public GameObject SliderContainer;
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
    bool jumpNow;
    bool jumping;
    bool falling;
    bool fallNow;

    float fallSlamPosX;
    public float waitTimeFallSlam;//the time before falling after last targetting of player position
    public float targettingTimeFallSlam;//the time the boss is in the air updating player target
    private float FallSlamTimer;
    bool FallSlamTargetting;
    public float timeBetweenJumps;

    //control if player is being hit by boss attacks
    public int attacksPerformed;
    public int lastAttackHit;

    //for the boss appearing
    public bool entranceDone;
    public GameObject entranceFallPos;
    public bool playerDetected;

    //camera shakes
    bool groundSlamCamShake;
    bool jumpSlamCamShake;
    bool fallSlamCamShake;



    bool fireSound;

    private void Awake()
    {
        originalBossMat = spriteRenderer.material;
    }
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
        jumpSlamCollider.enabled = false;

        jumping = false;
        falling = false;
        jumpNow = false;
        fallNow = false;
        fallSlamPosX = 0.0f;
        FallSlamTargetting = false;

        attacksPerformed = 0;
        lastAttackHit = 0;

        entranceDone = false;


        groundSlamCamShake = false; 
        jumpSlamCamShake = false;
        fallSlamCamShake = false;

        playerDetected = false;

        SetSpriteDirection();

        fireSound = false;
    }

    void Update()
    {
        if (playerDetected)
        {
            MusicManager.Instance.PlaySFX(MusicManager.Instance.monsterWalkingInDistance);          
            StartCoroutine(EntranceCoroutine());
            playerDetected = false;
        }


        if (entranceDone && currBossState != BossState.DEAD)
        {

            SetCurrentAnim();
        }
        

    }
    private IEnumerator EntranceCoroutine()
    {
        float startTime = Time.time;

        // Shake every 2 seconds until 8s
        while (Time.time - startTime < 8f)
        {
            camera.DirectionalShake(0.5f, 10f, 0.3f, 0.3f, 0.3f, 0.3f);
            Debug.Log("SHAKEEEEEEEEE");
            yield return new WaitForSeconds(2f);
        }

        // Wait until 12s total
        float elapsed = Time.time - startTime;
        if (elapsed < 12f)
        {
            yield return new WaitForSeconds(12f - elapsed);
        }

        // Final shake at 12s
        camera.DirectionalShake(0.5f, 30f, 0.4f, 0.4f, 0.4f, 0.4f);
        currBossState = BossState.JUMP;        
        entranceDone = true;

       
        
    }
    void FixedUpdate()
    {
        if (!entranceDone) return;
        float runAnimTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
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
                    bool playerInFront = (moveDir == 1 && player.transform.position.x > transform.position.x) ||
                         (moveDir == -1 && player.transform.position.x < transform.position.x);
                    if(!playerInFront && runAnimTime >= 0.99f)
                    {
                        SetSpriteDirection();
                    }

                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_FireThrower") && runAnimTime >= 0.20 && runAnimTime < 0.50)
                    {
                        
                       
                        Vector2 velDir = rb.linearVelocity;
                        velDir.x = chaseSpeed * -moveDir;
                        rb.linearVelocity = velDir;
                    }

                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_LegSlam") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_FireThrower") && !bAttackNowMelee)
                    {
                        if(attacksPerformed - lastAttackHit >= 2)
                        {
                            changeBossState(BossState.JUMP);
                            Debug.Log("BossState.JUMP_");
                            lastAttackHit = attacksPerformed;//to avoid infinite jumps
                            break;
                        }
                        else
                        {
                            bAttackNowMelee = true;
                            bMoveDir = false;
                        }
                       
                    }                
                    else if ((animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_LegSlam") || animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_FireThrower")) && runAnimTime >= 0.99f)
                    {
                        checkBossState();
                        
                        if (currBossState == BossState.CHASE)
                        {
                            Debug.Log("BossState.CHASE_");
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
                    groundSlamCollider.enabled = false;
                    fireThrowerCollider.enabled = false;

                    if (!jumping && !jumpNow)
                    {
                        jumpNow = true;
                    }

                    if (!jumping && jumpNow)
                    {                        
                        animator.Play("Boss_JumpUp");
                        jumping = true;
                        jumpNow = false;
                        getDamageCollider.enabled = false;
                    }
                    if(runAnimTime >= 0.40&& !jumpSlamCamShake)
                    {
                        jumpSlamCamShake = true;
                        if (!entranceDone)
                        {
                            camera.DirectionalShake(0.6f, 15.0f, 0.2f, 0.2f, 0.2f, 0.5f);
                        }
                        else
                        {
                            camera.DirectionalShake(0.25f, 20.0f, 0.2f, 0.2f, 2.0f, 0.5f);
                        }
                        
                    }
                    // Start targetting once jump anim finished
                    if (runAnimTime >= 0.99f && !FallSlamTargetting)
                    {
                        FallSlamTargetting = true;
                        FallSlamTimer = Time.time;
                    }

                    // While within targeting time, update fall position
                    if (FallSlamTargetting && (Time.time - FallSlamTimer) < targettingTimeFallSlam)
                    {
                        if (entranceDone)//already appeared to the player
                        {
                            fallSlamPosX = player.transform.position.x;
                            transform.position = new Vector3(fallSlamPosX, transform.position.y, transform.position.z);
                        }
                        else
                        {
                            fallSlamPosX = entranceFallPos.transform.position.x;
                            transform.position = new Vector3(fallSlamPosX, transform.position.y, transform.position.z);
                        }
                        
                    }

                    // After targeting time expires, go to fall slam
                    if (FallSlamTargetting && (Time.time - FallSlamTimer) >= targettingTimeFallSlam)
                    {
                        if (!entranceDone)
                        {
                            entranceDone = true;
                        }
                        FallSlamTargetting = false;
                        FallSlamTimer = Time.time; // prepare for waitTimeFallSlam
                        Debug.Log("BossState.FALL_SLAM_");
                        MusicManager.Instance.PlaySFX(MusicManager.Instance.MonsterFalling);
                        changeBossState(BossState.FALL_SLAM);
                    }

                    break;
                }

            case BossState.FALL_SLAM:
                {
                    jumpSlamCamShake = false;
                    // Wait before falling
                    if ((Time.time - FallSlamTimer) >= waitTimeFallSlam && !fallNow && !stateInfo.IsName("Boss_Idle"))
                    {                        
                        fallNow = true;
                        animator.Play("Boss_JumpDown");
                        
                        SetSpriteDirection();
                    }

                    // When landing anim is done, return to wandering
                    if (fallNow)
                    {
                        //Debug.Log("FALL ANIM" + runAnimTime);
                    }

                    if(fallNow && stateInfo.IsName("Boss_JumpDown") && stateInfo.normalizedTime >= 0.15 && !fallSlamCamShake)
                    {
                        fallSlamCamShake = true;
                        camera.DirectionalShake(0.40f, 30.0f, 0.2f, 0.2f, 0.5f, 2.0f);
                    }

                    if (fallNow && stateInfo.IsName("Boss_JumpDown") && stateInfo.normalizedTime >= 1f)
                    {
                        animator.Play("Boss_Idle");
                        SetSpriteDirection();
                        saveLastAttackTime();
                        lastAttackHit = attacksPerformed;
                        jumping = false; // reset for next jump
                        fallNow = false;            
                    }

                    if(stateInfo.IsName("Boss_Idle") && stateInfo.normalizedTime >= 1f)
                    {
                        getDamageCollider.enabled = true;
                        
                        checkBossState();
                        fallSlamCamShake = false;
                        if(SliderContainer.activeSelf == false)
                        {
                            SliderContainer.SetActive(true);
                            MusicManager.Instance.PlayMusic(MusicManager.Instance.battleTheme);
                        }
                    }

                    break;
                }
            case BossState.DEAD:

                    break;
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
                        Debug.Log("BossState.CHASE");

                    }
                    break;
                }
            case BossState.CHASE:
                {

                    if (Mathf.Abs(player.transform.position.x - this.transform.position.x) < meleeAttackDistance)
                    {
                        prevBossState = currBossState;
                        currBossState = BossState.ATTACK;
                        Debug.Log("BossState.ATTACK");
                        Vector2 velDir = rb.linearVelocity;
                        velDir.x = 0.0f;
                        rb.linearVelocity = velDir;
                    }

                    if(UnityEngine.Time.time - lastAttackTime > timeBetweenJumps && lastAttackTime != 0.0f)
                    {
                        changeBossState(BossState.JUMP);
                        Debug.Log("BossState.JUMP");
                    }

                    break;
                }
            case BossState.ATTACK:
                {
                    
                    if (Mathf.Abs(player.transform.position.x - this.transform.position.x) > meleeAttackDistance && !animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_Idle") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >=0.99)
                    {
                        prevBossState = currBossState;
                        currBossState = BossState.CHASE;
                        Debug.Log("BossState.CHASE");

                    }             
                    break;
                }
            case BossState.JUMP:
                {

                }
                break;
            case BossState.FALL_SLAM:
                {
                    if (Mathf.Abs(player.transform.position.x - this.transform.position.x) < meleeAttackDistance)
                    {
                        prevBossState = currBossState;
                        currBossState = BossState.ATTACK;
                        Debug.Log("BossState.ATTACK");
                        Vector2 velDir = rb.linearVelocity;
                        velDir.x = 0.0f;
                        rb.linearVelocity = velDir;
                    }
                    else
                    {
                        prevBossState = currBossState;
                        currBossState = BossState.CHASE;
                        Debug.Log("BossState.CHASE");

                    }
                }
                break;
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
                        saveLastAttackTime();
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
       
        }
    }

    void SetSpriteDirection()
    {
        //boss collider size is 3.21
        if (player.transform.position.x < transform.position.x)
        {
            // Facing left
            gameObject.transform.localScale = new Vector3(-bossScale, bossScale, 2);
            moveDir = -1;
            groundSlamColliderPosX = this.transform.position.x - 2.7f* bossScale;
            fireThrowerColliderPosX = this.transform.position.x - 3.0f* bossScale;
        }
        else
        {
            // Facing right
            gameObject.transform.localScale = new Vector3(bossScale, bossScale, 2);
            moveDir = 1;
            groundSlamColliderPosX = this.transform.position.x + 2.7f* bossScale;
            fireThrowerColliderPosX = this.transform.position.x + 3.0f* bossScale;
        }

        float SlamColliderY = transform.position.y - bossBoxCollider.bounds.size.y + groundSlamCollider.GetComponent<BoxCollider2D>().bounds.size.y;
        groundSlamCollider.transform.position = new Vector3(groundSlamColliderPosX, groundSlamCollider.GetComponent<BoxCollider2D>().transform.position.y, this.transform.position.z);

        float fireColliderY = transform.position.y - (bossBoxCollider.bounds.size.y / 2) + fireThrowerCollider.GetComponent<BoxCollider2D>().bounds.size.y;
        fireThrowerCollider.transform.position = new Vector3(fireThrowerColliderPosX, fireThrowerCollider.GetComponent<BoxCollider2D>().transform.position.y, this.transform.position.z);
    }

    public void SetDamageColliderOffsetChange(float offsetX)
    {
        getDamageCollider.offset = new Vector2(offsetX, getDamageCollider.offset.y);
        //getDamageCollider.size = new Vector2(getDamageCollider.size.x + 1.0f, getDamageCollider.size.y);//collider moves forward and attacking from the back of the boss does not hit the collider
    }

    public void changeBossState(BossState newState)
    {
        prevBossState = currBossState;
        currBossState = newState;
    }

    public IEnumerator SpriteWhiteFlash(float t)
    {
        //originalBossMat = spriteRenderer.material;//spriteRenderer.sharedMaterial;  now done in awake  
        spriteRenderer.material = whiteDamageMat;
        yield return new WaitForSeconds(t);
        spriteRenderer.material = originalBossMat;
    }

    public IEnumerator FreezeAnimation(float t)
    {
        if(animator.speed != 0)
        {
            float originalSpeed = animator.speed;
            animator.speed = 0f;

            yield return new WaitForSeconds(t);

            animator.speed = originalSpeed;
        }
        
    }

    void saveLastAttackTime()
    {
        lastAttackTime = UnityEngine.Time.time;
        attacksPerformed += 1;
    }

    public void Die()
    {
        currBossState = BossState.DEAD;
        MusicManager.Instance.audioSource.Stop();
        StartCoroutine(FreezeAnimation(2f));
        StartCoroutine(DieCorroutine());
    }
    private IEnumerator DieCorroutine()
    {
        yield return new WaitForSeconds(2);
        animator.Play("Boss_Die");
    }
}
