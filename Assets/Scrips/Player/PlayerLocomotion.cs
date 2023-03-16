using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Description
/**********************************************************************************************************************************************************************************************************\
/ This script handles the player's movement and rotation.                                                                                                                                                  \
/ It retrieves the player's input through the InputHandler script, calculates the movement direction based on the camera's forward and right vectors, and applies movement to the player using a Rigidbody.\ 
/ It also handles the player's rotation using the HandleRotation method, which uses the player's input to calculate the target direction and smoothly rotates the player towards it using Quaternion.Slerp.\
/ Finally, it updates the animator values for the player's movement based on the inputHandler.moveAmount value.                                                                                            \
/ The animatorHandler.canRotate variable is used to determine if the player can rotate while moving.                                                                                                       \
/**********************************************************************************************************************************************************************************************************/
#endregion
public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    Transform cameraObject;
    InputHandler inputHandler;
    Vector3 moveDirection;

    [HideInInspector]
    public Transform myTransform;
    [HideInInspector]
    public AnimatorHandler animatorHandler;
    public new Rigidbody rigidbody;
    public GameObject normalCamera;

    [Header("Stats")]
    [SerializeField]
    float movementSpeed = 5;
    [SerializeField]
    float sprintSpeed = 7;
    [SerializeField]
    float rotationSpeed = 10;

    


    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        cameraObject = Camera.main.transform;
        myTransform = transform;    
        animatorHandler.Initialize();
    }


    #region Movement
    Vector3 normalVector;
    Vector3 targetPosition;

    private void HandleRotation(float delta){
        Vector3 targetDir = Vector3.zero; 
        float moveOverride = inputHandler.moveAmount;

        targetDir = cameraObject.forward * inputHandler.vertical;
        targetDir += cameraObject.right * inputHandler.horizontal;

        targetDir.Normalize(); // Normalize is called on targetDir to make sure its length is equal to 1
        targetDir.y = 0;  // Sets targetDir.y to 0 to ensure that the player does not tilt upwards or downwards when rotating.

        if(targetDir == Vector3.zero)
            targetDir = myTransform.forward; // targetDir is equal to 0,0,0 which means there is no movement input, the players current forward direction is used as the target direction.

        float rs = rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir); // Sets the rotation to face the target direction
        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta); // Interpolates between the current rotation and the target rotation based on rs value and the delta time. 
        myTransform.rotation = targetRotation; // Player transform rotation is set to new targetRotation.
    }

    // Handles player movement based on user input.
    public void HandleMovement(float delta){    
        if (inputHandler.rollFlag) // if the rollFlag is true in the inputHandler, it returns and does not execute any further movement code.
            return;

        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize(); 
        moveDirection.y = 0; // Sets y too 0 to ensure the player moves only on the horizontal plane.

        float speed = movementSpeed;

        if (inputHandler.sprintFlag){ // checks the sprintFlag in inputHandler.
            speed = sprintSpeed; // sets the speed to sprintSpeed instead.
            playerManager.isSprinting = true;
            moveDirection *= speed; // moveDirection is multiplied with speed.
        }
        else
        {
            moveDirection *= speed;
        }

        moveDirection *= speed;

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;

        animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);

        if(animatorHandler.canRotate){
            HandleRotation(delta);
        }
    }

    public void HandleRollingAndSprinting(float delta){
        if(animatorHandler.anim.GetBool("isInteracting"))
            return;

        if(inputHandler.rollFlag){
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;

            if(inputHandler.moveAmount > 0){
                animatorHandler.PlayerTargetAnimation("Rolling", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
            }
            else{
                animatorHandler.PlayerTargetAnimation("BackStep", true);
            }
        }
    }
    #endregion
}