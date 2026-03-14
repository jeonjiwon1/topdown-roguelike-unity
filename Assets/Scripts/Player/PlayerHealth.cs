using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private float invincibleTime = 1f;

    private int currentHealth;
    private float invincibleTimer;

    // 피격 이벤트
    public event Action OnDamaged;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (invincibleTimer > 0)
        {
            invincibleTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        if (invincibleTimer > 0)
        {
            return;
        }

        currentHealth -= damage;

        Debug.Log("Player HP : " + currentHealth);

        invincibleTimer = invincibleTime;

        // 피격 이벤트 호출
        OnDamaged?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 힐 함수
    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        Debug.Log("Heal → HP : " + currentHealth);
    }

    private void Die()
    {
        Debug.Log("Player Dead");
        Destroy(gameObject);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public bool IsFullHealth()
    {
        return currentHealth >= maxHealth;
    }
}