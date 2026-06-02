using System;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    [SerializeField] private int fallbackMaxHealth = 100;
    [SerializeField] private GameObject destructionEffectPrefab;
    [SerializeField] private FloatingDamageText floatingDamageTextPrefab;

    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public bool IsDestroyed { get; private set; }

    public event Action<int, int> HealthChanged;
    public event Action ShipDestroyed;

    private DamageFlash damageFlash;

    private void Awake()
    {
        damageFlash = GetComponent<DamageFlash>();

        ShipDefinitionHolder holder = GetComponent<ShipDefinitionHolder>();
        MaxHealth = holder != null && holder.ShipDefinition != null
            ? holder.ShipDefinition.MaxHull
            : fallbackMaxHealth;

        CurrentHealth = MaxHealth;
    }

    private void Start()
    {
        HealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (IsDestroyed) return;

        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);

        damageFlash?.Flash();
        SpawnFloatingDamageText(amount);
        HealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0)
        {
            DestroyShip();
        }
    }

    private void SpawnFloatingDamageText(int amount)
    {
        if (floatingDamageTextPrefab == null) return;

        Vector3 spawnPosition = transform.position + new Vector3(0f, 0.8f, 0f);
        FloatingDamageText damageText = Instantiate(floatingDamageTextPrefab, spawnPosition, Quaternion.identity);
        damageText.Initialize(amount);
    }

    private void DestroyShip()
    {
        if (IsDestroyed) return;

        IsDestroyed = true;
        ShipDestroyed?.Invoke();

        if (destructionEffectPrefab != null)
        {
            Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}