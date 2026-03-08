using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [Header("근접 공격 설정")]
    [SerializeField] private float attackRadius = 1.5f;
    [SerializeField] private int damage = 2;
    [SerializeField] private float knockbackForce = 6f;
    [SerializeField] private float knockbackDuration = 0.15f;

    [Header("레이어 설정")]
    [SerializeField] private LayerMask enemyLayer;

    public void Attack()
    {
        // 플레이어 위치 기준으로 원형 범위 안의 적 찾기
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                // 데미지 적용
                enemyHealth.TakeDamage(damage);

                // 넉백 방향 계산
                Vector2 direction = (hit.transform.position - transform.position).normalized;

                // 넉백 적용
                enemyHealth.ApplyKnockback(direction, knockbackForce, knockbackDuration);
            }
        }

        Debug.Log("Melee Attack");
    }

    // 공격 범위 확인용 (Scene view에서 보임)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}