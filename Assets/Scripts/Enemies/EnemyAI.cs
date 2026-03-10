using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("이동")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("감지")]
    [SerializeField] private float chaseRange = 8f;
    [SerializeField] private float attackRange = 1.5f;

    [Header("공격")]
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackCooldown = 1f;

    [Header("적 타입 설정")]
    [SerializeField] private bool isSuicideEnemy = false;
    [SerializeField] private bool isRangedEnemy = false;

    [Header("원거리 적 거리 유지")]
    [SerializeField] private float preferredRange = 5f;
    [SerializeField] private float rangeTolerance = 0.5f;

    [Header("넉백 저항")]
    [SerializeField] private float knockbackMultiplier = 1f;

    private Transform player;
    private PlayerHealth playerHealth;
    private Rigidbody2D rb;

    private EnemyState currentState;
    private float lastAttackTime;

    private bool isKnockedBack;
    private float knockbackTimer;
    private Vector2 knockbackVelocity;

    private void Awake()
    {
        // 리지드바디 가져오기
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 플레이어 찾기
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>();
        }

        // 시작 상태
        currentState = EnemyState.Idle;

        // 시작 직후 공격 가능하게 설정
        lastAttackTime = -attackCooldown;
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        // 넉백 중이면 상태 업데이트 중단
        if (isKnockedBack)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Idle:
                UpdateIdle(distance);
                break;

            case EnemyState.Chase:
                UpdateChase(distance);
                break;

            case EnemyState.Attack:
                UpdateAttack(distance);
                break;
        }
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // 넉백 중이면 넉백 속도 적용
        if (isKnockedBack)
        {
            rb.linearVelocity = knockbackVelocity;

            // 넉백 시간 감소
            knockbackTimer -= Time.fixedDeltaTime;

            // 넉백 종료 처리
            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
                knockbackTimer = 0f;
                knockbackVelocity = Vector2.zero;
                rb.linearVelocity = Vector2.zero;
            }

            return;
        }

        if (currentState == EnemyState.Chase)
        {
            if (isRangedEnemy)
            {
                MoveForRangedEnemy();
            }
            else
            {
                MoveToPlayer();
            }
        }
        else
        {
            // 추적 상태가 아니면 멈춤
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void UpdateIdle(float distance)
    {
        // 추적 범위 안이면 추적으로 전환
        if (distance <= chaseRange)
        {
            currentState = EnemyState.Chase;
        }
    }

    private void UpdateChase(float distance)
    {
        // 원거리 적은 근접 공격 상태로 가지 않음
        if (!isRangedEnemy && distance <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
        // 너무 멀어지면 대기로 전환
        else if (distance > chaseRange)
        {
            currentState = EnemyState.Idle;
        }
    }

    private void UpdateAttack(float distance)
    {
        // 원거리 적이면 공격 상태 사용 안 함
        if (isRangedEnemy)
        {
            currentState = EnemyState.Chase;
            return;
        }

        // 공격 범위 밖이면 다시 추적으로 전환
        if (distance > attackRange)
        {
            currentState = EnemyState.Chase;
            return;
        }

        // 쿨타임마다 공격
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            AttackPlayer();
        }
    }

    private void MoveToPlayer()
    {
        // 플레이어 방향으로 이동
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    private void MoveForRangedEnemy()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        Vector2 direction = (player.position - transform.position).normalized;

        float minRange = preferredRange - rangeTolerance;
        float maxRange = preferredRange + rangeTolerance;

        // 너무 멀면 접근
        if (distance > maxRange)
        {
            rb.linearVelocity = direction * moveSpeed;
        }
        // 너무 가까우면 뒤로 후퇴
        else if (distance < minRange)
        {
            rb.linearVelocity = -direction * moveSpeed;
        }
        // 적정 거리면 정지
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void AttackPlayer()
    {
        // 플레이어 체력이 없으면 종료
        if (playerHealth == null)
        {
            return;
        }

        // 플레이어에게 데미지 적용
        playerHealth.TakeDamage(attackDamage);
        lastAttackTime = Time.time;

        // 자폭 적이면 공격 후 즉시 사망
        if (isSuicideEnemy)
        {
            Destroy(gameObject);
        }
    }

    public void ApplyKnockback(Vector2 hitDirection, float force, float duration)
    {
        // 넉백 배수 적용
        float finalForce = force * knockbackMultiplier;

        // 넉백이 거의 없으면 무시
        if (finalForce <= 0.01f)
        {
            return;
        }

        // 넉백 시작
        isKnockedBack = true;
        knockbackTimer = duration;
        knockbackVelocity = hitDirection.normalized * finalForce;
    }
}