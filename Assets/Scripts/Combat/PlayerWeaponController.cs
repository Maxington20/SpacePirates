using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private WeaponDefinition weaponDefinition;
    [SerializeField] private TargetingController targetingController;

    private float nextFireTime;

    private void Awake()
    {
        if (targetingController == null)
        {
            targetingController = GetComponent<TargetingController>();
        }
    }

    private void Update()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            return;
        }

        if (Time.time < nextFireTime)
        {
            return;
        }

        Fire();

        float cooldown = weaponDefinition != null ? weaponDefinition.FireCooldown : 0.25f;
        nextFireTime = Time.time + cooldown;
    }

    private void Fire()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogWarning("Projectile prefab or fire point is missing.");
            return;
        }

        Vector2 fireDirection = transform.up;

        if (targetingController != null && targetingController.CurrentTarget != null)
        {
            fireDirection = targetingController.CurrentTarget.transform.position - firePoint.position;
        }

        Projectile projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        projectile.Initialize(fireDirection, gameObject, weaponDefinition);
    }
}