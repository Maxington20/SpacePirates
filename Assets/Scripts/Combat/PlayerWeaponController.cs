using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("Projectile Prefabs")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private HomingMissile missilePrefab;

    [Header("Fire Points")]
    [SerializeField] private Transform firePoint;

    [Header("Input")]
    [SerializeField] private KeyCode primaryFireKey = KeyCode.Space;
    [SerializeField] private KeyCode secondaryFireKey = KeyCode.LeftControl;

    [Header("References")]
    [SerializeField] private TargetingController targetingController;

    private ShipLoadout shipLoadout;
    private ShipSystemDamage systemDamage;
    private CombatOrderController combatOrderController;

    private float nextPrimaryFireTime;
    private float nextSecondaryFireTime;

    private void Awake()
    {
        shipLoadout = GetComponent<ShipLoadout>();
        systemDamage = GetComponent<ShipSystemDamage>();
        combatOrderController = GetComponent<CombatOrderController>();

        if (targetingController == null)
        {
            targetingController = GetComponent<TargetingController>();
        }
    }

    private void Update()
    {
        if (PlayerState.IsDocked)
        {
            return;
        }

        if (systemDamage != null && systemDamage.WeaponsFailed)
        {
            return;
        }

        if (Input.GetKey(primaryFireKey))
        {
            TryFire(
                shipLoadout != null ? shipLoadout.PrimaryWeapon : null,
                ref nextPrimaryFireTime);
        }

        if (Input.GetKey(secondaryFireKey))
        {
            TryFire(
                shipLoadout != null ? shipLoadout.SecondaryWeapon : null,
                ref nextSecondaryFireTime);
        }
    }

    private void TryFire(WeaponDefinition weapon, ref float nextFireTime)
    {
        if (weapon == null)
        {
            return;
        }

        if (Time.time < nextFireTime)
        {
            return;
        }

        bool fired = Fire(weapon);

        if (!fired)
        {
            return;
        }

        float cooldown = weapon.FireCooldown;

        if (systemDamage != null && systemDamage.WeaponsDamaged)
        {
            cooldown *= 1.5f;
        }

        nextFireTime = Time.time + cooldown;
    }

    private bool Fire(WeaponDefinition weapon)
    {
        if (weapon == null)
        {
            return false;
        }

        float damageMultiplier = combatOrderController != null
            ? combatOrderController.WeaponDamageMultiplier
            : 1f;

        if (weapon.WeaponCategory == WeaponCategory.Missile)
        {
            return FireMissile(weapon, damageMultiplier);
        }

        return FireStandardProjectile(weapon, damageMultiplier);
    }

    private bool FireStandardProjectile(WeaponDefinition weapon, float damageMultiplier)
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogWarning("Projectile prefab or fire point is missing.");
            return false;
        }

        Projectile projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation);

        projectile.Initialize(
            transform.up,
            gameObject,
            weapon,
            damageMultiplier);

        return true;
    }

    private bool FireMissile(WeaponDefinition weapon, float damageMultiplier)
    {
        if (missilePrefab == null || firePoint == null)
        {
            Debug.LogWarning("Missile prefab or fire point is missing.");
            return false;
        }

        Targetable target = targetingController != null
            ? targetingController.CurrentTarget
            : null;

        if (target == null)
        {
            GameMessageUI.Instance?.ShowMessage("Missiles require a target.");
            return false;
        }

        HomingMissile missile = Instantiate(
            missilePrefab,
            firePoint.position,
            firePoint.rotation);

        missile.Initialize(
            target.transform,
            gameObject,
            weapon,
            damageMultiplier);

        return true;
    }
}