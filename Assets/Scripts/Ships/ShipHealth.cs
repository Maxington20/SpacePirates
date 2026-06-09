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
    private CombatOrderController combatOrderController;

    private float shieldRegenRate;
    private float shieldRechargeDelay;
    private float lastDamageTime;

    private void Awake()
    {
        damageFlash = GetComponent<DamageFlash>();
        shipCrew = GetComponent<ShipCrew>();
        systemDamage = GetComponent<ShipSystemDamage>();
        combatOrderController = GetComponent<CombatOrderController>();

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
        TakeDamage(amount, null);
    }

    public void TakeDamage(int amount, WeaponDefinition weaponDefinition)
    {
        if (IsDestroyed || amount <= 0)
        {
            return;
        }

        float shieldMultiplier = weaponDefinition != null ? weaponDefinition.ShieldDamageMultiplier : 1f;
        float hullMultiplier = weaponDefinition != null ? weaponDefinition.HullDamageMultiplier : 1f;
        float crewMultiplier = weaponDefinition != null ? weaponDefinition.CrewDamageMultiplier : 1f;
        float systemMultiplier = weaponDefinition != null ? weaponDefinition.SystemDamageMultiplier : 1f;

        lastDamageTime = Time.time;
        damageFlash?.Flash();

        int hullDamageTaken = 0;
        int displayedDamage = 0;

        if (CurrentShield > 0f)
        {
            float rawShieldDamage = amount * shieldMultiplier;
            float actualShieldDamage = Mathf.Min(CurrentShield, rawShieldDamage);

            CurrentShield -= actualShieldDamage;
            displayedDamage += Mathf.RoundToInt(actualShieldDamage);
            ShieldChanged?.Invoke(CurrentShield, MaxShield);

            float remainingDamageRatio = rawShieldDamage > 0f
                ? 1f - (actualShieldDamage / rawShieldDamage)
                : 0f;

            int hullDamage = Mathf.RoundToInt(amount * remainingDamageRatio * hullMultiplier);

            if (hullDamage > 0)
            {
                hullDamageTaken = ApplyHullDamage(hullDamage);
                displayedDamage += hullDamageTaken;
            }
        }
        else
        {
            int hullDamage = Mathf.RoundToInt(amount * hullMultiplier);
            hullDamageTaken = ApplyHullDamage(hullDamage);
            displayedDamage += hullDamageTaken;
        }

        if (hullDamageTaken > 0)
        {
            TryDamageCrewFromHullDamage(hullDamageTaken, crewMultiplier);
            TryDamageShipSystem(hullDamageTaken, systemMultiplier);
        }

        SpawnFloatingDamageText(Mathf.Max(displayedDamage, 1));

        if (CurrentHull <= 0)
        {
            DestroyShip();
        }
    }

    private int ApplyHullDamage(int hullDamage)
    {
        if (hullDamage <= 0)
        {
            return 0;
        }

        int previousHull = CurrentHull;
        CurrentHull = Mathf.Max(CurrentHull - hullDamage, 0);

        int hullDamageTaken = previousHull - CurrentHull;
        HullChanged?.Invoke(CurrentHull, MaxHull);

        return hullDamageTaken;
    }

    private void TryDamageCrewFromHullDamage(int hullDamageTaken, float crewDamageMultiplier)
    {
        if (!crewCanBeDamaged || shipCrew == null || hullDamageTaken <= 0)
        {
            return;
        }

        float crewLossChance = Mathf.Clamp01((hullDamageTaken * crewDamageMultiplier) / hullDamagePerCrewLossChance);

        if (UnityEngine.Random.value <= crewLossChance)
        {
            shipCrew.LoseCrew(1);

            if (CompareTag("Player"))
            {
                GameMessageUI.Instance?.ShowMessage("Crew casualty! Lost 1 crew.");
            }
        }
    }

    private void TryDamageShipSystem(int hullDamageTaken, float systemDamageMultiplier)
    {
        if (!systemsCanBeDamaged || systemDamage == null || hullDamageTaken <= 0)
        {
            return;
        }

        if (systemDamage.EnginesFailed &&
            systemDamage.WeaponsFailed &&
            systemDamage.ShieldsFailed)
        {
            return;
        }

        float damageChance = Mathf.Clamp01((hullDamageTaken * systemDamageMultiplier) / hullDamageForSystemDamageChance);

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

        if (systemDamage != null && systemDamage.ShieldsFailed)
        {
            return;
        }

        float effectiveRegenRate = shieldRegenRate;

        if (systemDamage != null && systemDamage.ShieldsDamaged)
        {
            effectiveRegenRate *= 0.4f;
        }

        if (combatOrderController != null)
        {
            effectiveRegenRate *= combatOrderController.ShieldRegenMultiplier;
        }

        if (Time.time < lastDamageTime + shieldRechargeDelay)
        {
            return;
        }

        CurrentShield = Mathf.Min(CurrentShield + effectiveRegenRate * Time.deltaTime, MaxShield);
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