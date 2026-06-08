using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float fallbackSpeed = 14f;
    [SerializeField] private int fallbackDamage = 10;
    [SerializeField] private float fallbackLifetime = 2f;

    private Vector2 direction;
    private GameObject owner;
    private WeaponDefinition weaponDefinition;
    private int damage;
    private float speed;

    public void Initialize(Vector2 fireDirection, GameObject projectileOwner, WeaponDefinition weaponDefinition = null)
    {
        direction = fireDirection.normalized;
        owner = projectileOwner;
        this.weaponDefinition = weaponDefinition;

        damage = weaponDefinition != null ? weaponDefinition.Damage : fallbackDamage;
        speed = weaponDefinition != null ? weaponDefinition.ProjectileSpeed : fallbackSpeed;

        float lifetime = weaponDefinition != null ? weaponDefinition.ProjectileLifetime : fallbackLifetime;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (owner != null && other.gameObject == owner)
        {
            return;
        }

        ShipHealth shipHealth = other.GetComponent<ShipHealth>();

        if (shipHealth == null)
        {
            return;
        }

        shipHealth.TakeDamage(damage, weaponDefinition);
        Destroy(gameObject);
    }
}