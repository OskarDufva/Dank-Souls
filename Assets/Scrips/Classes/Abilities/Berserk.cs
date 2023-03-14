using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserk : InputHandler
{
    //PlayerLocomotion _playerLocomotion;
    InputHandler _inputHandler;


    private void Start()
    {
        _inputHandler = GetComponent<InputHandler>();
    }

    private void Update()
    {
            Ability();
    }

    private void Ability()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            float moveSprint = _inputHandler.moveAmount;

            moveSprint = 9f;
            //_inputHandler.moveAmount = 7f;

            Debug.Log("Berserk");
        }
    }
}
