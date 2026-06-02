using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool spawnOnStart = true;

    private GameObject spawnedEnemy;

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner is missing an enemy prefab.");
            return;
        }

        Vector3 position = spawnPoint != null ? spawnPoint.position : transform.position;

        spawnedEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
    }
}