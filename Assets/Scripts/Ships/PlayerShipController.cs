using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    [Header("Fallback Movement")]
    [SerializeField] private float fallbackForwardSpeed = 6f;
    [SerializeField] private float fallbackReverseSpeed = 3f;
    [SerializeField] private float fallbackRotationSpeed = 180f;

    private float forwardSpeed;
    private float reverseSpeed;
    private float rotationSpeed;

    private void Awake()
    {
        ShipDefinitionHolder holder = GetComponent<ShipDefinitionHolder>();

        if (holder != null && holder.ShipDefinition != null)
        {
            forwardSpeed = holder.ShipDefinition.ForwardSpeed;
            reverseSpeed = holder.ShipDefinition.ReverseSpeed;
            rotationSpeed = holder.ShipDefinition.RotationSpeed;
        }
        else
        {
            forwardSpeed = fallbackForwardSpeed;
            reverseSpeed = fallbackReverseSpeed;
            rotationSpeed = fallbackRotationSpeed;
        }
    }

    private void Update()
    {
        float thrustInput = 0f;
        float rotationInput = 0f;

        if (Input.GetKey(KeyCode.W)) thrustInput = 1f;
        else if (Input.GetKey(KeyCode.S)) thrustInput = -1f;

        if (Input.GetKey(KeyCode.A)) rotationInput = 1f;
        else if (Input.GetKey(KeyCode.D)) rotationInput = -1f;

        if (!Mathf.Approximately(rotationInput, 0f))
        {
            transform.Rotate(0f, 0f, rotationInput * rotationSpeed * Time.deltaTime);
        }

        if (!Mathf.Approximately(thrustInput, 0f))
        {
            float speed = thrustInput > 0f ? forwardSpeed : reverseSpeed;
            transform.position += transform.up * thrustInput * speed * Time.deltaTime;
        }
    }
}