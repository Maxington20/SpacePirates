using TMPro;
using UnityEngine;

public class StationUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text stationNameText;
    [SerializeField] private MarketUI marketUI;

    private Station currentStation;
    private PlayerWallet playerWallet;
    private PlayerCargoHold playerCargoHold;

    private void Awake()
    {
        Hide();
    }

    public void Show(Station station, PlayerWallet wallet, PlayerCargoHold cargoHold)
    {
        PlayerState.SetDocked(true);

        currentStation = station;
        playerWallet = wallet;
        playerCargoHold = cargoHold;

        if (root != null)
        {
            root.SetActive(true);
        }

        if (stationNameText != null)
        {
            stationNameText.text = station != null
                ? $"Welcome to {station.StationName}"
                : "Welcome to Station";
        }
    }

    public void Hide()
    {
        PlayerState.SetDocked(false);

        if (marketUI != null)
        {
            marketUI.Hide();
        }

        if (root != null)
        {
            root.SetActive(false);
        }
    }

    public void OpenMarket()
    {
        if (marketUI == null)
        {
            Debug.LogWarning("StationUI is missing MarketUI.");
            return;
        }

        marketUI.Show(currentStation, playerWallet, playerCargoHold);
    }
}