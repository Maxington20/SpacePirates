using UnityEngine;

[CreateAssetMenu(fileName = "New Flare Definition", menuName = "Pirates In Space/Flare Definition")]
public class FlareDefinition : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string flareName = "Basic Flares";

    [Header("Effect")]
    [SerializeField] private float flareRadius = 6f;
    [SerializeField] private float cooldown = 4f;
    [SerializeField] private float missileSpoofChance = 1f;

    [Header("Visuals")]
    [SerializeField] private GameObject flareEffectPrefab;

    public string FlareName => flareName;
    public float FlareRadius => flareRadius;
    public float Cooldown => cooldown;
    public float MissileSpoofChance => missileSpoofChance;
    public GameObject FlareEffectPrefab => flareEffectPrefab;
}