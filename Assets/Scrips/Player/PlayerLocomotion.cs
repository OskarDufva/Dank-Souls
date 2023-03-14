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

        targetDir.Normalize();
        targetDir.y = 0;

        if(targetDir == Vector3.zero)
            targetDir = myTransform.forward;

        float rs = rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);
        myTransform.rotation = targetRotation;
    }

    public void HandleMovement(float delta){

        if (inputHandler.rollFlag)
            return;

        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = movementSpeed;

        if (inputHandler.sprintFlag){
            animatorHandler.PlayerTargetAnimation("Sprint", true);
            speed = sprintSpeed;
            playerManager.isSprinting = true;
            moveDirection *= speed;
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