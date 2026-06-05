using System;
using UnityEngine;

public class ShipSystemDamage : MonoBehaviour
{
    public bool EnginesDamaged { get; private set; }
    public bool WeaponsDamaged { get; private set; }
    public bool ShieldsDamaged { get; private set; }

    public event Action SystemsChanged;

    public bool HasAnyDamage =>
        EnginesDamaged ||
        WeaponsDamaged ||
        ShieldsDamaged;

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
            ShipSystemType selected =
                candidates[UnityEngine.Random.Range(0, candidates.Length)];

            if (TryDamageSystem(selected))
            {
                return;
            }
        }
    }

    private bool TryDamageSystem(ShipSystemType system)
    {
        switch (system)
        {
            case ShipSystemType.Engines:

                if (EnginesDamaged)
                {
                    return false;
                }

                EnginesDamaged = true;
                break;

            case ShipSystemType.Weapons:

                if (WeaponsDamaged)
                {
                    return false;
                }

                WeaponsDamaged = true;
                break;

            case ShipSystemType.Shields:

                if (ShieldsDamaged)
                {
                    return false;
                }

                ShieldsDamaged = true;
                break;

            default:
                return false;
        }

        SystemsChanged?.Invoke();

        if (CompareTag("Player"))
        {
            GameMessageUI.Instance?.ShowMessage($"{system} damaged!");
        }

        return true;
    }
}