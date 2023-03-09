using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    This script handles the animations for characters 
    and defines two variables to store the hash values for the "Vertical" and "Horizontal" parameters.
*/
public class AnimatorHandler : MonoBehaviour
{
    public Animator anim;
    int vertical;
    int horizontal;
    public bool canRotate;

    // The Initialize() method gets the Animator component attached to the game object and assigns the hash values for the "Vertical" and "Horizontal" parameters.
    public void Initialize(){
        anim = GetComponent<Animator>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    // The method takes two float parameters representing the vertical and horizontal movement of the character.
    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement){
        #region Vertical
        float v = 0;

        if(verticalMovement > 0 && verticalMovement < 0.55f){
            v = 0.5f;
        }
        else if (verticalMovement > 0.55f){
            v = 1;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f){
            v = -0.5f;
        }
        else if (verticalMovement < -0.55f){
            v = -1;
        }
        else{
            v = 0;
        }
        #endregion

        #region Horizontal
        float h = 0;

        if(horizontalMovement > 0 && horizontalMovement < 0.55f){
            h = 0.5f;
        }
        else if (horizontalMovement > 0.55f){
            h = 1;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f){
            h = -0.5f;
        }
        else if (horizontalMovement < -0.55f){
            h = -1;
        }
        else{
            h = 0;
        }
        #endregion

        anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    // The method sets the canRotate variable to true, allowing the character to rotate.
    public void CanRotate(){
        canRotate = true;
    }

    // The method sets the canRotate variable to false, preventing the character from rotating.
    public void StopRotation(){
        canRotate = false;
    }
}
