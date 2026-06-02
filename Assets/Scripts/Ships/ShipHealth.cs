using System;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject destructionEffectPrefab;
    [SerializeField] private FloatingDamageText floatingDamageTextPrefab;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;
    public bool IsDestroyed { get; private set; }

    public event Action<int, int> HealthChanged;
    public event Action ShipDestroyed;

    private DamageFlash damageFlash;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        damageFlash = GetComponent<DamageFlash>();
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

        damageFlash?.Flash();
        SpawnFloatingDamageText(amount);

        HealthChanged?.Invoke(CurrentHealth, maxHealth);

        Debug.Log($"{gameObject.name} took {amount} damage. HP: {CurrentHealth}/{maxHealth}");

        if (CurrentHealth <= 0)
        {
            DestroyShip();
        }
    }

    private void SpawnFloatingDamageText(int amount)
    {
        if (floatingDamageTextPrefab == null)
        {
            return;
        }

        Vector3 spawnPosition = transform.position + new Vector3(0f, 0.8f, 0f);
        FloatingDamageText damageText = Instantiate(floatingDamageTextPrefab, spawnPosition, Quaternion.identity);
        damageText.Initialize(amount);
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