using System.Collections.Generic;
using UnityEngine;

public class PlayerCargoHold : MonoBehaviour
{
    private readonly Dictionary<CargoItemDefinition, int> cargo =
        new Dictionary<CargoItemDefinition, int>();

    private int cargoCapacity;

    public int CargoCapacity => cargoCapacity;
    public int CurrentCargo => GetTotalCargo();

    private void Awake()
    {
        ShipDefinitionHolder shipDefinitionHolder =
            GetComponent<ShipDefinitionHolder>();

        if (shipDefinitionHolder != null &&
            shipDefinitionHolder.ShipDefinition != null)
        {
            cargoCapacity =
                shipDefinitionHolder.ShipDefinition.CargoCapacity;
        }
        else
        {
            cargoCapacity = 10;
        }
    }

    public int GetQuantity(CargoItemDefinition item)
    {
        if (item == null)
        {
            return 0;
        }

        return cargo.TryGetValue(item, out int quantity)
            ? quantity
            : 0;
    }

    public bool AddCargo(CargoItemDefinition item, int amount)
    {
        if (item == null || amount <= 0)
        {
            return false;
        }

        if (CurrentCargo + amount > CargoCapacity)
        {
            return false;
        }

        if (!cargo.ContainsKey(item))
        {
            cargo[item] = 0;
        }

        cargo[item] += amount;

        return true;
    }

    public bool RemoveCargo(CargoItemDefinition item, int amount)
    {
        if (item == null || amount <= 0)
        {
            return false;
        }

        if (GetQuantity(item) < amount)
        {
            return false;
        }

        cargo[item] -= amount;

        if (cargo[item] <= 0)
        {
            cargo.Remove(item);
        }

        return true;
    }

    private int GetTotalCargo()
    {
        int total = 0;

        foreach (var cargoEntry in cargo)
        {
            total += cargoEntry.Value;
        }

        return total;
    }
}