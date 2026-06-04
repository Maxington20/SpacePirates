using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    [SerializeField] private int startingCredits = 500;

    public int Credits { get; private set; }

    private void Awake()
    {
        Credits = startingCredits;
    }

    public bool CanAfford(int amount)
    {
        return Credits >= amount;
    }

    public void AddCredits(int amount)
    {
        Credits += amount;
    }

    public bool SpendCredits(int amount)
    {
        if (!CanAfford(amount))
        {
            return false;
        }

        Credits -= amount;
        return true;
    }
}