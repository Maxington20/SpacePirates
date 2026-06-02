using TMPro;
using UnityEngine;

public class CurrentSystemDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text systemNameText;

    private void Start()
    {
        Refresh();
    }

    private void Refresh()
    {
        if (systemNameText == null)
        {
            return;
        }

        string systemName = GameState.CurrentSystem != null
            ? GameState.CurrentSystem.SystemName
            : "Unknown System";

        systemNameText.text = systemName;
    }
}