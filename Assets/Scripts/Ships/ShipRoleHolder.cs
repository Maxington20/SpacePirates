using UnityEngine;

public class ShipRoleHolder : MonoBehaviour
{
    [SerializeField] private ShipRoleProfile roleProfile;

    public ShipRoleProfile RoleProfile => roleProfile;
}