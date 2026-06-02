using System.Collections.Generic;
using UnityEngine;

public class TargetingController : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] private float targetingRange = 25f;
    [SerializeField] private KeyCode cycleTargetKey = KeyCode.Tab;

    public Targetable CurrentTarget { get; private set; }

    public event System.Action<Targetable> TargetChanged;

    private void Update()
    {
        if (Input.GetKeyDown(cycleTargetKey))
        {
            CycleTarget();
        }

        if (CurrentTarget != null && !IsValidTarget(CurrentTarget))
        {
            SetTarget(null);
        }
    }

    public void CycleTarget()
    {
        List<Targetable> validTargets = GetValidTargets();

        if (validTargets.Count == 0)
        {
            SetTarget(null);
            return;
        }

        int currentIndex = validTargets.IndexOf(CurrentTarget);
        int nextIndex = currentIndex + 1;

        if (nextIndex >= validTargets.Count)
        {
            nextIndex = 0;
        }

        SetTarget(validTargets[nextIndex]);
    }

    private List<Targetable> GetValidTargets()
    {
        Targetable[] allTargets = FindObjectsByType<Targetable>(FindObjectsSortMode.None);
        List<Targetable> validTargets = new List<Targetable>();

        foreach (Targetable targetable in allTargets)
        {
            if (!IsValidTarget(targetable))
            {
                continue;
            }

            validTargets.Add(targetable);
        }

        validTargets.Sort((a, b) =>
        {
            float distanceA = Vector2.Distance(transform.position, a.transform.position);
            float distanceB = Vector2.Distance(transform.position, b.transform.position);
            return distanceA.CompareTo(distanceB);
        });

        return validTargets;
    }

    private bool IsValidTarget(Targetable targetable)
    {
        if (targetable == null)
        {
            return false;
        }

        if (!targetable.IsTargetable)
        {
            return false;
        }

        if (targetable.gameObject == gameObject)
        {
            return false;
        }

        if (targetable.Health == null || targetable.Health.IsDestroyed)
        {
            return false;
        }

        float distance = Vector2.Distance(transform.position, targetable.transform.position);

        return distance <= targetingRange;
    }

    private void SetTarget(Targetable target)
    {
        CurrentTarget = target;
        TargetChanged?.Invoke(CurrentTarget);
    }
}