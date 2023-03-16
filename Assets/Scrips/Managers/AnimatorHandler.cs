using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    This script handles the animations for characters 
    and defines two variables to store the hash values for the "Vertical" and "Horizontal" parameters.
*/
public class AnimatorHandler : MonoBehaviour
{
    PlayerManager playerManager;
    public Animator anim;
    InputHandler inputHandler;
    PlayerLocomotion playerLocomotion;
    int vertical;
    int horizontal;
    public bool canRotate;

    // The Initialize() method gets the Animator component attached to the game object and assigns the hash values for the "Vertical" and "Horizontal" parameters.
    public void Initialize(){
        anim = GetComponent<Animator>();
        inputHandler = GetComponentInParent<InputHandler>();
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    // The method takes two float parameters representing the vertical and horizontal movement of the character.
    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting){
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

        if (isSprinting){
            v = 2;
            h = horizontalMovement;
        }

        anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    public void PlayerTargetAnimation(string targetAnim, bool isInteracting){
        anim.applyRootMotion = isInteracting;
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }

    public void OnAnimatorMove(){
        if (playerManager.isInteracting == false)
            return;
            
            float delta = Time.deltaTime;
            playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.rigidbody.velocity = velocity;
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
