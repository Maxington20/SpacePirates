using UnityEngine;

public class StationSpawner : MonoBehaviour
{
    [SerializeField] private GameObject stationPrefab;

    private void Start()
    {
        TrySpawnStation();
    }

    private void TrySpawnStation()
    {
        if (GameState.CurrentSystem == null)
        {
            Debug.Log("No current system set. Station will not spawn.");
            return;
        }

        if (!GameState.CurrentSystem.HasStation)
        {
            Debug.Log($"{GameState.CurrentSystem.SystemName} has no station.");
            return;
        }

        if (stationPrefab == null)
        {
            Debug.LogWarning("StationSpawner is missing a station prefab.");
            return;
        }

        Instantiate(stationPrefab, transform.position, Quaternion.identity);
    }
}