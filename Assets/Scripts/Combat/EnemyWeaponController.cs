using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireCooldown = 1.25f;
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
        nextFireTime = Time.time + fireCooldown;
    }

    private void Fire()
    {
        Projectile projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.Initialize(transform.up, gameObject);
    }
}