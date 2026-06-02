using UnityEngine;

public class ShipDefinitionHolder : MonoBehaviour
{
    [SerializeField] private ShipDefinition shipDefinition;

    public ShipDefinition ShipDefinition => shipDefinition;
}