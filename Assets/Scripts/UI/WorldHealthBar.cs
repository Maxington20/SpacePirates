using UnityEngine;
using UnityEngine.UI;

public class WorldHealthBar : MonoBehaviour
{
    [SerializeField] private ShipHealth shipHealth;
    [SerializeField] private Image fillImage;
    [SerializeField] private Vector3 worldOffset = new Vector3(0f, 1.2f, 0f);

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        if (shipHealth != null)
        {
            shipHealth.HealthChanged += HandleHealthChanged;
            shipHealth.ShipDestroyed += HandleShipDestroyed;
        }
    }

    private void OnDisable()
    {
        if (shipHealth != null)
        {
            shipHealth.HealthChanged -= HandleHealthChanged;
            shipHealth.ShipDestroyed -= HandleShipDestroyed;
        }
    }

    private void LateUpdate()
    {
        if (shipHealth == null || mainCamera == null)
        {
            return;
        }

        transform.position = mainCamera.WorldToScreenPoint(shipHealth.transform.position + worldOffset);
    }

    public void Initialize(ShipHealth health)
    {
        if (shipHealth != null)
        {
            shipHealth.HealthChanged -= HandleHealthChanged;
            shipHealth.ShipDestroyed -= HandleShipDestroyed;
        }

        shipHealth = health;

        shipHealth.HealthChanged += HandleHealthChanged;
        shipHealth.ShipDestroyed += HandleShipDestroyed;

        HandleHealthChanged(shipHealth.CurrentHealth, shipHealth.MaxHealth);
    }

    private void HandleHealthChanged(int currentHealth, int maxHealth)
    {
        if (fillImage == null || maxHealth <= 0)
        {
            return;
        }

        fillImage.fillAmount = (float)currentHealth / maxHealth;
    }

    private void HandleShipDestroyed()
    {
        Destroy(gameObject);
    }
}