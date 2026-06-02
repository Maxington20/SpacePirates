using UnityEngine;

public class ShipLoadout : MonoBehaviour
{
    [SerializeField] private WeaponDefinition primaryWeapon;

    public WeaponDefinition PrimaryWeapon => primaryWeapon;
}