using TMPro;
using UnityEngine;

public class MarketUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text creditsText;
    [SerializeField] private TMP_Text cargoText;

    public void Show(Station station, PlayerWallet wallet, PlayerCargoHold cargoHold)
    {
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
}