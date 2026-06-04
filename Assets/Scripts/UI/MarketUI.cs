using TMPro;
using UnityEngine;

public class MarketUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text creditsText;
    [SerializeField] private TMP_Text cargoText;

    [Header("Goods List")]
    [SerializeField] private Transform goodsContainer;
    [SerializeField] private MarketItemRow marketItemRowPrefab;

    private Station currentStation;
    private PlayerWallet currentWallet;
    private PlayerCargoHold currentCargoHold;

    public void Show(Station station, PlayerWallet wallet, PlayerCargoHold cargoHold)
    {
        currentStation = station;
        currentWallet = wallet;
        currentCargoHold = cargoHold;

        if (root != null)
        {
            root.SetActive(true);
        }

        if (titleText != null)
        {
            titleText.text = station != null
                ? $"{station.StationName} Market"
                : "Station Market";
        }

        Refresh(wallet, cargoHold);
        BuildGoodsList();
    }

    public void Hide()
    {
        if (root != null)
        {
            root.SetActive(false);
        }
    }

    public void Refresh(PlayerWallet wallet, PlayerCargoHold cargoHold)
    {
        if (creditsText != null)
        {
            creditsText.text = wallet != null
                ? $"Credits: {wallet.Credits}"
                : "Credits: 0";
        }

        if (cargoText != null)
        {
            cargoText.text = cargoHold != null
                ? $"Cargo: {cargoHold.CurrentCargo}/{cargoHold.CargoCapacity}"
                : "Cargo: 0/0";
        }
    }

    private void BuildGoodsList()
    {
        ClearGoodsList();

        if (currentStation == null ||
            currentWallet == null ||
            currentCargoHold == null ||
            goodsContainer == null ||
            marketItemRowPrefab == null)
        {
            return;
        }

        StationMarket stationMarket = currentStation.GetComponent<StationMarket>();

        if (stationMarket == null)
        {
            Debug.LogWarning($"{currentStation.StationName} has no StationMarket component.");
            return;
        }

        foreach (CargoItemDefinition item in stationMarket.AvailableGoods)
        {
            MarketItemRow row = Instantiate(marketItemRowPrefab, goodsContainer);

            int buyPrice = stationMarket.GetBuyPrice(item);
            int sellPrice = stationMarket.GetSellPrice(item);
            int ownedQuantity = currentCargoHold.GetQuantity(item);

            row.Initialize(
                item,
                buyPrice,
                sellPrice,
                ownedQuantity,
                HandleBuyRequested,
                HandleSellRequested);
        }
    }

    private void ClearGoodsList()
    {
        if (goodsContainer == null)
        {
            return;
        }

        for (int i = goodsContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(goodsContainer.GetChild(i).gameObject);
        }
    }

    private void HandleBuyRequested(CargoItemDefinition item)
    {
        if (currentStation == null || currentWallet == null || currentCargoHold == null)
        {
            return;
        }

        StationMarket stationMarket = currentStation.GetComponent<StationMarket>();

        if (stationMarket == null)
        {
            return;
        }

        int price = stationMarket.GetBuyPrice(item);

        if (!currentWallet.CanAfford(price))
        {
            Debug.Log("Not enough credits.");
            return;
        }

        if (!currentCargoHold.AddCargo(item, 1))
        {
            Debug.Log("Cargo hold is full.");
            return;
        }

        currentWallet.SpendCredits(price);

        Refresh(currentWallet, currentCargoHold);
        BuildGoodsList();
    }

    private void HandleSellRequested(CargoItemDefinition item)
    {
        if (currentStation == null || currentWallet == null || currentCargoHold == null)
        {
            return;
        }

        StationMarket stationMarket = currentStation.GetComponent<StationMarket>();

        if (stationMarket == null)
        {
            return;
        }

        if (!currentCargoHold.RemoveCargo(item, 1))
        {
            Debug.Log("No cargo to sell.");
            return;
        }

        int sellPrice = stationMarket.GetSellPrice(item);
        currentWallet.AddCredits(sellPrice);

        Refresh(currentWallet, currentCargoHold);
        BuildGoodsList();
    }
}