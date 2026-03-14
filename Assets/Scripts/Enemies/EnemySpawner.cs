using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    [Header("방 전투 설정")]
    [SerializeField] private bool startOnPlay = true;
    [SerializeField] private bool useRoomWaveLimit = true;
    [SerializeField] private int maxWavesPerRoom = 5;

    [Header("랜덤 웨이브 기본 스폰 방식")]
    [SerializeField] private SpawnMode defaultSpawnMode = SpawnMode.Sequential;
    [SerializeField] private float sequentialSpawnInterval = 1f;
    [SerializeField] private int burstSpawnCountPerBatch = 3;
    [SerializeField] private float burstSpawnInterval = 2f;

    [Header("스폰 인디케이터")]
    [SerializeField] private GameObject spawnIndicatorPrefab;
    [SerializeField] private float spawnIndicatorDuration = 0.8f;
    [SerializeField] private bool useSpawnIndicator = true;

    [Header("엘리트 웨이브 설정")]
    [SerializeField] private bool useEliteWave = true;
    [SerializeField] private int eliteWaveNumber = 3;
    [SerializeField] private GameObject eliteEnemyPrefab;
    [SerializeField] private int eliteEnemyCount = 1;
    [SerializeField] private bool spawnEliteOnlyOnce = true;

    private bool hasSpawnedEliteWave;

    private int currentWave;
    private int enemiesToSpawn;
    private int enemiesSpawnedInWave;

    private float spawnTimer;
    private float waveDelayTimer;

    private bool isWaveActive;
    private bool isWaitingNextWave;
    private bool isBattleStarted;
    private bool isRoomCleared;

    public Action OnRoomCleared;

    private readonly List<GameObject> aliveEnemies = new List<GameObject>();

    private void Start()
    {
        currentWave = startWave - 1;

        if (startOnPlay)
        {
            StartRoomBattle();
        }
    }

    private void Update()
    {
        if (!isBattleStarted || isRoomCleared)
        {
            return;
        }

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

        // 패턴 웨이브가 끝난 뒤에만 랜덤 웨이브 스폰
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

        // 웨이브 종료 판정
        if (enemiesSpawnedInWave >= enemiesToSpawn && aliveEnemies.Count == 0)
        {
            isWaveActive = false;

            Debug.Log("Wave Clear : " + currentWave);

            // 마지막 웨이브면 방 클리어
            if (useRoomWaveLimit && currentWave >= maxWavesPerRoom)
            {
                ClearRoom();
            }
            else
            {
                isWaitingNextWave = true;
            }
        }
    }

    public void StartRoomBattle()
    {
        if (isBattleStarted)
        {
            return;
        }

        isBattleStarted = true;
        isRoomCleared = false;
        isWaitingNextWave = false;
        isWaveActive = false;
        waveDelayTimer = 0f;
        spawnTimer = 0f;

        currentWave = startWave - 1;
        StartNextWave();
    }

    private void StartNextWave()
    {
        if (useRoomWaveLimit && currentWave >= maxWavesPerRoom)
        {
            ClearRoom();
            return;
        }

        currentWave++;
        enemiesSpawnedInWave = 0;
        spawnTimer = 0f;
        isWaveActive = true;

        // 엘리트 웨이브 우선 처리
        if (ShouldStartEliteWave())
        {
            StartEliteWave();
            return;
        }

        // 패턴 웨이브가 있으면 우선 사용
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

    private bool ShouldStartEliteWave()
    {
        if (!useEliteWave)
        {
            return false;
        }

        if (eliteEnemyPrefab == null)
        {
            return false;
        }

        if (currentWave != eliteWaveNumber)
        {
            return false;
        }

        if (spawnEliteOnlyOnce && hasSpawnedEliteWave)
        {
            return false;
        }

        return true;
    }

    private void StartEliteWave()
    {
        enemiesToSpawn = eliteEnemyCount;

        for (int i = 0; i < eliteEnemyCount; i++)
        {
            SpawnSpecificEnemy(eliteEnemyPrefab);
        }

        hasSpawnedEliteWave = true;

        Debug.Log("Elite Wave Start : " + currentWave + " / Elite Count : " + eliteEnemyCount);
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
        StartCoroutine(SpawnEnemyWithIndicator(selectedEnemy, spawnPosition));
    }

    private void SpawnSpecificEnemy(GameObject prefab)
    {
        if (player == null || prefab == null)
        {
            return;
        }

        Vector2 spawnPosition = GetRandomSpawnPosition();
        StartCoroutine(SpawnEnemyWithIndicator(prefab, spawnPosition));
    }

    // 인디케이터를 먼저 보여주고 일정 시간 후에 적을 생성하는 코루틴
    private System.Collections.IEnumerator SpawnEnemyWithIndicator(GameObject enemyPrefab, Vector2 spawnPosition)
    {
        // 인디케이터 표시
        if (useSpawnIndicator && spawnIndicatorPrefab != null)
        {
            GameObject indicator = Instantiate(spawnIndicatorPrefab, spawnPosition, Quaternion.identity);

            SpawnIndicator indicatorComponent = indicator.GetComponent<SpawnIndicator>();
            if (indicatorComponent != null)
            {
                indicatorComponent.SetLifeTime(spawnIndicatorDuration);
            }

            yield return new WaitForSeconds(spawnIndicatorDuration);
        }

        // 실제 적 생성
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        aliveEnemies.Add(spawnedEnemy);
        enemiesSpawnedInWave++;
    }

    private Vector2 GetRandomSpawnPosition()
    {
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;

        if (randomDirection == Vector2.zero)
        {
            randomDirection = Vector2.right;
        }

        return (Vector2)player.position + randomDirection * spawnDistance;
    }

    private GameObject GetRandomEnemyPrefab()
    {
        int randomIndex = UnityEngine.Random.Range(0, enemyPrefabs.Length);
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

    private void ClearRoom()
    {
        // 중복 호출 방지
        if (isRoomCleared)
        {
            return;
        }

        isRoomCleared = true;
        isWaveActive = false;
        isWaitingNextWave = false;

        Debug.Log("Room Cleared");

        // 외부 시스템에 방 클리어 알림
        OnRoomCleared?.Invoke();
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }

    public int GetAliveEnemyCount()
    {
        return aliveEnemies.Count;
    }

    public bool IsRoomCleared()
    {
        return isRoomCleared;
    }

    public bool IsBattleStarted()
    {
        return isBattleStarted;
    }
}