using System;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    [Header("Fallback Stats")]
    [SerializeField] private int fallbackMaxHull = 100;
    [SerializeField] private int fallbackMaxShield = 50;
    [SerializeField] private float fallbackShieldRegenRate = 5f;
    [SerializeField] private float fallbackShieldRechargeDelay = 5f;

    [Header("Effects")]
    [SerializeField] private GameObject destructionEffectPrefab;
    [SerializeField] private FloatingDamageText floatingDamageTextPrefab;

    public int CurrentHull { get; private set; }
    public int MaxHull { get; private set; }

    public float CurrentShield { get; private set; }
    public int MaxShield { get; private set; }

    public bool IsDestroyed { get; private set; }

    public event Action<int, int> HullChanged;
    public event Action<float, int> ShieldChanged;
    public event Action ShipDestroyed;

    private DamageFlash damageFlash;
    private float shieldRegenRate;
    private float shieldRechargeDelay;
    private float lastDamageTime;

    private void Awake()
    {
        damageFlash = GetComponent<DamageFlash>();

        ShipDefinitionHolder holder = GetComponent<ShipDefinitionHolder>();

        if (holder != null && holder.ShipDefinition != null)
        {
            MaxHull = holder.ShipDefinition.MaxHull;
            MaxShield = holder.ShipDefinition.MaxShield;
            shieldRegenRate = holder.ShipDefinition.ShieldRegenRate;
            shieldRechargeDelay = holder.ShipDefinition.ShieldRechargeDelay;
        }
        else
        {
            MaxHull = fallbackMaxHull;
            MaxShield = fallbackMaxShield;
            shieldRegenRate = fallbackShieldRegenRate;
            shieldRechargeDelay = fallbackShieldRechargeDelay;
        }

        CurrentHull = MaxHull;
        CurrentShield = MaxShield;
    }

    private void Start()
    {
        HullChanged?.Invoke(CurrentHull, MaxHull);
        ShieldChanged?.Invoke(CurrentShield, MaxShield);
    }

    private void Update()
    {
        RegenerateShield();
    }

    public void TakeDamage(int amount)
    {
        if (IsDestroyed || amount <= 0)
        {
            return;
        }

        lastDamageTime = Time.time;
        damageFlash?.Flash();

        int remainingDamage = amount;

        if (CurrentShield > 0f)
        {
            float shieldDamage = Mathf.Min(CurrentShield, remainingDamage);
            CurrentShield -= shieldDamage;
            remainingDamage -= Mathf.RoundToInt(shieldDamage);

            ShieldChanged?.Invoke(CurrentShield, MaxShield);
        }

        if (remainingDamage > 0)
        {
            CurrentHull = Mathf.Max(CurrentHull - remainingDamage, 0);
            HullChanged?.Invoke(CurrentHull, MaxHull);

            if (CurrentHull <= 0)
            {
                DestroyShip();
            }
        }

        SpawnFloatingDamageText(amount);
    }

    private void RegenerateShield()
    {
        if (IsDestroyed)
        {
            return;
        }

        if (MaxShield <= 0)
        {
            return;
        }

        if (CurrentShield >= MaxShield)
        {
            return;
        }

        if (Time.time < lastDamageTime + shieldRechargeDelay)
        {
            return;
        }

        CurrentShield = Mathf.Min(CurrentShield + shieldRegenRate * Time.deltaTime, MaxShield);
        ShieldChanged?.Invoke(CurrentShield, MaxShield);
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

        Destroy(gameObject);
    }
}