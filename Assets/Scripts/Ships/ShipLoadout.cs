using UnityEngine;

public class ShipLoadout : MonoBehaviour
{
    [SerializeField] private WeaponDefinition primaryWeapon;
    [SerializeField] private WeaponDefinition secondaryWeapon;
    [SerializeField] private WeaponDefinition utilityWeapon;

    public WeaponDefinition PrimaryWeapon => primaryWeapon;
    public WeaponDefinition SecondaryWeapon => secondaryWeapon;
    public WeaponDefinition UtilityWeapon => utilityWeapon;
}