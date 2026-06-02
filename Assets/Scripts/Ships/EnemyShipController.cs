using UnityEngine;

public class EnemyShipController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Fallback Movement")]
    [SerializeField] private float fallbackMoveSpeed = 2f;
    [SerializeField] private float fallbackRotationSpeed = 120f;
    [SerializeField] private float stoppingDistance = 3f;

    private float moveSpeed;
    private float rotationSpeed;

    private void Awake()
    {
        ShipDefinitionHolder holder = GetComponent<ShipDefinitionHolder>();

        if (holder != null && holder.ShipDefinition != null)
        {
            moveSpeed = holder.ShipDefinition.ForwardSpeed;
            rotationSpeed = holder.ShipDefinition.RotationSpeed;
        }
        else
        {
            moveSpeed = fallbackMoveSpeed;
            rotationSpeed = fallbackRotationSpeed;
        }
    }

    private void Start()
    {
        if (target != null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            target = player.transform;
        }
    }

    private void Update()
    {
        if (target == null) return;

        RotateTowardTarget();
        MoveTowardTarget();
    }

    private void RotateTowardTarget()
    {
        Vector2 directionToTarget = target.position - transform.position;

        if (directionToTarget.sqrMagnitude <= 0.001f) return;

        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;
        float currentAngle = transform.eulerAngles.z;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
    }

    private void MoveTowardTarget()
    {
        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= stoppingDistance) return;

        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }
}