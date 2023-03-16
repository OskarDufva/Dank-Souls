using System;
using UnityEngine;

#region Description
/* This script is responsible for handling player input and storing the input values in public variables
The public variables include horizontal and vertical movement input, camera mouse input and move amount
PlayerControls is used to define input actions and read input values using the Unity Input System
Vector2 variables are used to store the movement and camera input values, which are later used to update the public variables */
#endregion
public class InputHandler : MonoBehaviour
{
    public float horizontal, vertical, moveAmount, mouseX, mouseY;

    public bool b_Input;
    public bool rollFlag;
    public bool sprintFlag;
    public float rollInputTimer;

    PlayerControls inputActions;
    
    Vector2 movementInput;
    Vector2 cameraInput;

    private void Awake(){
        Cursor.visible = false;
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
        HandleRollInput(delta);
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

    private void HandleRollInput(float delta){
        b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;
        if(b_Input){
            rollInputTimer += delta;
            sprintFlag = true;
        }
        else{
            if (rollInputTimer > 0 && rollInputTimer < 0.5f){
                sprintFlag = false;
                rollFlag = true;
            }

            rollInputTimer = 0;
        }

        sprintFlag = b_Input && rollInputTimer >= 0.5f;
    }
}
