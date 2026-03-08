using UnityEngine;

public class MagicArea : MonoBehaviour
{
    [Header("장판 설정")]
    [SerializeField] private float radius = 1.5f;
    [SerializeField] private float duration = 3f;
    [SerializeField] private float tickInterval = 0.5f;
    [SerializeField] private int damage = 1;

    [SerializeField] private LayerMask enemyLayer;

    private float tickTimer;

    private void Start()
    {
        // 일정 시간 후 장판 삭제
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        tickTimer += Time.deltaTime;

        if (tickTimer >= tickInterval)
        {
            tickTimer = 0f;
            DealDamage();
        }
    }

    private void DealDamage()
    {
        // 범위 내 적 탐지
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    // Scene view에서 장판 범위 표시
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}