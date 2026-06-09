using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 220f;

    private Transform target;
    private float moveSpeed;
    private int damage;
    private GameObject owner;
    private WeaponDefinition weaponDefinition;
    private bool hasLostLock;

    public GameObject Owner => owner;

    public void Initialize(
        Transform target,
        GameObject owner,
        WeaponDefinition weaponDefinition,
        float damageMultiplier)
    {
        this.target = target;
        this.owner = owner;
        this.weaponDefinition = weaponDefinition;

        damage = Mathf.RoundToInt(weaponDefinition.Damage * damageMultiplier);
        moveSpeed = weaponDefinition.ProjectileSpeed;

        Destroy(gameObject, weaponDefinition.ProjectileLifetime);
    }

    private void Update()
    {
        if (target != null && !hasLostLock)
        {
            RotateTowardTarget();
        }

        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }

    public void LoseLock()
    {
        hasLostLock = true;
        target = null;
    }

    private void RotateTowardTarget()
    {
        Vector2 directionToTarget = target.position - transform.position;

        if (directionToTarget.sqrMagnitude <= 0.001f)
        {
            return;
        }

        float targetAngle =
            Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;

        float newAngle = Mathf.MoveTowardsAngle(
            transform.eulerAngles.z,
            targetAngle,
            turnSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (owner != null && other.gameObject == owner)
        {
            return;
        }

        ShipHealth health = other.GetComponent<ShipHealth>();

        if (health == null)
        {
            return;
        }

        health.TakeDamage(damage, weaponDefinition);
        Destroy(gameObject);
    }
}