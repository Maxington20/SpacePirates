using UnityEngine;

[RequireComponent(typeof(ShipHealth))]
public class Targetable : MonoBehaviour
{
    [SerializeField] private string fallbackDisplayName = "Unknown Ship";
    [SerializeField] private bool isTargetable = true;

    public bool IsTargetable => isTargetable;
    public ShipHealth Health { get; private set; }

    public string DisplayName
    {
        get
        {
            ShipDefinitionHolder holder = GetComponent<ShipDefinitionHolder>();

            if (holder != null && holder.ShipDefinition != null)
            {
                return holder.ShipDefinition.ShipName;
            }

            return fallbackDisplayName;
        }
    }

    private void Awake()
    {
        Health = GetComponent<ShipHealth>();
    }
}