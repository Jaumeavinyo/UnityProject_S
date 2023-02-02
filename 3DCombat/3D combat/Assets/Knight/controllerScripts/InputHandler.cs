using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputHandler : MonoBehaviour
{


    public InputAction inputAction_move;
    public AnimatorHandler animatorHandler;

    public Vector2 inputMoveDir;

    public float moveAmount;

    public float verticalInput;
    public float horizontalInput;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputMoveDir = inputAction_move.ReadValue<Vector2>();
        verticalInput = inputMoveDir.y;
        horizontalInput = inputMoveDir.x;
        animatorHandler = GetComponent<AnimatorHandler>();
        handleMovementInput();
    }

    private void handleMovementInput()
    {
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        
        animatorHandler.updateAnimatorValues(0, moveAmount);
    }

    private void OnEnable()
    {
        inputAction_move.Enable();
    }
    private void OnDisable()
    {
        inputAction_move.Disable();
    }

}
