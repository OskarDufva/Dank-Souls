using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth;

    public float Damage;
    public float Healing;

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ApplyHealing();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            DamageTaken();
        }
    }

    void ApplyHealing()
    {
        if (CurrentHealth < MaxHealth)
        {
            CurrentHealth = CurrentHealth + Healing;
        }
    }

    public void DamageTaken()
    {
        if (CurrentHealth > 0)
        {
            CurrentHealth = CurrentHealth - Damage;
        }
    }
}
