using System.Collections.Generic;
using UnityEngine;

public class StationMarket : MonoBehaviour
{
    [SerializeField] private List<CargoItemDefinition> availableGoods = new List<CargoItemDefinition>();
    [SerializeField] private float priceMultiplier = 1f;

    public IReadOnlyList<CargoItemDefinition> AvailableGoods => availableGoods;

    public int GetBuyPrice(CargoItemDefinition item)
    {
        if (item == null)
        {
            return 0;
        }

        return Mathf.RoundToInt(item.BasePrice * priceMultiplier);
    }

    public int GetSellPrice(CargoItemDefinition item)
    {
        if (item == null)
        {
            return 0;
        }

        return Mathf.RoundToInt(item.BasePrice * priceMultiplier * 0.75f);
    }
}