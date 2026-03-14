using System.Collections;
using UnityEngine;

public class BossPatternController : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private BossShooter bossShooter;
    [SerializeField] private EnemyHealth enemyHealth;

    [Header("패턴 순환 설정")]
    [SerializeField] private float startDelay = 2f;
    [SerializeField] private float restBetweenPatterns = 1.5f;

    [Header("360도 패턴 설정")]
    [SerializeField] private int circleRepeatCount = 3;
    [SerializeField] private float circleInterval = 1.2f;

    [Header("연속 발사 패턴 설정")]
    [SerializeField] private float burstPatternRest = 2f;

    private Coroutine patternRoutine;

    private void Start()
    {
        if (bossShooter == null)
        {
            bossShooter = GetComponent<BossShooter>();
        }

        patternRoutine = StartCoroutine(PatternLoopRoutine());
    }

    private void OnDisable()
    {
        if (patternRoutine != null)
        {
            StopCoroutine(patternRoutine);
            patternRoutine = null;
        }

        if (bossShooter != null)
        {
            bossShooter.StopBurstFire();
        }
    }

    private IEnumerator PatternLoopRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            // 패턴 A : 360도 탄막
            for (int i = 0; i < circleRepeatCount; i++)
            {
                if (!IsBossAlive())
                {
                    yield break;
                }

                bossShooter.FireCircle();
                yield return new WaitForSeconds(circleInterval);
            }

            yield return new WaitForSeconds(restBetweenPatterns);

            // 패턴 B : 플레이어 추적 연속 발사
            if (!IsBossAlive())
            {
                yield break;
            }

            bossShooter.StartBurstFire();
            yield return new WaitForSeconds(burstPatternRest + GetEstimatedBurstDuration());

            yield return new WaitForSeconds(restBetweenPatterns);
        }
    }

    private bool IsBossAlive()
    {
        return enemyHealth != null && enemyHealth.gameObject != null;
    }

    private float GetEstimatedBurstDuration()
    {
        return 3f;
    }
}