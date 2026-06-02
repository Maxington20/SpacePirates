using TMPro;
using UnityEngine;

public class StationUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text stationNameText;

    private void Awake()
    {
        Hide();
    }

    public void Show(Station station)
    {
        PlayerState.SetDocked(true);

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

        if (root != null)
        {
            root.SetActive(false);
        }
    }
}