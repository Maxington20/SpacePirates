using UnityEngine;

[CreateAssetMenu(
    fileName = "New Ship Role Profile",
    menuName = "Pirates In Space/Ship Role Profile")]
public class ShipRoleProfile : ScriptableObject
{
    [Header("Role")]
    [SerializeField] private ShipRole role;

    [Header("Combat")]
    [SerializeField] private float weaponDamageMultiplier = 1f;
    [SerializeField] private float missileDamageMultiplier = 1f;

    [Header("Mobility")]
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private float boostMultiplier = 1f;

    [Header("Boarding")]
    [SerializeField] private float boardingStrengthMultiplier = 1f;

    [Header("Defense")]
    [SerializeField] private float shieldMultiplier = 1f;
    [SerializeField] private float hullMultiplier = 1f;

    public ShipRole Role => role;

    public float WeaponDamageMultiplier => weaponDamageMultiplier;
    public float MissileDamageMultiplier => missileDamageMultiplier;

    public float SpeedMultiplier => speedMultiplier;
    public float BoostMultiplier => boostMultiplier;

    public float BoardingStrengthMultiplier => boardingStrengthMultiplier;

    public float ShieldMultiplier => shieldMultiplier;
    public float HullMultiplier => hullMultiplier;
}