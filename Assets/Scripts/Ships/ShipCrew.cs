using System;
using UnityEngine;

public class ShipCrew : MonoBehaviour
{
    [SerializeField] private int fallbackCrewCapacity = 5;
    [SerializeField] private int fallbackStartingCrew = 5;

    public int CrewCapacity { get; private set; }
    public int CurrentCrew { get; private set; }

    public event Action<int, int> CrewChanged;

    private void Awake()
    {
        ShipDefinitionHolder holder = GetComponent<ShipDefinitionHolder>();

        if (holder != null && holder.ShipDefinition != null)
        {
            CrewCapacity = holder.ShipDefinition.CrewCapacity;
            CurrentCrew = Mathf.Clamp(holder.ShipDefinition.StartingCrew, 0, CrewCapacity);
        }
        else
        {
            CrewCapacity = fallbackCrewCapacity;
            CurrentCrew = Mathf.Clamp(fallbackStartingCrew, 0, CrewCapacity);
        }
    }

    private void Start()
    {
        CrewChanged?.Invoke(CurrentCrew, CrewCapacity);
    }

    public void LoseCrew(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        CurrentCrew = Mathf.Max(CurrentCrew - amount, 0);
        CrewChanged?.Invoke(CurrentCrew, CrewCapacity);
    }

    public void AddCrew(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        CurrentCrew = Mathf.Min(CurrentCrew + amount, CrewCapacity);
        CrewChanged?.Invoke(CurrentCrew, CrewCapacity);
    }
}