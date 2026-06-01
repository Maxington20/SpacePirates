using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerShipController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float thrustForce = 8f;
    [SerializeField] private float reverseForce = 4f;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float maxSpeed = 8f;

    private Rigidbody2D rb;

    private float thrustInput;
    private float rotationInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        thrustInput = 0f;
        rotationInput = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            thrustInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            thrustInput = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            rotationInput = 1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotationInput = -1f;
        }
    }

    private void FixedUpdate()
    {
        RotateShip();
        MoveShip();
        ClampSpeed();
    }

    private void RotateShip()
    {
        if (Mathf.Approximately(rotationInput, 0f))
        {
            return;
        }

        float rotationAmount = rotationInput * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotationAmount);
    }

    private void MoveShip()
    {
        if (Mathf.Approximately(thrustInput, 0f))
        {
            return;
        }

        float force = thrustInput > 0f ? thrustForce : reverseForce;
        Vector2 direction = transform.up * thrustInput;

        rb.AddForce(direction * force, ForceMode2D.Force);
    }

    private void ClampSpeed()
    {
        if (rb.linearVelocity.magnitude <= maxSpeed)
        {
            return;
        }

        rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }
}