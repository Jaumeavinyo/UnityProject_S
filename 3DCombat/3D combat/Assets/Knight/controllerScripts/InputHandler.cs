using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputHandler : MonoBehaviour
{


    public InputAction inputAction_move;

    public Vector2 inputMoveDir;
    public float verticalInput;
    public float horizontalInput;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputMoveDir = inputAction_move.ReadValue<Vector2>().normalized;
        verticalInput = inputMoveDir.y;
        horizontalInput = inputMoveDir.x;
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
