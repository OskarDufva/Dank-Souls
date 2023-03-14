using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dash : Ability
{
    public float DashVelocity;

    public override void Activate(GameObject parent)
    {
        PlayerLocomotion movement = parent.GetComponent<PlayerLocomotion>();
        Rigidbody rigidbody = parent.GetComponent<Rigidbody>();

        //rigidbody.velocity = movement.HandleMovement(2) * DashVelocity;
    }
}
