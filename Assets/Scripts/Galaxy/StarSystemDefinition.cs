using UnityEngine;

[CreateAssetMenu(fileName = "New Star System", menuName = "Pirates In Space/Star System")]
public class StarSystemDefinition : ScriptableObject
{
    [SerializeField] private string systemName = "Unknown System";
    [SerializeField] private bool hasStation = true;
    [SerializeField] private bool isPirateControlled = false;

    public string SystemName => systemName;
    public bool HasStation => hasStation;
    public bool IsPirateControlled => isPirateControlled;
}