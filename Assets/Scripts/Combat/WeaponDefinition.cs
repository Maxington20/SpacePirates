using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Definition", menuName = "Pirates In Space/Weapon Definition")]
public class WeaponDefinition : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string weaponName = "Light Laser";
    [SerializeField] private WeaponCategory weaponCategory = WeaponCategory.Laser;

    [Header("Base Combat")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float projectileSpeed = 14f;
    [SerializeField] private float fireCooldown = 0.25f;
    [SerializeField] private float projectileLifetime = 2f;

    [Header("Damage Modifiers")]
    [SerializeField] private float shieldDamageMultiplier = 1f;
    [SerializeField] private float hullDamageMultiplier = 1f;
    [SerializeField] private float crewDamageMultiplier = 1f;
    [SerializeField] private float systemDamageMultiplier = 1f;

    public string WeaponName => weaponName;
    public WeaponCategory WeaponCategory => weaponCategory;

    public int Damage => damage;
    public float ProjectileSpeed => projectileSpeed;
    public float FireCooldown => fireCooldown;
    public float ProjectileLifetime => projectileLifetime;

    public float ShieldDamageMultiplier => shieldDamageMultiplier;
    public float HullDamageMultiplier => hullDamageMultiplier;
    public float CrewDamageMultiplier => crewDamageMultiplier;
    public float SystemDamageMultiplier => systemDamageMultiplier;
}