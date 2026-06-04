using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetFrameUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TargetingController targetingController;
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image shieldFillImage;
    [SerializeField] private Image hullFillImage;

    private Targetable currentTarget;

    private void Start()
    {
        if (targetingController != null)
        {
            targetingController.TargetChanged += HandleTargetChanged;
        }

        Refresh(null);
    }

    private void OnDestroy()
    {
        if (targetingController != null)
        {
            targetingController.TargetChanged -= HandleTargetChanged;
        }

        UnsubscribeFromCurrentTarget();
    }

    private void HandleTargetChanged(Targetable target)
    {
        UnsubscribeFromCurrentTarget();

        currentTarget = target;

        if (currentTarget != null && currentTarget.Health != null)
        {
            currentTarget.Health.HullChanged += HandleTargetHullChanged;
            currentTarget.Health.ShieldChanged += HandleTargetShieldChanged;
            currentTarget.Health.ShipDestroyed += HandleTargetDestroyed;
        }

        Refresh(currentTarget);
    }

    private void HandleTargetHullChanged(int currentHull, int maxHull)
    {
        UpdateHullBar(currentHull, maxHull);
    }

    private void HandleTargetShieldChanged(float currentShield, int maxShield)
    {
        UpdateShieldBar(currentShield, maxShield);
    }

    private void HandleTargetDestroyed()
    {
        Refresh(null);
    }

    private void Refresh(Targetable target)
    {
        if (target == null)
        {
            if (root != null)
            {
                root.SetActive(false);
            }

            return;
        }

        if (root != null)
        {
            root.SetActive(true);
        }

        if (nameText != null)
        {
            nameText.text = target.DisplayName;
        }

        if (target.Health != null)
        {
            UpdateHullBar(target.Health.CurrentHull, target.Health.MaxHull);
            UpdateShieldBar(target.Health.CurrentShield, target.Health.MaxShield);
        }
    }

    private void UpdateHullBar(int currentHull, int maxHull)
    {
        if (hullFillImage == null || maxHull <= 0)
        {
            return;
        }

        hullFillImage.fillAmount = (float)currentHull / maxHull;
    }

    private void UpdateShieldBar(float currentShield, int maxShield)
    {
        if (shieldFillImage == null || maxShield <= 0)
        {
            return;
        }

        shieldFillImage.fillAmount = currentShield / maxShield;
    }

    private void UnsubscribeFromCurrentTarget()
    {
        if (currentTarget == null || currentTarget.Health == null)
        {
            return;
        }

        currentTarget.Health.HullChanged -= HandleTargetHullChanged;
        currentTarget.Health.ShieldChanged -= HandleTargetShieldChanged;
        currentTarget.Health.ShipDestroyed -= HandleTargetDestroyed;
    }
}