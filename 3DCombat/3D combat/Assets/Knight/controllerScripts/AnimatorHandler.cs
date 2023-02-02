using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    Animator animator;

    int horizontal;
    int vertical;

    private void Awake()
    {
        animator    = GetComponent<Animator>();

        horizontal  = Animator.StringToHash("horizontal");
        vertical    = Animator.StringToHash("vertical");
    }
    public void updateAnimatorValues(float horizontalMovement,float verticalMovement) 
    {
        float roundHorizontal;
        float roundVertical;

        //______horizontal______
        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            roundHorizontal = 0.5f;
        }else if (horizontalMovement > 0.55f)
        {
            roundHorizontal = 1f;
        }else if (horizontalMovement < 0 && horizontalMovement >-0.55f)
        {
            roundHorizontal = -0.5f;
        }else if (horizontalMovement < -0.55f)
        {
            roundHorizontal = -1;
        }
        else
        {
            roundHorizontal = 0;
        }

        //______vertical______
        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            roundVertical = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            roundVertical = 1f;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            roundVertical = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            roundVertical = -1;
        }
        else
        {
            roundVertical = 0;
        }

        animator.SetFloat(horizontal, roundHorizontal,0.1f,Time.deltaTime);
        animator.SetFloat(vertical, roundVertical, 0.1f, Time.deltaTime);
    }
}
