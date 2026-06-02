using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Definition", menuName = "Pirates In Space/Weapon Definition")]
public class WeaponDefinition : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string weaponName = "Light Laser";

    [Header("Combat")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float projectileSpeed = 14f;
    [SerializeField] private float fireCooldown = 0.25f;
    [SerializeField] private float projectileLifetime = 2f;

    public string WeaponName => weaponName;
    public int Damage => damage;
    public float ProjectileSpeed => projectileSpeed;
    public float FireCooldown => fireCooldown;
    public float ProjectileLifetime => projectileLifetime;
}