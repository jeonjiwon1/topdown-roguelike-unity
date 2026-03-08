using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;

    [Header("피격 피드백")]
    [SerializeField] private Color hitColor = Color.white;
    [SerializeField] private float hitFlashDuration = 0.08f;

    private int currentHealth;
    private EnemyAI enemyAI;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine hitFlashCoroutine;

    private void Awake()
    {
        // 체력 초기화
        currentHealth = maxHealth;

        // EnemyAI 가져오기
        enemyAI = GetComponent<EnemyAI>();

        // 스프라이트 렌더러 찾기
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // 원래 색 저장
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // 피격 피드백 실행
        PlayHitFeedback();

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

    private void PlayHitFeedback()
    {
        // 스프라이트가 없으면 종료
        if (spriteRenderer == null)
        {
            return;
        }

        // 기존 피격 코루틴이 있으면 중지
        if (hitFlashCoroutine != null)
        {
            StopCoroutine(hitFlashCoroutine);
        }

        // 새 피격 코루틴 시작
        hitFlashCoroutine = StartCoroutine(HitFlashRoutine());
    }

    private IEnumerator HitFlashRoutine()
    {
        // 피격 색으로 변경
        spriteRenderer.color = hitColor;

        // 잠깐 대기
        yield return new WaitForSeconds(hitFlashDuration);

        // 원래 색으로 복구
        spriteRenderer.color = originalColor;

        hitFlashCoroutine = null;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}