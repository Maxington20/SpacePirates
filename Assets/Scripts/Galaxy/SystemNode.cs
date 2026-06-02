using UnityEngine;

public class SystemNode : MonoBehaviour
{
    [SerializeField] private StarSystemDefinition systemDefinition;

    public StarSystemDefinition SystemDefinition => systemDefinition;
}