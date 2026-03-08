using UnityEngine;

public class MagicWeapon : MonoBehaviour
{
    [Header("마법 장판")]
    [SerializeField] private GameObject magicAreaPrefab;

    [Header("사거리")]
    [SerializeField] private float maxRange = 4f;

    [Header("스택 시스템")]
    [SerializeField] private int maxStacks = 2;
    [SerializeField] private float rechargeTime = 4f;

    private int currentStacks;
    private float rechargeTimer;

    private void Start()
    {
        currentStacks = maxStacks;
    }

    private void Update()
    {
        RechargeStack();
    }

    private void RechargeStack()
    {
        if (currentStacks >= maxStacks)
            return;

        rechargeTimer += Time.deltaTime;

        if (rechargeTimer >= rechargeTime)
        {
            rechargeTimer = 0f;
            currentStacks++;
        }
    }

    public void Attack(Vector2 targetPosition)
    {
        if (currentStacks <= 0)
        {
            Debug.Log("No Magic Stacks");
            return;
        }

        // 사거리 체크
        float distance = Vector2.Distance(transform.position, targetPosition);

        if (distance > maxRange)
        {
            Debug.Log("Out of Range");
            return;
        }

        // 장판 생성
        Instantiate(magicAreaPrefab, targetPosition, Quaternion.identity);

        currentStacks--;

        Debug.Log("Magic Cast");
    }
}