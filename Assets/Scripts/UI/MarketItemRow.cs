using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketItemRow : MonoBehaviour
{
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text buyPriceText;
    [SerializeField] private TMP_Text sellPriceText;
    [SerializeField] private TMP_Text ownedText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button sellButton;

    private CargoItemDefinition item;
    private Action<CargoItemDefinition> buyRequested;
    private Action<CargoItemDefinition> sellRequested;

    public void Initialize(
        CargoItemDefinition item,
        int buyPrice,
        int sellPrice,
        int ownedQuantity,
        Action<CargoItemDefinition> onBuyRequested,
        Action<CargoItemDefinition> onSellRequested)
    {
        this.item = item;
        buyRequested = onBuyRequested;
        sellRequested = onSellRequested;

        itemNameText.text = item != null ? item.ItemName : "Unknown";
        buyPriceText.text = $"Buy: {buyPrice}";
        sellPriceText.text = $"Sell: {sellPrice}";
        ownedText.text = $"Owned: {ownedQuantity}";

        buyButton.interactable = item != null;
        sellButton.interactable = ownedQuantity > 0;

        buyButton.onClick.RemoveAllListeners();
        sellButton.onClick.RemoveAllListeners();

        buyButton.onClick.AddListener(HandleBuyClicked);
        sellButton.onClick.AddListener(HandleSellClicked);
    }

    private void HandleBuyClicked()
    {
        buyRequested?.Invoke(item);
    }

    private void HandleSellClicked()
    {
        sellRequested?.Invoke(item);
    }
}