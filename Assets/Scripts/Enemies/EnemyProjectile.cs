using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 7f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private int damage = 1;

    private Vector2 moveDirection;

    public void Initialize(Vector2 direction)
    {
        // 이동 방향 저장
        moveDirection = direction.normalized;

        // 일정 시간 뒤 삭제
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // 탄환 이동
        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 체력 찾기
        PlayerHealth player = collision.GetComponent<PlayerHealth>();

        if (player != null)
        {
            // 플레이어에게 데미지
            player.TakeDamage(damage);

            // 탄환 제거
            Destroy(gameObject);
        }
    }

    // 외부에서 이동 방향 설정
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }
}