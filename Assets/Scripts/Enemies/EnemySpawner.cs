using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("랜덤 스폰용 적 목록")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("패턴 웨이브 설정")]
    [SerializeField] private WavePattern[] wavePatterns;

    [Header("대상")]
    [SerializeField] private Transform player;

    [Header("기본 스폰 설정")]
    [SerializeField] private float spawnDistance = 8f;

    [Header("웨이브 설정")]
    [SerializeField] private int startWave = 1;
    [SerializeField] private int baseEnemyCount = 5;
    [SerializeField] private int enemyCountIncreasePerWave = 2;
    [SerializeField] private float waveStartDelay = 2f;

    [Header("랜덤 웨이브 기본 스폰 방식")]
    [SerializeField] private SpawnMode defaultSpawnMode = SpawnMode.Sequential;
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

        // 패턴 웨이브가 없는 경우에만 랜덤 웨이브 스폰 실행
        if (currentWave - 1 >= wavePatterns.Length)
        {
            switch (defaultSpawnMode)
            {
                case SpawnMode.Sequential:
                    UpdateSequentialSpawn();
                    break;

                case SpawnMode.Burst:
                    UpdateBurstSpawn();
                    break;
            }
        }

        // 이번 웨이브 적을 전부 생성했고, 살아있는 적도 없으면 웨이브 종료
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
        enemiesSpawnedInWave = 0;
        spawnTimer = 0f;
        isWaveActive = true;

        // 패턴 웨이브가 있으면 그걸 우선 사용
        if (currentWave - 1 < wavePatterns.Length)
        {
            StartPatternWave(wavePatterns[currentWave - 1]);
        }
        else
        {
            enemiesToSpawn = baseEnemyCount + (currentWave - 1) * enemyCountIncreasePerWave;
            Debug.Log("Random Wave Start : " + currentWave + " / Enemy Count : " + enemiesToSpawn);
        }
    }

    private void StartPatternWave(WavePattern pattern)
    {
        enemiesToSpawn = 0;

        if (pattern.spawnGroups != null)
        {
            foreach (SpawnGroup group in pattern.spawnGroups)
            {
                enemiesToSpawn += group.count;
            }

            foreach (SpawnGroup group in pattern.spawnGroups)
            {
                StartCoroutine(SpawnGroupRoutine(group));
            }
        }

        Debug.Log("Pattern Wave Start : " + pattern.waveName + " / Enemy Count : " + enemiesToSpawn);
    }

    private IEnumerator SpawnGroupRoutine(SpawnGroup group)
    {
        if (group.enemyPrefab == null)
        {
            yield break;
        }

        if (group.spawnMode == SpawnMode.Burst)
        {
            for (int i = 0; i < group.count; i++)
            {
                SpawnSpecificEnemy(group.enemyPrefab);
            }
        }
        else
        {
            for (int i = 0; i < group.count; i++)
            {
                SpawnSpecificEnemy(group.enemyPrefab);
                yield return new WaitForSeconds(group.spawnInterval);
            }
        }
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
            SpawnOneRandomEnemy();
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
                SpawnOneRandomEnemy();
            }
        }
    }

    private void SpawnOneRandomEnemy()
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

        Vector2 spawnPosition = GetRandomSpawnPosition();
        GameObject spawnedEnemy = Instantiate(selectedEnemy, spawnPosition, Quaternion.identity);

        aliveEnemies.Add(spawnedEnemy);
        enemiesSpawnedInWave++;
    }

    private void SpawnSpecificEnemy(GameObject prefab)
    {
        if (player == null || prefab == null)
        {
            return;
        }

        Vector2 spawnPosition = GetRandomSpawnPosition();
        GameObject spawnedEnemy = Instantiate(prefab, spawnPosition, Quaternion.identity);

        aliveEnemies.Add(spawnedEnemy);
        enemiesSpawnedInWave++;
    }

    private Vector2 GetRandomSpawnPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        if (randomDirection == Vector2.zero)
        {
            randomDirection = Vector2.right;
        }

        return (Vector2)player.position + randomDirection * spawnDistance;
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