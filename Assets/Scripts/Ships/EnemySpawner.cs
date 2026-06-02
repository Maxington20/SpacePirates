using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float respawnDelay = 5f;

    private GameObject spawnedEnemy;
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
            SpawnEnemy();
            waitingForRespawn = false;
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner is missing an enemy prefab.");
            return;
        }

        spawnedEnemy = Instantiate(
            enemyPrefab,
            transform.position,
            Quaternion.identity);

        ShipHealth health = spawnedEnemy.GetComponent<ShipHealth>();

        if (health != null)
        {
            health.ShipDestroyed += HandleEnemyDestroyed;
        }
    }

    private void HandleEnemyDestroyed()
    {
        waitingForRespawn = true;
        respawnTimer = respawnDelay;
    }
}