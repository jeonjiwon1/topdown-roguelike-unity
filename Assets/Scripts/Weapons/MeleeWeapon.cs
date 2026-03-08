using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [Header("근접 공격 설정")]
    [SerializeField] private float attackRadius = 1.5f;
    [SerializeField] private int damage = 2;
    [SerializeField] private float knockbackForce = 6f;
    [SerializeField] private float knockbackDuration = 0.15f;
    [SerializeField] private float attackCooldown = 0.5f;

    private float lastAttackTime;

    [Header("레이어 설정")]
    [SerializeField] private LayerMask enemyLayer;

    public void Attack()
    {
        // 쿨타임 체크
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                // 적에게 피해 적용
                enemyHealth.TakeDamage(damage);
                // 넉백 방향 계산
                Vector2 direction = (hit.transform.position - transform.position).normalized;
                // 적에게 넉백 적용
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