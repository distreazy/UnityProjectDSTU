using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Префабы врагов")]
    public GameObject normalEnemyPrefab;
    public GameObject heavyEnemyPrefab;

    [Header("Настройки спавна")]
    public float spawnInterval = 5f;
    public float spawnRadius = 15f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval);
    }

    void SpawnEnemy()
    {
        // Выбираем случайную точку вокруг спавнера
        Vector3 randomPoint = GetRandomSpawnPoint();
        if (randomPoint == Vector3.zero) return;

        // 20% шанс заспавнить тяжеловеса
        GameObject enemyToSpawn = Random.value > 0.8f ? heavyEnemyPrefab : normalEnemyPrefab;

        if (enemyToSpawn != null)
        {
            Instantiate(enemyToSpawn, randomPoint, Quaternion.identity);
        }
    }

    Vector3 GetRandomSpawnPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection += transform.position;

        UnityEngine.AI.NavMeshHit hit;

        if (UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, UnityEngine.AI.NavMesh.AllAreas))
        {
            return hit.position;
        }
        return Vector3.zero;
    }
}