using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int health = 100;
    private int healthMax;
    private void Awake() {
        healthMax = health;
    }

    public event EventHandler<Unit> OnDead;
    public event EventHandler OnDamaged;

    public void Damage(int damageAmount, Unit shooterUnit)
    {
        health -= damageAmount;
        if (health < 0)
        {
            health = 0;
        }
        OnDamaged?.Invoke(this, EventArgs.Empty);
        if (health == 0)
        {
            Die(shooterUnit);
        }
        Debug.Log(health);
    }

    private void Die(Unit shooterUnit)
    {
        OnDead?.Invoke(this, shooterUnit);
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }
}
