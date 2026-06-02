using UnityEngine;

public class StationDockingController : MonoBehaviour
{
    [SerializeField] private TargetingController targetingController;
    [SerializeField] private KeyCode dockKey = KeyCode.E;

    private void Awake()
    {
        if (targetingController == null)
        {
            targetingController = GetComponent<TargetingController>();
        }
    }

    private void Update()
    {
        if (!Input.GetKeyDown(dockKey))
        {
            return;
        }

        TryDock();
    }

    private void TryDock()
    {
        if (targetingController == null || targetingController.CurrentTarget == null)
        {
            Debug.Log("No target selected.");
            return;
        }

        Station station = targetingController.CurrentTarget.GetComponent<Station>();

        if (station == null)
        {
            Debug.Log("Current target is not a station.");
            return;
        }

        float distance = Vector2.Distance(transform.position, station.transform.position);

        if (distance > station.DockingRange)
        {
            Debug.Log($"Too far to dock with {station.StationName}.");
            return;
        }

        Debug.Log($"Docked at {station.StationName}.");
    }
}