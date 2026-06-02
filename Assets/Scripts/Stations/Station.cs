using UnityEngine;

public class Station : MonoBehaviour
{
    [SerializeField] private string stationName = "Station";
    [SerializeField] private float dockingRange = 3f;

    public string StationName => stationName;
    public float DockingRange => dockingRange;
}