using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public enum grabb
{
    noGrabb,
    leftGrabb,
    rightGrabb,
    topLeftGrabb,
    topRightGrabb
}

public enum AttackType
{
    COMBO1_1,//combo 1 anim
    COMBO1_2,//combo 2 anim
    COMBO1_3_HEAVY,//heavy 1 anim
    HEAVY1
}

[System.Serializable]
public class AttackCollider
{
    public Vector2 offset;
    public Vector2 size;
}

[System.Serializable]
public class Attack
{
    public AttackType type;
    public AttackCollider collider;
}

public class FSM_CharMov : FSM
{
    public bool disabled = false;

    public PlayerInputs     inputActions; //own generated file for inputs new system
    public InputAction      inputAction_jump; //own input added to player inputs
    public InputAction      inputAction_move;
    public InputAction      inputAction_roll;
    public InputAction      inputAction_dash;
    public InputAction      inputAction_heavy_attack;
    public InputAction      inputAction_light_attack;

    public idle_state        idle;
    public run_state         run;
    public jump_state        jump;
    public dash_state        dash;
    public roll_state        roll;
    public attack_state      attack;
    public wallJumping_state wallJump;
    public knockback_state   knockback;

    [Header("Attack Definitions")]
    public List<Attack> attackDefinitions;

    public Rigidbody2D          rigidBody;
    public Animator             animator;
    public SpriteRenderer spriteRenderer;
    public CapsuleCollider2D    capsuleCollider;

    public GameObject attackGameObj;
    public BoxCollider2D attackCollider2D;


    public energySlider energySlider;
    public HealthSlider healthSlider;

    public characterSFX audioSFX;

    public float attackingEnemyDir;

    public bool Alive = true;
    public float playerHP = 1000;

    public float        speed = 5;
    public float        dashSpeed = 10;
    public float        rollSpeed = 10;
    public float        jumpForce = 6;

    public int doubleJumpEnergy = 100;

    public float        directionInput;
    public float        lastDirectionInput;

    public bool         grounded;
    public float        groundDistanceDetection;

    public grabb        wallGrabbed;
    public float        wallDistanceDetection;
    public bool         topGrab;
    public bool         bottomGrab;

    bool deathMovementApplied = false;
    public float deathKnockbackVelocity=0;
    public float KnockbackVelocity=0;

    public bool characterFreezed;
    
    private void Awake()
    {

        inputActions = new PlayerInputs();

        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        attackCollider2D = attackGameObj.GetComponent<BoxCollider2D>();

        idle = new idle_state(this);
        run = new run_state(this);
        jump = new jump_state(this);
        dash = new dash_state(this);
        roll = new roll_state(this);
        attack = new attack_state(this);
        knockback = new knockback_state(this);


        grounded = isGrounded();
        wallGrabbed = isWallGrabbed();

        directionInput = 0;
        lastDirectionInput = 0;

        groundDistanceDetection = 0.4f;
        wallDistanceDetection = 0.4f;
        characterFreezed = false;
    }

    public override void Update()
    {

        if (!disabled)
        {
            if (currentState != knockback)
            {
                if (inputAction_move.ReadValue<Vector2>().x > 0)
                {
                    directionInput = 1;
                }
                else if (inputAction_move.ReadValue<Vector2>().x < 0)
                {
                    directionInput = -1;
                }
            }

            setDirection();

            grounded = isGrounded();
            wallGrabbed = isWallGrabbed();

            if (currentState != null)
            {
                currentState.UpdateLogic();
            }

            if (currentState != jump && currentState != dash && currentState != roll)
            {
                rigidBody.gravityScale = 3;
            }
            else
            {
                rigidBody.gravityScale = 1.5f;
            }
        }
        else
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Knockback"))
            {
                animator.Play("KnockBack");
            }
           
        }
        
               
    }

    protected override FSM_BaseState getInitialState()
    {
        return idle;
    }

   
    public bool isGrounded()
    {
        bool ret = false;
        
        RaycastHit2D rayHit = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.down, capsuleCollider.bounds.extents.y+groundDistanceDetection);
        Color rayColor;
        if(rayHit.collider != null)
        {
            ret = true;
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(capsuleCollider.bounds.center, Vector2.down * (capsuleCollider.bounds.extents.y + groundDistanceDetection),rayColor);
        return ret;
    }

    public grabb isWallGrabbed()
    {
        grabb ret = grabb.noGrabb;

        RaycastHit2D RrayHit = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.right, capsuleCollider.bounds.extents.x + wallDistanceDetection);
        RaycastHit2D LrayHit = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.left, capsuleCollider.bounds.extents.x + wallDistanceDetection);

        Vector2 top = new Vector2(capsuleCollider.bounds.center.x, (capsuleCollider.bounds.center.y + (capsuleCollider.bounds.size.y / 2)));

        RaycastHit2D TRrayHit = Physics2D.Raycast(top, Vector2.right, capsuleCollider.bounds.extents.x + wallDistanceDetection);
        RaycastHit2D TLrayHit = Physics2D.Raycast(top, Vector2.left, capsuleCollider.bounds.extents.x + wallDistanceDetection);

        Color rayColor;

        if (RrayHit.collider != null)
        {
            ret = grabb.rightGrabb;
            rayColor = Color.green;
        }
        else if(RrayHit.collider == null)
        {
            rayColor = Color.red;
        }
        if (LrayHit.collider != null)
        {
            ret = grabb.leftGrabb;
            rayColor = Color.green;
        }
        else if (LrayHit.collider == null)
        {
            rayColor = Color.red;
        }

        if (TRrayHit.collider != null)
        {
            ret = grabb.topRightGrabb;
            rayColor = Color.green;
        }
        else if (RrayHit.collider == null)
        {
            rayColor = Color.red;
        }
        if (TLrayHit.collider != null)
        {
            ret = grabb.topLeftGrabb;
            rayColor = Color.green;
        }
        else if (LrayHit.collider == null)
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(capsuleCollider.bounds.center, Vector2.right * (capsuleCollider.bounds.extents.x + wallDistanceDetection));
        Debug.DrawRay(capsuleCollider.bounds.center, Vector2.left * (capsuleCollider.bounds.extents.x + wallDistanceDetection));

        Debug.DrawRay(top, Vector2.right * (capsuleCollider.bounds.extents.x + wallDistanceDetection));
        Debug.DrawRay(top, Vector2.left * (capsuleCollider.bounds.extents.x + wallDistanceDetection));
        return ret;
    }

    public void setDirection()
    {
        

        //no inputs yet
        if (directionInput == 0 && lastDirectionInput == 0)
        {
            directionInput = lastDirectionInput = gameObject.transform.localScale.x;
        }

        //existing inputs
        if (directionInput > 0)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            lastDirectionInput = directionInput;
        }
        else if (directionInput < 0)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            lastDirectionInput = directionInput;
        }
        else if (directionInput == 0)
        {
            if (lastDirectionInput > 0)
            {
                directionInput = lastDirectionInput; //avoid dir = 0
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                directionInput = lastDirectionInput; //avoid dir = 0
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    public void setInvulnerability(bool condition)
    {
        capsuleCollider.enabled = !condition;
        gameObject.GetComponentInChildren<BoxCollider2D>().enabled = condition;
    }

    public FSM_BaseState getCurrState()
    {
        return currentState;
    }

    public void Die()
    {
        Alive = false;
        ChangeState(knockback);
    }

    public IEnumerator FreezeAnimation(float t)
    {  
        float originalSpeed = animator.speed;
        animator.speed = 0f;
        characterFreezed = true;
        yield return new WaitForSeconds(t);
        characterFreezed = false;
        animator.speed = originalSpeed;   
    }



    private void OnEnable()
    {
        inputAction_move = inputActions.Player.Move;
        inputAction_move.Enable();

        inputAction_jump = inputActions.Player.jump;
        inputAction_jump.Enable();

        inputAction_roll = inputActions.Player.roll;
        inputAction_roll.Enable();

        inputAction_dash = inputActions.Player.dash;
        inputAction_dash.Enable();

        inputAction_light_attack = inputActions.Player.light_attack;
        inputAction_light_attack.Enable();

        inputAction_heavy_attack = inputActions.Player.heavy_attack;
        inputAction_heavy_attack.Enable();
    }

    private void OnDisable()
    {
        inputAction_move.Disable();
        inputAction_jump.Disable();
        inputAction_roll.Disable();
        inputAction_dash.Disable();
        inputAction_light_attack.Disable();
        inputAction_heavy_attack.Disable();
    }
}

