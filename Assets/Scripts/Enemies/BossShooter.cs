using System.Collections;
using UnityEngine;

public class BossShooter : MonoBehaviour
{
    [Header("공통 참조")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform player;

    [Header("360도 탄막 설정")]
    [SerializeField] private int circleBulletCount = 16;
    [SerializeField] private float circleSpreadAngle = 360f;

    [Header("연속 발사 설정")]
    [SerializeField] private int burstShotCount = 30;
    [SerializeField] private float burstInterval = 0.08f;
    [SerializeField] private float aimRandomAngle = 4f;

    private Coroutine burstRoutine;

    private void Awake()
    {
        if (firePoint == null)
        {
            firePoint = transform;
        }

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    public void FireCircle()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            return;
        }

        float angleStep = circleSpreadAngle / circleBulletCount;
        float startAngle = 0f;

        for (int i = 0; i < circleBulletCount; i++)
        {
            float angle = startAngle + angleStep * i;
            SpawnProjectileByAngle(angle);
        }

        Debug.Log("Boss Pattern : 360 Fire");
    }

    public void StartBurstFire()
    {
        if (burstRoutine != null)
        {
            StopCoroutine(burstRoutine);
        }

        burstRoutine = StartCoroutine(BurstFireRoutine());
    }

    public void StopBurstFire()
    {
        if (burstRoutine != null)
        {
            StopCoroutine(burstRoutine);
            burstRoutine = null;
        }
    }

    private IEnumerator BurstFireRoutine()
    {
        if (projectilePrefab == null || firePoint == null || player == null)
        {
            yield break;
        }

        Debug.Log("Boss Pattern : Burst Fire Start");

        for (int i = 0; i < burstShotCount; i++)
        {
            if (player == null)
            {
                yield break;
            }

            Vector2 direction = ((Vector2)player.position - (Vector2)firePoint.position).normalized;

            float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float randomOffset = Random.Range(-aimRandomAngle, aimRandomAngle);
            float finalAngle = baseAngle + randomOffset;

            SpawnProjectileByAngle(finalAngle);

            yield return new WaitForSeconds(burstInterval);
        }

        burstRoutine = null;

        Debug.Log("Boss Pattern : Burst Fire End");
    }

    private void SpawnProjectileByAngle(float angle)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Vector2 direction = AngleToDirection(angle);

        EnemyProjectile enemyProjectile = projectile.GetComponent<EnemyProjectile>();
        if (enemyProjectile != null)
        {
            enemyProjectile.SetDirection(direction);
        }
        else
        {
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float speed = 6f;
                rb.linearVelocity = direction * speed;
            }
        }
    }

    private Vector2 AngleToDirection(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
    }
}