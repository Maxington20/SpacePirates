using System;
using UnityEngine;

public class CombatOrderController : MonoBehaviour
{
    [SerializeField] private CombatOrder currentOrder = CombatOrder.Balanced;

    public CombatOrder CurrentOrder => currentOrder;

    public event Action<CombatOrder> CombatOrderChanged;

    public float WeaponDamageMultiplier
    {
        get
        {
            return currentOrder switch
            {
                CombatOrder.Aggressive => 1.25f,
                CombatOrder.Evasive => 0.75f,
                CombatOrder.Pursuit => 0.75f,
                _ => 1f
            };
        }
    }

    public float ShieldRegenMultiplier
    {
        get
        {
            return currentOrder switch
            {
                CombatOrder.Aggressive => 0.75f,
                CombatOrder.Evasive => 1.5f,
                _ => 1f
            };
        }
    }

    public float SpeedMultiplier
    {
        get
        {
            return currentOrder switch
            {
                CombatOrder.Pursuit => 1.25f,
                _ => 1f
            };
        }
    }

    public float BoostDrainMultiplier
    {
        get
        {
            return currentOrder switch
            {
                CombatOrder.Pursuit => 0.5f,
                _ => 1f
            };
        }
    }

    private void Update()
    {
        if (PlayerState.IsDocked)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            SetOrder(CombatOrder.Balanced);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            SetOrder(CombatOrder.Aggressive);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            SetOrder(CombatOrder.Evasive);
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            SetOrder(CombatOrder.Pursuit);
        }
    }

    public void SetOrder(CombatOrder newOrder)
    {
        if (currentOrder == newOrder)
        {
            return;
        }

        currentOrder = newOrder;
        CombatOrderChanged?.Invoke(currentOrder);

        GameMessageUI.Instance?.ShowMessage($"Combat Order: {currentOrder}");
    }
}