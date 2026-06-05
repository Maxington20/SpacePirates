using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;

    private ShipLoadout shipLoadout;
    private ShipSystemDamage systemDamage;
    private float nextFireTime;

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

        if (!Input.GetKey(KeyCode.Space))
        {
            return;
        }

        if (Time.time < nextFireTime)
        {
            return;
        }

        Fire();

        WeaponDefinition weapon = shipLoadout != null ? shipLoadout.PrimaryWeapon : null;
        float cooldown = weapon != null ? weapon.FireCooldown : 0.25f;

        if (systemDamage != null && systemDamage.WeaponsDamaged)
        {
            cooldown *= 1.75f;
        }

        nextFireTime = Time.time + cooldown;
    }

    private void Fire()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogWarning("Projectile prefab or fire point is missing.");
            return;
        }

        WeaponDefinition weapon = shipLoadout != null ? shipLoadout.PrimaryWeapon : null;

        Projectile projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.Initialize(transform.up, gameObject, weapon);
    }
}