using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [Header("Projectile Prefabs")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private HomingMissile missilePrefab;

    [Header("Fire Points")]
    [SerializeField] private Transform firePoint;

    [Header("Fallbacks")]
    [SerializeField] private float fallbackFireCooldown = 1.25f;
    [SerializeField] private float firingRange = 8f;

    [Header("Target")]
    [SerializeField] private Transform target;

    private ShipLoadout shipLoadout;
    private ShipSystemDamage systemDamage;

    private float nextPrimaryFireTime;

    private void Awake()
    {
        shipLoadout = GetComponent<ShipLoadout>();
        systemDamage = GetComponent<ShipSystemDamage>();
    }

    private void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                target = player.transform;
            }
        }
    }

    private void Update()
    {
        if (target == null || firePoint == null)
        {
            return;
        }

        if (systemDamage != null && systemDamage.WeaponsFailed)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > firingRange)
        {
            return;
        }

        WeaponDefinition weapon = shipLoadout != null
            ? shipLoadout.PrimaryWeapon
            : null;

        if (weapon == null)
        {
            return;
        }

        if (Time.time < nextPrimaryFireTime)
        {
            return;
        }

        bool fired = Fire(weapon);

        if (!fired)
        {
            return;
        }

        float cooldown = weapon != null
            ? weapon.FireCooldown
            : fallbackFireCooldown;

        if (systemDamage != null && systemDamage.WeaponsDamaged)
        {
            cooldown *= 1.5f;
        }

        nextPrimaryFireTime = Time.time + cooldown;
    }

    private bool Fire(WeaponDefinition weapon)
    {
        if (weapon.WeaponCategory == WeaponCategory.Missile)
        {
            return FireMissile(weapon);
        }

        return FireStandardProjectile(weapon);
    }

    private bool FireStandardProjectile(WeaponDefinition weapon)
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing projectile prefab.");
            return false;
        }

        Vector2 fireDirection = target.position - firePoint.position;

        Projectile projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation);

        projectile.Initialize(
            fireDirection,
            gameObject,
            weapon);

        return true;
    }

    private bool FireMissile(WeaponDefinition weapon)
    {
        if (missilePrefab == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing missile prefab.");
            return false;
        }

        HomingMissile missile = Instantiate(
            missilePrefab,
            firePoint.position,
            firePoint.rotation);

        missile.Initialize(
            target,
            gameObject,
            weapon,
            1f);

        return true;
    }
}