using UnityEngine;

public class SpawnIndicator : MonoBehaviour
{
    [Header("표시 시간")]
    [SerializeField] private float lifeTime = 0.8f;

    [Header("크기 변화")]
    [SerializeField] private float startScale = 0.4f;
    [SerializeField] private float endScale = 1.2f;

    [Header("깜빡임")]
    [SerializeField] private bool useBlink = true;
    [SerializeField] private float blinkSpeed = 12f;

    private SpriteRenderer spriteRenderer;
    private float timer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 시작 크기 설정
        transform.localScale = Vector3.one * startScale;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float t = Mathf.Clamp01(timer / lifeTime);

        // 크기 점점 증가
        float currentScale = Mathf.Lerp(startScale, endScale, t);
        transform.localScale = Vector3.one * currentScale;

        // 깜빡임 처리
        if (useBlink && spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(0.2f, 0.9f, (Mathf.Sin(Time.time * blinkSpeed) + 1f) * 0.5f);
            spriteRenderer.color = color;
        }

        // 시간 끝나면 제거
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    // 외부에서 표시 시간 설정 가능
    public void SetLifeTime(float value)
    {
        lifeTime = value;
    }
}