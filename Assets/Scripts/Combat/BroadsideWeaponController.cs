using System;
using UnityEngine;

public class BroadsideWeaponController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform[] portFirePoints;
    [SerializeField] private Transform[] starboardFirePoints;
    [SerializeField] private TargetingController targetingController;
    [SerializeField] private BroadsideArcController arcController;

    [Header("Input")]
    [SerializeField] private KeyCode fireKey = KeyCode.Space;

    [Header("Battery")]
    [SerializeField] private int maxCannonsPerSide = 4;
    [SerializeField] private float secondsPerCannonReload = 1.2f;
    [SerializeField] private float projectileSpreadDegrees = 4f;

    public int LoadedPortCannons { get; private set; }
    public int LoadedStarboardCannons { get; private set; }
    public int MaxCannonsPerSide => maxCannonsPerSide;

    public event Action<int, int> PortBatteryChanged;
    public event Action<int, int> StarboardBatteryChanged;

    private ShipLoadout shipLoadout;
    private CombatOrderController combatOrderController;

    private float portReloadTimer;
    private float starboardReloadTimer;

    private void Awake()
    {
        shipLoadout = GetComponent<ShipLoadout>();
        combatOrderController = GetComponent<CombatOrderController>();

        if (targetingController == null)
        {
            targetingController = GetComponent<TargetingController>();
        }

        if (arcController == null)
        {
            arcController = GetComponent<BroadsideArcController>();
        }

        LoadedPortCannons = maxCannonsPerSide;
        LoadedStarboardCannons = maxCannonsPerSide;
    }

    private void Start()
    {
        PortBatteryChanged?.Invoke(LoadedPortCannons, maxCannonsPerSide);
        StarboardBatteryChanged?.Invoke(LoadedStarboardCannons, maxCannonsPerSide);
    }

    private void Update()
    {
        if (PlayerState.IsDocked)
        {
            return;
        }

        ReloadCannons();

        if (Input.GetKeyDown(fireKey))
        {
            FireAvailableBroadsides();
        }
    }

    private void ReloadCannons()
    {
        ReloadPortSide();
        ReloadStarboardSide();
    }

    private void ReloadPortSide()
    {
        if (LoadedPortCannons >= maxCannonsPerSide)
        {
            portReloadTimer = 0f;
            return;
        }

        portReloadTimer += Time.deltaTime;

        while (portReloadTimer >= secondsPerCannonReload &&
               LoadedPortCannons < maxCannonsPerSide)
        {
            portReloadTimer -= secondsPerCannonReload;
            LoadedPortCannons++;
            PortBatteryChanged?.Invoke(LoadedPortCannons, maxCannonsPerSide);
        }
    }

    private void ReloadStarboardSide()
    {
        if (LoadedStarboardCannons >= maxCannonsPerSide)
        {
            starboardReloadTimer = 0f;
            return;
        }

        starboardReloadTimer += Time.deltaTime;

        while (starboardReloadTimer >= secondsPerCannonReload &&
               LoadedStarboardCannons < maxCannonsPerSide)
        {
            starboardReloadTimer -= secondsPerCannonReload;
            LoadedStarboardCannons++;
            StarboardBatteryChanged?.Invoke(LoadedStarboardCannons, maxCannonsPerSide);
        }
    }

    private void FireAvailableBroadsides()
    {
        Targetable target = targetingController != null
            ? targetingController.CurrentTarget
            : null;

        if (target == null)
        {
            GameMessageUI.Instance?.ShowMessage("No target selected.");
            return;
        }

        bool targetInPortArc = arcController != null &&
                               arcController.IsTargetInArc(target.transform, BroadsideSide.Port);

        bool targetInStarboardArc = arcController != null &&
                                    arcController.IsTargetInArc(target.transform, BroadsideSide.Starboard);

        if (!targetInPortArc && !targetInStarboardArc)
        {
            GameMessageUI.Instance?.ShowMessage("No broadside firing angle.");
            return;
        }

        bool firedAny = false;

        if (targetInPortArc)
        {
            firedAny |= TryFireSide(BroadsideSide.Port);
        }

        if (targetInStarboardArc)
        {
            firedAny |= TryFireSide(BroadsideSide.Starboard);
        }

        if (!firedAny)
        {
            GameMessageUI.Instance?.ShowMessage("No cannons loaded.");
        }
    }

    private bool TryFireSide(BroadsideSide side)
    {
        int loadedCannons = side == BroadsideSide.Port
            ? LoadedPortCannons
            : LoadedStarboardCannons;

        if (loadedCannons <= 0)
        {
            return false;
        }

        FireSide(side, loadedCannons);

        if (side == BroadsideSide.Port)
        {
            LoadedPortCannons = 0;
            PortBatteryChanged?.Invoke(LoadedPortCannons, maxCannonsPerSide);
        }
        else
        {
            LoadedStarboardCannons = 0;
            StarboardBatteryChanged?.Invoke(LoadedStarboardCannons, maxCannonsPerSide);
        }

        return true;
    }

    private void FireSide(BroadsideSide side, int cannonCount)
    {
        WeaponDefinition weapon = shipLoadout != null
            ? shipLoadout.PrimaryWeapon
            : null;

        if (weapon == null || projectilePrefab == null || arcController == null)
        {
            return;
        }

        Transform[] firePoints = side == BroadsideSide.Port
            ? portFirePoints
            : starboardFirePoints;

        if (firePoints == null || firePoints.Length == 0)
        {
            Debug.LogWarning($"{side} fire points are missing.");
            return;
        }

        Vector2 baseDirection = arcController.GetFireDirection(side);

        float damageMultiplier = combatOrderController != null
            ? combatOrderController.WeaponDamageMultiplier
            : 1f;

        int shotsToFire = Mathf.Min(cannonCount, firePoints.Length);

        for (int i = 0; i < shotsToFire; i++)
        {
            Transform firePoint = firePoints[i];

            if (firePoint == null)
            {
                continue;
            }

            float spread = UnityEngine.Random.Range(
                -projectileSpreadDegrees,
                projectileSpreadDegrees);

            Vector2 direction = Quaternion.Euler(0f, 0f, spread) * baseDirection;

            Projectile projectile = Instantiate(
                projectilePrefab,
                firePoint.position,
                Quaternion.identity);

            projectile.Initialize(
                direction,
                gameObject,
                weapon,
                damageMultiplier);
        }

        GameMessageUI.Instance?.ShowMessage($"{side} broadside fired: {shotsToFire} cannon(s)!");
    }
}