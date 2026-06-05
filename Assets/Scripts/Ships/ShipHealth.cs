using System;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    [Header("Fallback Stats")]
    [SerializeField] private int fallbackMaxHull = 100;
    [SerializeField] private int fallbackMaxShield = 50;
    [SerializeField] private float fallbackShieldRegenRate = 5f;
    [SerializeField] private float fallbackShieldRechargeDelay = 5f;

    [Header("Crew Damage")]
    [SerializeField] private bool crewCanBeDamaged = true;
    [SerializeField] private int hullDamagePerCrewLossChance = 50;

    [Header("System Damage")]
    [SerializeField] private bool systemsCanBeDamaged = true;
    [SerializeField] private float hullDamageForSystemDamageChance = 75f;

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
    private ShipCrew shipCrew;
    private ShipSystemDamage systemDamage;

    private float shieldRegenRate;
    private float shieldRechargeDelay;
    private float lastDamageTime;

    private void Awake()
    {
        damageFlash = GetComponent<DamageFlash>();
        shipCrew = GetComponent<ShipCrew>();
        systemDamage = GetComponent<ShipSystemDamage>();

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
        int hullDamageTaken = 0;

        if (CurrentShield > 0f)
        {
            float shieldDamage = Mathf.Min(CurrentShield, remainingDamage);
            CurrentShield -= shieldDamage;
            remainingDamage -= Mathf.RoundToInt(shieldDamage);

            ShieldChanged?.Invoke(CurrentShield, MaxShield);
        }

        if (remainingDamage > 0)
        {
            int previousHull = CurrentHull;

            CurrentHull = Mathf.Max(CurrentHull - remainingDamage, 0);
            hullDamageTaken = previousHull - CurrentHull;

            HullChanged?.Invoke(CurrentHull, MaxHull);

            TryDamageCrewFromHullDamage(hullDamageTaken);
            TryDamageShipSystem(hullDamageTaken);

            if (CurrentHull <= 0)
            {
                DestroyShip();
            }
        }

        SpawnFloatingDamageText(amount);
    }

    private void TryDamageCrewFromHullDamage(int hullDamageTaken)
    {
        if (!crewCanBeDamaged || shipCrew == null || hullDamageTaken <= 0)
        {
            return;
        }

        float crewLossChance = Mathf.Clamp01((float)hullDamageTaken / hullDamagePerCrewLossChance);

        if (UnityEngine.Random.value <= crewLossChance)
        {
            shipCrew.LoseCrew(1);

            if (GameMessageUI.Instance != null && gameObject.CompareTag("Player"))
            {
                GameMessageUI.Instance.ShowMessage("Crew casualty! Lost 1 crew.");
            }
        }
    }

    private void TryDamageShipSystem(int hullDamageTaken)
    {
        if (!systemsCanBeDamaged || systemDamage == null || hullDamageTaken <= 0)
        {
            return;
        }

        if (systemDamage.EnginesDamaged &&
            systemDamage.WeaponsDamaged &&
            systemDamage.ShieldsDamaged)
        {
            return;
        }

        float damageChance = Mathf.Clamp01(hullDamageTaken / hullDamageForSystemDamageChance);

        if (UnityEngine.Random.value <= damageChance)
        {
            systemDamage.DamageRandomSystem();
        }
    }

    private void RegenerateShield()
    {
        if (IsDestroyed || MaxShield <= 0 || CurrentShield >= MaxShield)
        {
            return;
        }

        if (systemDamage != null && systemDamage.ShieldsDamaged)
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