using System;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject destructionEffectPrefab;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;
    public bool IsDestroyed { get; private set; }

    public event Action<int, int> HealthChanged;
    public event Action ShipDestroyed;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    private void Start()
    {
        HealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (IsDestroyed)
        {
            return;
        }

        CurrentHealth -= amount;
        CurrentHealth = Mathf.Max(CurrentHealth, 0);

        HealthChanged?.Invoke(CurrentHealth, maxHealth);

        Debug.Log($"{gameObject.name} took {amount} damage. HP: {CurrentHealth}/{maxHealth}");

        if (CurrentHealth <= 0)
        {
            DestroyShip();
        }
    }

    private void DestroyShip()
    {
        if (IsDestroyed)
        {
            return;
        }

        IsDestroyed = true;
        ShipDestroyed?.Invoke();

        if (destructionEffectPrefab != null)
        {
            Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity);
        }

        Debug.Log($"{gameObject.name} destroyed.");

        Destroy(gameObject);
    }
}