using UnityEngine;

public class TargetIndicatorController : MonoBehaviour
{
    [SerializeField] private TargetingController targetingController;
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private Vector3 worldOffset = Vector3.zero;

    private GameObject activeIndicator;
    private Targetable currentTarget;

    private void Start()
    {
        if (targetingController == null)
        {
            targetingController = FindFirstObjectByType<TargetingController>();
        }

        if (targetingController != null)
        {
            targetingController.TargetChanged += HandleTargetChanged;
        }
    }

    private void OnDestroy()
    {
        if (targetingController != null)
        {
            targetingController.TargetChanged -= HandleTargetChanged;
        }

        UnsubscribeFromCurrentTarget();
    }

    private void LateUpdate()
    {
        if (activeIndicator == null || currentTarget == null)
        {
            DestroyIndicator();
            return;
        }

        if (currentTarget.Health == null || currentTarget.Health.IsDestroyed)
        {
            DestroyIndicator();
            return;
        }

        activeIndicator.transform.position = currentTarget.transform.position + worldOffset;
    }

    private void HandleTargetChanged(Targetable target)
    {
        UnsubscribeFromCurrentTarget();
        DestroyIndicator();

        currentTarget = target;

        if (currentTarget == null || indicatorPrefab == null)
        {
            return;
        }

        if (currentTarget.Health != null)
        {
            currentTarget.Health.ShipDestroyed += HandleTargetDestroyed;
        }

        activeIndicator = Instantiate(
            indicatorPrefab,
            currentTarget.transform.position + worldOffset,
            Quaternion.identity);
    }

    private void HandleTargetDestroyed()
    {
        DestroyIndicator();
        currentTarget = null;
    }

    private void DestroyIndicator()
    {
        if (activeIndicator != null)
        {
            Destroy(activeIndicator);
            activeIndicator = null;
        }
    }

    private void UnsubscribeFromCurrentTarget()
    {
        if (currentTarget == null || currentTarget.Health == null)
        {
            return;
        }

        currentTarget.Health.ShipDestroyed -= HandleTargetDestroyed;
    }
}