using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private enum SpawnMode
    {
        Sequential,
        Burst
    }

    [Header("스폰할 적 목록")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("대상")]
    [SerializeField] private Transform player;

    [Header("기본 스폰 설정")]
    [SerializeField] private float spawnDistance = 8f;

    [Header("웨이브 설정")]
    [SerializeField] private int startWave = 1;
    [SerializeField] private int baseEnemyCount = 5;
    [SerializeField] private int enemyCountIncreasePerWave = 2;
    [SerializeField] private float waveStartDelay = 2f;

    [Header("스폰 방식 설정")]
    [SerializeField] private SpawnMode spawnMode = SpawnMode.Sequential;
    [SerializeField] private float sequentialSpawnInterval = 1f;
    [SerializeField] private int burstSpawnCountPerBatch = 3;
    [SerializeField] private float burstSpawnInterval = 2f;

    private int currentWave;
    private int enemiesToSpawn;
    private int enemiesSpawnedInWave;

    private float spawnTimer;
    private float waveDelayTimer;

    private bool isWaveActive;
    private bool isWaitingNextWave;

    private readonly List<GameObject> aliveEnemies = new List<GameObject>();

    private void Start()
    {
        currentWave = startWave - 1;
        StartNextWave();
    }

    private void Update()
    {
        CleanupDeadEnemies();

        if (isWaitingNextWave)
        {
            waveDelayTimer += Time.deltaTime;

            if (waveDelayTimer >= waveStartDelay)
            {
                waveDelayTimer = 0f;
                isWaitingNextWave = false;
                StartNextWave();
            }

            return;
        }

        if (!isWaveActive)
        {
            return;
        }

        switch (spawnMode)
        {
            case SpawnMode.Sequential:
                UpdateSequentialSpawn();
                break;

            case SpawnMode.Burst:
                UpdateBurstSpawn();
                break;
        }

        // 현재 웨이브의 적을 전부 스폰했고, 살아있는 적도 없으면 클리어
        if (enemiesSpawnedInWave >= enemiesToSpawn && aliveEnemies.Count == 0)
        {
            isWaveActive = false;
            isWaitingNextWave = true;

            Debug.Log("Wave Clear : " + currentWave);
        }
    }

    private void StartNextWave()
    {
        currentWave++;
        enemiesToSpawn = baseEnemyCount + (currentWave - 1) * enemyCountIncreasePerWave;
        enemiesSpawnedInWave = 0;
        spawnTimer = 0f;
        isWaveActive = true;

        Debug.Log("Wave Start : " + currentWave + " / Enemy Count : " + enemiesToSpawn);
    }

    private void UpdateSequentialSpawn()
    {
        if (enemiesSpawnedInWave >= enemiesToSpawn)
        {
            return;
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= sequentialSpawnInterval)
        {
            spawnTimer = 0f;
            SpawnOneEnemy();
        }
    }

    private void UpdateBurstSpawn()
    {
        if (enemiesSpawnedInWave >= enemiesToSpawn)
        {
            return;
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= burstSpawnInterval)
        {
            spawnTimer = 0f;

            int remainCount = enemiesToSpawn - enemiesSpawnedInWave;
            int spawnCount = Mathf.Min(burstSpawnCountPerBatch, remainCount);

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnOneEnemy();
            }
        }
    }

    private void SpawnOneEnemy()
    {
        if (player == null)
        {
            return;
        }

        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            return;
        }

        GameObject selectedEnemy = GetRandomEnemyPrefab();

        if (selectedEnemy == null)
        {
            return;
        }

        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        // 0 벡터 방지
        if (randomDirection == Vector2.zero)
        {
            randomDirection = Vector2.right;
        }

        Vector2 spawnPosition = (Vector2)player.position + randomDirection * spawnDistance;
        GameObject spawnedEnemy = Instantiate(selectedEnemy, spawnPosition, Quaternion.identity);

        aliveEnemies.Add(spawnedEnemy);
        enemiesSpawnedInWave++;
    }

    private GameObject GetRandomEnemyPrefab()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        return enemyPrefabs[randomIndex];
    }

    private void CleanupDeadEnemies()
    {
        for (int i = aliveEnemies.Count - 1; i >= 0; i--)
        {
            if (aliveEnemies[i] == null)
            {
                aliveEnemies.RemoveAt(i);
            }
        }
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }

    public int GetAliveEnemyCount()
    {
        return aliveEnemies.Count;
    }
}