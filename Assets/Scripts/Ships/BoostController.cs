using System;
using UnityEngine;

public class BoostController : MonoBehaviour
{
    [Header("Boost")]
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float drainPerSecond = 45f;
    [SerializeField] private float regenPerSecond = 18f;
    [SerializeField] private float rechargeDelay = 1.25f;
    [SerializeField] private float boostSpeedMultiplier = 1.8f;

    public float CurrentEnergy { get; private set; }
    public float MaxEnergy => maxEnergy;
    public float BoostSpeedMultiplier => boostSpeedMultiplier;
    public bool IsBoosting { get; private set; }

    public event Action<float, float> EnergyChanged;

    private float lastBoostUseTime;
    private CombatOrderController combatOrderController;

    private void Awake()
    {
        combatOrderController = GetComponent<CombatOrderController>();

        CurrentEnergy = maxEnergy;
        EnergyChanged?.Invoke(CurrentEnergy, maxEnergy);
    }

    private void Update()
    {
        if (IsBoosting)
        {
            DrainBoostEnergy();
            return;
        }

        RegenerateEnergy();
    }

    public void SetBoosting(bool boosting)
    {
        if (boosting && CurrentEnergy <= 0f)
        {
            IsBoosting = false;
            return;
        }

        IsBoosting = boosting;
    }

    private void DrainBoostEnergy()
    {
        float drainMultiplier = combatOrderController != null
            ? combatOrderController.BoostDrainMultiplier
            : 1f;

        CurrentEnergy = Mathf.Max(CurrentEnergy - drainPerSecond * drainMultiplier * Time.deltaTime, 0f);
        lastBoostUseTime = Time.time;
        EnergyChanged?.Invoke(CurrentEnergy, maxEnergy);

        if (CurrentEnergy <= 0f)
        {
            IsBoosting = false;
        }
    }

    private void RegenerateEnergy()
    {
        if (CurrentEnergy >= maxEnergy)
        {
            return;
        }

        if (Time.time < lastBoostUseTime + rechargeDelay)
        {
            return;
        }

        CurrentEnergy = Mathf.Min(CurrentEnergy + regenPerSecond * Time.deltaTime, maxEnergy);
        EnergyChanged?.Invoke(CurrentEnergy, maxEnergy);
    }
}