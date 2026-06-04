using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    [SerializeField] private ShipHealth playerHealth;
    [SerializeField] private TMP_Text shipNameText;
    [SerializeField] private Image shieldFillImage;
    [SerializeField] private Image hullFillImage;

    private void Start()
    {
        if (playerHealth == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<ShipHealth>();
            }
        }

        if (playerHealth == null)
        {
            return;
        }

        playerHealth.ShieldChanged += HandleShieldChanged;
        playerHealth.HullChanged += HandleHullChanged;

        ShipDefinitionHolder holder = playerHealth.GetComponent<ShipDefinitionHolder>();
        if (holder != null && holder.ShipDefinition != null && shipNameText != null)
        {
            shipNameText.text = holder.ShipDefinition.ShipName;
        }

        HandleShieldChanged(playerHealth.CurrentShield, playerHealth.MaxShield);
        HandleHullChanged(playerHealth.CurrentHull, playerHealth.MaxHull);
    }

    private void OnDestroy()
    {
        if (playerHealth == null)
        {
            return;
        }

        playerHealth.ShieldChanged -= HandleShieldChanged;
        playerHealth.HullChanged -= HandleHullChanged;
    }

    private void HandleShieldChanged(float currentShield, int maxShield)
    {
        if (shieldFillImage == null || maxShield <= 0)
        {
            return;
        }

        shieldFillImage.fillAmount = currentShield / maxShield;
    }

    private void HandleHullChanged(int currentHull, int maxHull)
    {
        if (hullFillImage == null || maxHull <= 0)
        {
            return;
        }

        hullFillImage.fillAmount = (float)currentHull / maxHull;
    }
}