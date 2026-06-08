using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Input")]
    [SerializeField] private KeyCode primaryFireKey = KeyCode.Space;
    [SerializeField] private KeyCode secondaryFireKey = KeyCode.LeftControl;

    private ShipLoadout shipLoadout;
    private ShipSystemDamage systemDamage;

    private float nextPrimaryFireTime;
    private float nextSecondaryFireTime;

    private void Awake()
    {
        shipLoadout = GetComponent<ShipLoadout>();
        systemDamage = GetComponent<ShipSystemDamage>();
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
            TryFire(shipLoadout != null ? shipLoadout.PrimaryWeapon : null, ref nextPrimaryFireTime);
        }

        if (Input.GetKey(secondaryFireKey))
        {
            TryFire(shipLoadout != null ? shipLoadout.SecondaryWeapon : null, ref nextSecondaryFireTime);
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

        Fire(weapon);

        float cooldown = weapon.FireCooldown;

        if (systemDamage != null && systemDamage.WeaponsDamaged)
        {
            cooldown *= 1.5f;
        }

        nextFireTime = Time.time + cooldown;
    }

    private void Fire(WeaponDefinition weapon)
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogWarning("Projectile prefab or fire point is missing.");
            return;
        }

        Projectile projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.Initialize(transform.up, gameObject, weapon);
    }
}