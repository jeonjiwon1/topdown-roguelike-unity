using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;

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
        if (enemyPrefab == null || player == null)
        {
            return;
        }

        // 랜덤 방향 계산
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        // 플레이어로부터 일정 거리 떨어진 위치 계산
        Vector2 spawnPosition = (Vector2)player.position + randomDirection * spawnDistance;

        // 적 생성
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}