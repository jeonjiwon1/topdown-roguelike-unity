using UnityEngine;

public class DroneAttackModule : MonoBehaviour
{
    [Header("필수 참조")]
    [SerializeField] private DroneController droneController;
    [SerializeField] private GameObject projectilePrefab;

    [Header("공격 설정")]
    [SerializeField] private float attackRange = 6f;
    [SerializeField] private float attackCooldown = 0.8f;
    [SerializeField] private float targetRefreshInterval = 0.2f;

    private float attackTimer;
    private float targetRefreshTimer;
    private Transform currentTarget;

    private void Awake()
    {
        // 같은 오브젝트의 DroneController 자동 연결
        if (droneController == null)
        {
            droneController = GetComponent<DroneController>();
        }
    }

    private void Update()
    {
        // 드론 컨트롤러 없으면 종료
        if (droneController == null)
        {
            return;
        }

        // Attack 모드가 아니면 공격 비활성
        if (droneController.GetCurrentMode() != DroneMode.Attack)
        {
            currentTarget = null;
            return;
        }

        attackTimer += Time.deltaTime;
        targetRefreshTimer += Time.deltaTime;

        // 일정 주기마다 타겟 다시 탐색
        if (targetRefreshTimer >= targetRefreshInterval)
        {
            targetRefreshTimer = 0f;
            currentTarget = FindClosestTarget();
        }

        // 타겟 없으면 종료
        if (currentTarget == null)
        {
            return;
        }

        // 사거리 밖이면 타겟 해제
        float distanceToTarget = Vector2.Distance(transform.position, currentTarget.position);

        if (distanceToTarget > attackRange)
        {
            currentTarget = null;
            return;
        }

        // 쿨타임마다 발사
        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            FireProjectile(currentTarget.position);
        }
    }

    private Transform FindClosestTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);

        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            // 부모까지 포함해서 EnemyHealth 찾기
            EnemyHealth enemyHealth = hit.GetComponentInParent<EnemyHealth>();

            if (enemyHealth == null)
            {
                continue;
            }

            float distance = Vector2.Distance(transform.position, enemyHealth.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = enemyHealth.transform;
            }
        }

        return closestTarget;
    }

    private void FireProjectile(Vector3 targetPosition)
    {
        // 프리팹 없으면 종료
        if (projectilePrefab == null)
        {
            return;
        }

        // 방향 계산
        Vector2 direction = (targetPosition - transform.position).normalized;

        // 투사체 생성
        GameObject projectileObject = Instantiate(
            projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        // Projectile 초기화
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Initialize(direction);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 공격 범위 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}