using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketItemRow : MonoBehaviour
{
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text buyPriceText;
    [SerializeField] private TMP_Text ownedText;
    [SerializeField] private Button buyButton;

    private CargoItemDefinition item;
    private Action<CargoItemDefinition> buyRequested;

    public void Initialize(
        CargoItemDefinition item,
        int buyPrice,
        int ownedQuantity,
        Action<CargoItemDefinition> onBuyRequested)
    {
        this.item = item;
        buyRequested = onBuyRequested;

        itemNameText.text = item != null ? item.ItemName : "Unknown";
        buyPriceText.text = $"Buy: {buyPrice}";
        ownedText.text = $"Owned: {ownedQuantity}";

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(HandleBuyClicked);
    }

    private void HandleBuyClicked()
    {
        buyRequested?.Invoke(item);
    }
}