using UnityEngine;

public class Station : MonoBehaviour
{
    [SerializeField] private string stationName = "Station";

    public string StationName => stationName;
}