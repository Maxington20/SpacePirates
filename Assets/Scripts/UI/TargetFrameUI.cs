using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetFrameUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TargetingController targetingController;
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text nameText;
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
            currentTarget.Health.HealthChanged += HandleTargetHealthChanged;
            currentTarget.Health.ShipDestroyed += HandleTargetDestroyed;
        }

        Refresh(currentTarget);
    }

    private void HandleTargetHealthChanged(int currentHealth, int maxHealth)
    {
        UpdateHealthBar(currentHealth, maxHealth);
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
            UpdateHealthBar(target.Health.CurrentHealth, target.Health.MaxHealth);
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (hullFillImage == null || maxHealth <= 0)
        {
            return;
        }

        hullFillImage.fillAmount = (float)currentHealth / maxHealth;
    }

    private void UnsubscribeFromCurrentTarget()
    {
        if (currentTarget == null || currentTarget.Health == null)
        {
            return;
        }

        currentTarget.Health.HealthChanged -= HandleTargetHealthChanged;
        currentTarget.Health.ShipDestroyed -= HandleTargetDestroyed;
    }
}