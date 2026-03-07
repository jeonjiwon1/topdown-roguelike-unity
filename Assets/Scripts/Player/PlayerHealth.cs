using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private float invincibleTime = 1f;

    private int currentHealth;
    private float invincibleTimer;

    private void Awake()
    {
        // 체력 초기화
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // 무적 시간 감소
        if (invincibleTimer > 0)
        {
            invincibleTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        // 무적 상태면 무시
        if (invincibleTimer > 0)
        {
            return;
        }

        currentHealth -= damage;

        Debug.Log("Player HP : " + currentHealth);

        invincibleTimer = invincibleTime;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Dead");
        Destroy(gameObject);
    }
}