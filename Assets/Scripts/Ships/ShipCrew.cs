using UnityEngine;

public class ShipCrew : MonoBehaviour
{
    [SerializeField] private int fallbackCrewCapacity = 5;
    [SerializeField] private int fallbackStartingCrew = 5;

    public int CrewCapacity { get; private set; }
    public int CurrentCrew { get; private set; }

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

    public void LoseCrew(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        CurrentCrew = Mathf.Max(CurrentCrew - amount, 0);
    }
}