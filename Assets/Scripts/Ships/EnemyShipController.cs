using UnityEngine;

public class EnemyShipController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private float stoppingDistance = 3f;

    private void Start()
    {
        if (target != null)
        {
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            target = player.transform;
        }
    }

    private void Update()
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
        float currentAngle = transform.eulerAngles.z;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
    }

    private void MoveTowardTarget()
    {
        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= stoppingDistance)
        {
            return;
        }

        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }
}