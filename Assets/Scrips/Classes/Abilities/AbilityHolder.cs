using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{

    public Ability Ability;
    float CooldownTime;
    float ActiveTime;

    enum AbilityState
    {
        ready,
        active,
        cooldown
    }

    AbilityState state = AbilityState.ready;

    public KeyCode Key;

    private void Update()
    {
        switch (state)
        {
            case AbilityState.ready:
                if (Input.GetKeyDown(Key))
                {
                    Ability.Activate(gameObject);
                    state = AbilityState.active;
                    ActiveTime = Ability.ActiveTime;
                }
                break;
            case AbilityState.active:
                if (ActiveTime > 0)
                {
                    ActiveTime -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.cooldown;
                    CooldownTime = Ability.CooldownTime;
                }
                break;
            case AbilityState.cooldown:
                if (CooldownTime > 0)
                {
                    CooldownTime -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.ready;
                }
                break;
                   
        }
    }
}
