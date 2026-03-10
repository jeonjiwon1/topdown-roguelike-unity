using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private int damage = 1;

    [Header("넉백")]
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackDuration = 0.08f;

    private Vector2 moveDirection;

    public void Initialize(Vector2 direction)
    {
        // 발사 방향 저장
        moveDirection = direction.normalized;

        // 일정 시간 뒤 자동 삭제
        Destroy(gameObject, lifetime);
    }

    // 데미지 배수 적용
    public void ApplyDamageMultiplier(float multiplier)
    {
        damage = Mathf.RoundToInt(damage * multiplier);
    }

    private void Update()
    {
        // 탄환 이동
        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            enemy.ApplyKnockback(moveDirection, knockbackForce, knockbackDuration);
            Destroy(gameObject);
        }
    }
}