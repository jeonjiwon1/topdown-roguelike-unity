using UnityEngine;

public class HealStation : InteractableBase
{
    [Header("회복 설정")]
    [SerializeField] private int healAmount = 3;
    [SerializeField] private bool oneTimeUse = true;

    [Header("시각 처리")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color usedColor = Color.gray;

    private bool hasUsed;

    private void Reset()
    {
        interactPrompt = "Press Space to Heal";
    }

    public override void Interact(GameObject interactor)
    {
        if (hasUsed && oneTimeUse)
        {
            Debug.Log("HealStation : 이미 사용함");
            return;
        }

        PlayerHealth playerHealth = interactor.GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.Log("HealStation : PlayerHealth를 찾지 못함");
            return;
        }

        playerHealth.Heal(healAmount);
        Debug.Log("HealStation : 플레이어 회복 " + healAmount);

        if (oneTimeUse)
        {
            hasUsed = true;
            interactPrompt = "Used";

            if (spriteRenderer != null)
            {
                spriteRenderer.color = usedColor;
            }
        }
    }
}