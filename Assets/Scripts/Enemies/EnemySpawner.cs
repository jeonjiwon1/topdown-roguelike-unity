using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("스폰할 적 목록")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("대상")]
    [SerializeField] private Transform player;

    [Header("스폰 설정")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnDistance = 8f;

    private float spawnTimer;

    private void Update()
    {
        // 시간 누적
        spawnTimer += Time.deltaTime;

        // 스폰 시간 도달 시 적 생성
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemy();
        }
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

        // 랜덤 적 선택
        GameObject selectedEnemy = GetRandomEnemyPrefab();

        if (selectedEnemy == null)
        {
            return;
        }

        // 랜덤 방향 계산
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        // 0 벡터 방지
        if (randomDirection == Vector2.zero)
        {
            randomDirection = Vector2.right;
        }

        // 플레이어로부터 일정 거리 떨어진 위치 계산
        Vector2 spawnPosition = (Vector2)player.position + randomDirection * spawnDistance;

        // 적 생성
        Instantiate(selectedEnemy, spawnPosition, Quaternion.identity);
    }

    private GameObject GetRandomEnemyPrefab()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        return enemyPrefabs[randomIndex];
    }
}