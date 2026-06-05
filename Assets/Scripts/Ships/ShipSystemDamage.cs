using System;
using UnityEngine;

public class ShipSystemDamage : MonoBehaviour
{
    public ShipSystemState EnginesState { get; private set; } = ShipSystemState.Operational;
    public ShipSystemState WeaponsState { get; private set; } = ShipSystemState.Operational;
    public ShipSystemState ShieldsState { get; private set; } = ShipSystemState.Operational;

    public event Action SystemsChanged;

    public bool HasAnyDamage =>
        EnginesState != ShipSystemState.Operational ||
        WeaponsState != ShipSystemState.Operational ||
        ShieldsState != ShipSystemState.Operational;

    public bool EnginesDamaged => EnginesState != ShipSystemState.Operational;
    public bool WeaponsDamaged => WeaponsState != ShipSystemState.Operational;
    public bool ShieldsDamaged => ShieldsState != ShipSystemState.Operational;

    public bool EnginesFailed => EnginesState == ShipSystemState.Failed;
    public bool WeaponsFailed => WeaponsState == ShipSystemState.Failed;
    public bool ShieldsFailed => ShieldsState == ShipSystemState.Failed;

    public void DamageRandomSystem()
    {
        ShipSystemType[] candidates =
        {
            ShipSystemType.Engines,
            ShipSystemType.Weapons,
            ShipSystemType.Shields
        };

        for (int attempt = 0; attempt < 10; attempt++)
        {
            ShipSystemType selected = candidates[UnityEngine.Random.Range(0, candidates.Length)];

            if (TryWorsenSystem(selected))
            {
                return;
            }
        }
    }

    public ShipSystemState GetSystemState(ShipSystemType systemType)
    {
        return systemType switch
        {
            ShipSystemType.Engines => EnginesState,
            ShipSystemType.Weapons => WeaponsState,
            ShipSystemType.Shields => ShieldsState,
            _ => ShipSystemState.Operational
        };
    }

    private bool TryWorsenSystem(ShipSystemType systemType)
    {
        ShipSystemState currentState = GetSystemState(systemType);

        if (currentState == ShipSystemState.Failed)
        {
            return false;
        }

        ShipSystemState newState = currentState == ShipSystemState.Operational
            ? ShipSystemState.Damaged
            : ShipSystemState.Failed;

        SetSystemState(systemType, newState);

        if (CompareTag("Player"))
        {
            GameMessageUI.Instance?.ShowMessage($"{systemType} {newState}!");
        }

        return true;
    }

    private void SetSystemState(ShipSystemType systemType, ShipSystemState state)
    {
        switch (systemType)
        {
            case ShipSystemType.Engines:
                EnginesState = state;
                break;

            case ShipSystemType.Weapons:
                WeaponsState = state;
                break;

            case ShipSystemType.Shields:
                ShieldsState = state;
                break;
        }

        SystemsChanged?.Invoke();
    }
}