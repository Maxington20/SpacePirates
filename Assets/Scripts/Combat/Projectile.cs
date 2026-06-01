using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 14f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 2f;

    private Vector2 direction;
    private GameObject owner;

    public void Initialize(Vector2 fireDirection, GameObject projectileOwner)
    {
        direction = fireDirection.normalized;
        owner = projectileOwner;

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

        shipHealth.TakeDamage(damage);
        Destroy(gameObject);
    }
}