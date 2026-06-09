using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float fallbackSpeed = 14f;
    [SerializeField] private int fallbackDamage = 10;
    [SerializeField] private float fallbackLifetime = 2f;

    private Vector2 direction;
    private GameObject owner;
    private WeaponDefinition weaponDefinition;
    private SpriteRenderer spriteRenderer;
    private int damage;
    private float speed;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(
        Vector2 fireDirection,
        GameObject projectileOwner,
        WeaponDefinition weaponDefinition = null,
        float damageMultiplier = 1f)
    {
        direction = fireDirection.normalized;
        owner = projectileOwner;
        this.weaponDefinition = weaponDefinition;

        int baseDamage = weaponDefinition != null ? weaponDefinition.Damage : fallbackDamage;
        damage = Mathf.RoundToInt(baseDamage * damageMultiplier);

        speed = weaponDefinition != null ? weaponDefinition.ProjectileSpeed : fallbackSpeed;

        float lifetime = weaponDefinition != null ? weaponDefinition.ProjectileLifetime : fallbackLifetime;
        Destroy(gameObject, lifetime);

        ApplyWeaponVisuals();
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

    private void ApplyWeaponVisuals()
    {
        if (spriteRenderer == null || weaponDefinition == null)
        {
            return;
        }

        switch (weaponDefinition.WeaponCategory)
        {
            case WeaponCategory.Laser:
                spriteRenderer.color = Color.red;
                break;

            case WeaponCategory.Railgun:
                spriteRenderer.color = Color.yellow;
                break;

            case WeaponCategory.EMP:
                spriteRenderer.color = Color.cyan;
                break;

            case WeaponCategory.Missile:
                spriteRenderer.color = Color.white;
                break;

            case WeaponCategory.BoardingPod:
                spriteRenderer.color = Color.magenta;
                break;
        }
    }
}