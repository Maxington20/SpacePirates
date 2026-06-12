using UnityEngine;

public class BroadsideArcController : MonoBehaviour
{
    [SerializeField] private float arcHalfAngle = 45f;

    public bool IsTargetInArc(Transform target, BroadsideSide side)
    {
        if (target == null)
        {
            return false;
        }

        Vector2 sideDirection = side == BroadsideSide.Port
            ? -transform.right
            : transform.right;

        Vector2 directionToTarget = target.position - transform.position;

        if (directionToTarget.sqrMagnitude <= 0.001f)
        {
            return false;
        }

        float angle = Vector2.Angle(sideDirection, directionToTarget.normalized);

        return angle <= arcHalfAngle;
    }

    public Vector2 GetFireDirection(BroadsideSide side)
    {
        return side == BroadsideSide.Port
            ? -transform.right
            : transform.right;
    }
}