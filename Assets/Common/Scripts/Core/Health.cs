using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Tooltip("Maximum amount of health")] public int MaxHealth = 10;

    public event Action<int> OnDamaged;
    public event Action OnDie;

    public int CurrentHealth { get; set; }

    private bool _isDead;

    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        int healthBefore = CurrentHealth;
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        int trueDamageAmount = healthBefore - CurrentHealth;
        if (trueDamageAmount > 0)
        {
            OnDamaged?.Invoke(trueDamageAmount);
        }

        HandleDeath();
    }

    public void Kill()
    {
        CurrentHealth = 0;
        OnDamaged?.Invoke(MaxHealth);

        HandleDeath();
    }

    void HandleDeath()
    {
        if (_isDead)
            return;

        if (CurrentHealth > 0) return;
        _isDead = true;
        OnDie?.Invoke();
    }
}