using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyShipController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private float stoppingDistance = 3f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

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

    private void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        RotateTowardTarget();
        MoveTowardTarget();
    }

    private void RotateTowardTarget()
    {
        Vector2 directionToTarget = target.position - transform.position;

        if (directionToTarget.sqrMagnitude <= 0.001f)
        {
            return;
        }

        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;
        float newAngle = Mathf.MoveTowardsAngle(rb.rotation, targetAngle, rotationSpeed * Time.fixedDeltaTime);

        rb.MoveRotation(newAngle);
    }

    private void MoveTowardTarget()
    {
        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= stoppingDistance)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = transform.up * moveSpeed;
    }
}