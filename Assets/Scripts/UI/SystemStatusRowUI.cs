using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SystemStatusRowUI : MonoBehaviour
{
    [SerializeField] private TMP_Text systemNameText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private Image statusIndicator;

    [Header("Colors")]
    [SerializeField] private Color operationalColor = Color.green;
    [SerializeField] private Color damagedColor = new Color(1f, 0.55f, 0f);
    [SerializeField] private Color failedColor = Color.red;

    public void SetStatus(string systemName, ShipSystemState state)
    {
        if (systemNameText != null)
        {
            systemNameText.text = systemName;
        }

        if (statusText != null)
        {
            statusText.text = state.ToString();
        }

        if (statusIndicator != null)
        {
            statusIndicator.color = GetColorForState(state);
        }
    }

    private Color GetColorForState(ShipSystemState state)
    {
        switch (state)
        {
            case ShipSystemState.Damaged:
                return damagedColor;

            case ShipSystemState.Failed:
                return failedColor;

            default:
                return operationalColor;
        }
    }
}