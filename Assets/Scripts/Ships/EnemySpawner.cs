using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject defaultEnemyPrefab;
    [SerializeField] private GameObject pirateEnemyPrefab;

    [Header("Respawn")]
    [SerializeField] private float respawnDelay = 5f;

    private float respawnTimer;
    private bool waitingForRespawn;

    private void Start()
    {
        SpawnEnemy();
    }

    private void Update()
    {
        if (!waitingForRespawn)
        {
            return;
        }

        respawnTimer -= Time.deltaTime;

        if (respawnTimer <= 0f)
        {
            waitingForRespawn = false;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        GameObject prefabToSpawn = GetPrefabForCurrentSystem();

        if (prefabToSpawn == null)
        {
            Debug.LogWarning("EnemySpawner has no valid enemy prefab assigned.");
            return;
        }

        GameObject spawnedEnemy = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

        ShipHealth health = spawnedEnemy.GetComponent<ShipHealth>();

        if (health != null)
        {
            health.ShipDestroyed += HandleEnemyDestroyed;
        }
    }

    private GameObject GetPrefabForCurrentSystem()
    {
        if (GameState.CurrentSystem != null && GameState.CurrentSystem.IsPirateControlled)
        {
            return pirateEnemyPrefab != null ? pirateEnemyPrefab : defaultEnemyPrefab;
        }

        return defaultEnemyPrefab;
    }

    private void HandleEnemyDestroyed()
    {
        waitingForRespawn = true;
        respawnTimer = respawnDelay;
    }
}