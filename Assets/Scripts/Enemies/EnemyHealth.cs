using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;

    private int currentHealth;
    private EnemyAI enemyAI;

    private void Awake()
    {
        // 체력 초기화
        currentHealth = maxHealth;

        // EnemyAI 가져오기
        enemyAI = GetComponent<EnemyAI>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ApplyKnockback(Vector2 hitDirection, float knockbackForce, float knockbackDuration)
    {
        // EnemyAI가 있으면 넉백 전달
        if (enemyAI != null)
        {
            enemyAI.ApplyKnockback(hitDirection, knockbackForce, knockbackDuration);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}