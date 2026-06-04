using UnityEngine;

[CreateAssetMenu(fileName = "New Cargo Item", menuName = "Pirates In Space/Cargo Item")]
public class CargoItemDefinition : ScriptableObject
{
    [SerializeField] private string itemName = "Cargo";
    [SerializeField] private int basePrice = 10;

    public string ItemName => itemName;
    public int BasePrice => basePrice;
}