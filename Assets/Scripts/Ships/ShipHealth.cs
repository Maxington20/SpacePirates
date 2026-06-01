using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;
    public bool IsDestroyed { get; private set; }

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (IsDestroyed)
        {
            return;
        }

        CurrentHealth -= amount;
        CurrentHealth = Mathf.Max(CurrentHealth, 0);

        Debug.Log($"{gameObject.name} took {amount} damage. HP: {CurrentHealth}/{maxHealth}");

        if (CurrentHealth <= 0)
        {
            DestroyShip();
        }
    }

    private void DestroyShip()
    {
        IsDestroyed = true;

        Debug.Log($"{gameObject.name} destroyed.");

        Destroy(gameObject);
    }
}