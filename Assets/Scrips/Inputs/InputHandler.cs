using System;
using UnityEngine;

    /* This script is responsible for handling player input and storing the input values in public variables
    The public variables include horizontal and vertical movement input, camera mouse input and move amount
    PlayerControls is used to define input actions and read input values using the Unity Input System
    Vector2 variables are used to store the movement and camera input values, which are later used to update the public variables */

public class InputHandler : MonoBehaviour
{
    public float horizontal, vertical, moveAmount, mouseX, mouseY;

    PlayerControls inputActions;
    CameraHandler cameraHandler;
    Vector2 movementInput;
    Vector2 cameraInput;

    private void Awake(){
        cameraHandler = CameraHandler.singleton;
        Cursor.visible = false;
    }

    private void FixedUpdate(){
        float delta = Time.deltaTime;

        if(cameraHandler != null){
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
        }
    }

    // Sets up the PlayerControls input actions and enables them for use
    public void OnEnable(){
        if (inputActions == null){
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
        }

        inputActions.Enable();
    }   

    // Disables the input actions when this script is disabled or destroyed
    private void OnDisable(){
        inputActions.Disable();
    }

    // TickInput() is called from the PlayerManager to handle the movement input
    public void TickInput(float delta){
        MoveInput(delta);
    }

    /* MoveInput() updates the public variables with the movement and camera input values 
    Mathf.Clamp01() is used to ensure that the move amount stays within the range of 0 and 1 */
    private void MoveInput(float delta){
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }
}