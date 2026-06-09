using UnityEngine;

public class ShipLoadout : MonoBehaviour
{
    [SerializeField] private WeaponDefinition primaryWeapon;
    [SerializeField] private WeaponDefinition secondaryWeapon;
    [SerializeField] private WeaponDefinition utilityWeapon;
    [SerializeField] private FlareDefinition flareDefinition;

    public WeaponDefinition PrimaryWeapon => primaryWeapon;
    public WeaponDefinition SecondaryWeapon => secondaryWeapon;
    public WeaponDefinition UtilityWeapon => utilityWeapon;
    public FlareDefinition FlareDefinition => flareDefinition;
}