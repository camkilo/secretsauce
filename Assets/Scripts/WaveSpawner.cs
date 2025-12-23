using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages enemy wave spawning and difficulty progression
/// </summary>
public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance { get; private set; }

    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform arenaCenter;
    [SerializeField] private float arenaRadius = 15f;
    [SerializeField] private float spawnHeight = 0.5f;
    
    [Header("Wave Configuration")]
    [SerializeField] private float timeBetweenWaves = 10f;
    [SerializeField] private int baseEnemiesPerWave = 3;
    [SerializeField] private float difficultyScaling = 1.2f;
    
    private int currentWave = 0;
    private int enemiesAlive = 0;
    private float waveTimer;
    private bool waveActive;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (arenaCenter == null)
        {
            arenaCenter = transform;
        }
        
        waveTimer = 3f; // Initial delay before first wave
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameActive()) return;

        if (!waveActive)
        {
            waveTimer -= Time.deltaTime;
            if (waveTimer <= 0)
            {
                StartNextWave();
            }
        }
        else if (enemiesAlive <= 0)
        {
            waveActive = false;
            waveTimer = timeBetweenWaves;
        }
    }

    void StartNextWave()
    {
        currentWave++;
        waveActive = true;
        
        int enemyCount = Mathf.FloorToInt(baseEnemiesPerWave * Mathf.Pow(difficultyScaling, currentWave - 1));
        enemyCount = Mathf.Min(enemyCount, 20); // Cap at 20 enemies per wave
        
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        // Random position on circle perimeter
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 spawnPosition = arenaCenter.position + new Vector3(
            Mathf.Cos(angle) * arenaRadius,
            spawnHeight,
            Mathf.Sin(angle) * arenaRadius
        );

        GameObject enemy;
        
        if (enemyPrefab != null)
        {
            enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            // Create basic enemy if prefab is missing
            enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.transform.position = spawnPosition;
            enemy.tag = "Enemy";
            enemy.AddComponent<EnemyController>();
        }

        // Assign enemy type based on wave progression and randomness
        EnemyController controller = enemy.GetComponent<EnemyController>();
        if (controller != null)
        {
            controller.SetEnemyType(GetRandomEnemyType());
        }

        spawnedEnemies.Add(enemy);
        enemiesAlive++;
    }

    EnemyController.EnemyType GetRandomEnemyType()
    {
        // Distribution: 50% Rushers, 30% Brutes, 20% Casters
        float rand = Random.Range(0f, 1f);
        
        if (rand < 0.5f)
        {
            return EnemyController.EnemyType.Rusher;
        }
        else if (rand < 0.8f)
        {
            return EnemyController.EnemyType.Brute;
        }
        else
        {
            return EnemyController.EnemyType.Caster;
        }
    }

    public void OnEnemyDeath()
    {
        enemiesAlive--;
        enemiesAlive = Mathf.Max(0, enemiesAlive);
    }

    public int GetCurrentWave() => currentWave;
    public int GetEnemiesAlive() => enemiesAlive;
}
