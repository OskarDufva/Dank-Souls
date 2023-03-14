using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public new string Name;
    public float CooldownTime;
    public float ActiveTime;

    public virtual void Activate(GameObject parent)
    {

    }
}
