using UnityEngine;

[RequireComponent(typeof(ShipHealth))]
public class Targetable : MonoBehaviour
{
    [SerializeField] private string displayName = "Unknown Ship";
    [SerializeField] private bool isTargetable = true;

    public string DisplayName => displayName;
    public bool IsTargetable => isTargetable;
    public ShipHealth Health { get; private set; }

    private void Awake()
    {
        Health = GetComponent<ShipHealth>();
    }
}