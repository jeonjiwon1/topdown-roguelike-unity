using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Detection")]
    [SerializeField] private float chaseRange = 8f;
    [SerializeField] private float attackRange = 1.5f;

    private Transform player;
    private Rigidbody2D rb;

    private EnemyState currentState;

    private void Awake()
    {
        // 리지드바디 가져오기
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 플레이어 찾기
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // 초기 상태
        currentState = EnemyState.Idle;
    }

    private void Update()
    {
        if (player == null) return;

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
        if (currentState == EnemyState.Chase)
        {
            MoveToPlayer();
        }
    }

    private void UpdateIdle(float distance)
    {
        if (distance <= chaseRange)
        {
            currentState = EnemyState.Chase;
        }
    }

    private void UpdateChase(float distance)
    {
        if (distance <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
        else if (distance > chaseRange)
        {
            currentState = EnemyState.Idle;
        }
    }

    private void UpdateAttack(float distance)
    {
        rb.linearVelocity = Vector2.zero;

        if (distance > attackRange)
        {
            currentState = EnemyState.Chase;
        }
    }

    private void MoveToPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    // 플레이어와 충돌 시 데미지 처리
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(1);
        }
    }
}