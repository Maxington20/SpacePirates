using UnityEngine;

public class BoardingController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TargetingController targetingController;
    [SerializeField] private PlayerWallet playerWallet;

    [Header("Boarding")]
    [SerializeField] private KeyCode boardingKey = KeyCode.B;
    [SerializeField] private float boardingRange = 2.5f;
    [SerializeField] private float targetHullThreshold = 0.25f;
    [SerializeField] private int successCreditReward = 150;
    [SerializeField] private int failureCrewLoss = 1;

    public bool CanBoardCurrentTarget => CanBoard(GetCurrentTarget(), out _);

    private ShipCrew playerCrew;

    private void Awake()
    {
        if (targetingController == null)
        {
            targetingController = GetComponent<TargetingController>();
        }

        if (playerWallet == null)
        {
            playerWallet = GetComponent<PlayerWallet>();
        }

        playerCrew = GetComponent<ShipCrew>();
    }

    private void Update()
    {
        if (PlayerState.IsDocked)
        {
            return;
        }

        if (Input.GetKeyDown(boardingKey))
        {
            TryBoardTarget();
        }
    }

    private Targetable GetCurrentTarget()
    {
        return targetingController != null ? targetingController.CurrentTarget : null;
    }

    private void TryBoardTarget()
    {
        Targetable target = GetCurrentTarget();

        if (!CanBoard(target, out string reason))
        {
            Debug.Log(reason);
            return;
        }

        ResolveBoarding(target);
    }

    private bool CanBoard(Targetable target, out string reason)
    {
        reason = string.Empty;

        if (target == null)
        {
            reason = "No target selected.";
            return false;
        }

        if (target.GetComponent<Station>() != null)
        {
            reason = "You cannot board a station.";
            return false;
        }

        ShipHealth targetHealth = target.Health;
        if (targetHealth == null || targetHealth.IsDestroyed)
        {
            reason = "Target cannot be boarded.";
            return false;
        }

        if (targetHealth.CurrentShield > 0.5f)
        {
            reason = "Target shields must be down before boarding.";
            return false;
        }

        float hullPercent = targetHealth.MaxHull > 0
            ? (float)targetHealth.CurrentHull / targetHealth.MaxHull
            : 1f;

        if (hullPercent > targetHullThreshold)
        {
            reason = "Target hull is still too strong to board.";
            return false;
        }

        float distance = Vector2.Distance(transform.position, target.transform.position);
        if (distance > boardingRange)
        {
            reason = "Too far away to board.";
            return false;
        }

        if (playerCrew == null || playerCrew.CurrentCrew <= 0)
        {
            reason = "You have no crew available for boarding.";
            return false;
        }

        return true;
    }

    private void ResolveBoarding(Targetable target)
    {
        ShipCrew targetCrew = target.GetComponent<ShipCrew>();
        ShipHealth targetHealth = target.Health;

        float targetHullPercent = targetHealth.MaxHull > 0
            ? (float)targetHealth.CurrentHull / targetHealth.MaxHull
            : 1f;

        float playerStrength = playerCrew != null ? playerCrew.CurrentCrew : 1f;
        float enemyStrength = targetCrew != null
            ? Mathf.Max(1f, targetCrew.CurrentCrew * targetHullPercent)
            : 1f;

        float successChance = playerStrength / (playerStrength + enemyStrength);

        if (Random.value <= successChance)
        {
            HandleBoardingSuccess(target);
        }
        else
        {
            HandleBoardingFailure();
        }
    }

    private void HandleBoardingSuccess(Targetable target)
    {
        if (playerWallet != null)
        {
            playerWallet.AddCredits(successCreditReward);
        }

        Debug.Log($"Boarding successful! Captured {target.DisplayName}. Recovered {successCreditReward} credits.");

        ShipHealth targetHealth = target.Health;
        if (targetHealth != null && !targetHealth.IsDestroyed)
        {
            targetHealth.TakeDamage(targetHealth.CurrentHull);
        }
    }

    private void HandleBoardingFailure()
    {
        if (playerCrew != null)
        {
            playerCrew.LoseCrew(failureCrewLoss);
        }

        Debug.Log($"Boarding failed! Lost {failureCrewLoss} crew.");
    }
}