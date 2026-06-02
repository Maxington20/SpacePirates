using UnityEngine;

[CreateAssetMenu(fileName = "New Ship Definition", menuName = "Pirates In Space/Ship Definition")]
public class ShipDefinition : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string shipName = "Unknown Ship";
    [SerializeField] private string shipClass = "Scout";

    [Header("Core Stats")]
    [SerializeField] private int maxHull = 100;
    [SerializeField] private float forwardSpeed = 6f;
    [SerializeField] private float reverseSpeed = 3f;
    [SerializeField] private float rotationSpeed = 180f;

    [Header("Capacity")]
    [SerializeField] private int crewCapacity = 5;
    [SerializeField] private int cargoCapacity = 10;

    public string ShipName => shipName;
    public string ShipClass => shipClass;
    public int MaxHull => maxHull;
    public float ForwardSpeed => forwardSpeed;
    public float ReverseSpeed => reverseSpeed;
    public float RotationSpeed => rotationSpeed;
    public int CrewCapacity => crewCapacity;
    public int CargoCapacity => cargoCapacity;
}