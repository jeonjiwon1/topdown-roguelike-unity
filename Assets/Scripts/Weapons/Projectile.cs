using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private int damage = 1;

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    private void Awake()
    {
        // 리지드바디 가져오기
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 일정 시간 뒤 탄환 제거
        Destroy(gameObject, lifeTime);
    }

    public void Initialize(Vector2 direction)
    {
        // 이동 방향 저장
        moveDirection = direction.normalized;
    }

    private void FixedUpdate()
    {
        // 탄환 이동
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    // 적과 충돌 시 데미지 적용
    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}