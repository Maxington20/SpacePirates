using TMPro;
using UnityEngine;

public class CurrentSystemDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text systemNameText;

    private void Start()
    {
        if (systemNameText == null)
        {
            return;
        }

        systemNameText.text = GameState.CurrentSystem != null
            ? GameState.CurrentSystem.SystemName
            : "Unknown System";
    }
}