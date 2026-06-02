using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private WeaponDefinition weaponDefinition;
    [SerializeField] private float fallbackFireCooldown = 1.25f;
    [SerializeField] private float firingRange = 6f;

    [Header("Target")]
    [SerializeField] private Transform target;

    private float nextFireTime;

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
        if (target == null || projectilePrefab == null || firePoint == null)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > firingRange)
        {
            return;
        }

        if (Time.time < nextFireTime)
        {
            return;
        }

        Fire();

        float cooldown = weaponDefinition != null
            ? weaponDefinition.FireCooldown
            : fallbackFireCooldown;

        nextFireTime = Time.time + cooldown;
    }

    private void Fire()
    {
        Vector2 fireDirection = target.position - firePoint.position;

        Projectile projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        projectile.Initialize(fireDirection, gameObject, weaponDefinition);
    }
}