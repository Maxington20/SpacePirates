using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireCooldown = 0.25f;

    private float nextFireTime;

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
        nextFireTime = Time.time + fireCooldown;
    }

    private void Fire()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogWarning("Projectile prefab or fire point is missing.");
            return;
        }

        Projectile projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.Initialize(transform.up, gameObject);
    }
}