using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("스폰할 적 목록")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("대상")]
    [SerializeField] private Transform player;

    [Header("스폰 설정")]
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float spawnDistance = 8f;

    [Header("웨이브 설정")]
    [SerializeField] private int startWave = 1;
    [SerializeField] private int baseEnemyCount = 5;
    [SerializeField] private int enemyCountIncreasePerWave = 2;
    [SerializeField] private float waveStartDelay = 2f;

    private int currentWave;
    private int enemiesToSpawn;
    private int enemiesSpawnedInWave;

    private float spawnTimer;
    private float waveDelayTimer;

    private bool isWaveActive;
    private bool isWaitingNextWave;

    private List<GameObject> aliveEnemies = new List<GameObject>();

    private void Start()
    {
        currentWave = startWave - 1;
        StartNextWave();
    }

    private void Update()
    {
        CleanupDeadEnemies();

        // 다음 웨이브 대기 상태
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

        // 웨이브 진행 중이면 적 순차 스폰
        if (isWaveActive)
        {
            spawnTimer += Time.deltaTime;

            if (enemiesSpawnedInWave < enemiesToSpawn && spawnTimer >= spawnInterval)
            {
                spawnTimer = 0f;
                SpawnEnemy();
            }

            // 이번 웨이브 적을 전부 생성했고, 살아있는 적도 없으면 다음 웨이브
            if (enemiesSpawnedInWave >= enemiesToSpawn && aliveEnemies.Count == 0)
            {
                isWaveActive = false;
                isWaitingNextWave = true;

                Debug.Log("Wave Clear : " + currentWave);
            }
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

    private void SpawnEnemy()
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