using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    [SerializeField] private ShipHealth playerHealth;
    [SerializeField] private BoostController boostController;

    [SerializeField] private TMP_Text shipNameText;
    [SerializeField] private Image shieldFillImage;
    [SerializeField] private Image hullFillImage;
    [SerializeField] private Image boostFillImage;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (playerHealth == null && player != null)
        {
            playerHealth = player.GetComponent<ShipHealth>();
        }

        if (boostController == null && player != null)
        {
            boostController = player.GetComponent<BoostController>();
        }

        if (playerHealth != null)
        {
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

        if (boostController != null)
        {
            boostController.EnergyChanged += HandleBoostChanged;
            HandleBoostChanged(boostController.CurrentEnergy, boostController.MaxEnergy);
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.ShieldChanged -= HandleShieldChanged;
            playerHealth.HullChanged -= HandleHullChanged;
        }

        if (boostController != null)
        {
            boostController.EnergyChanged -= HandleBoostChanged;
        }
    }

    private void HandleShieldChanged(float currentShield, int maxShield)
    {
        if (shieldFillImage != null && maxShield > 0)
        {
            shieldFillImage.fillAmount = currentShield / maxShield;
        }
    }

    private void HandleHullChanged(int currentHull, int maxHull)
    {
        if (hullFillImage != null && maxHull > 0)
        {
            hullFillImage.fillAmount = (float)currentHull / maxHull;
        }
    }

    private void HandleBoostChanged(float currentEnergy, float maxEnergy)
    {
        if (boostFillImage != null && maxEnergy > 0)
        {
            boostFillImage.fillAmount = currentEnergy / maxEnergy;
        }
    }
}