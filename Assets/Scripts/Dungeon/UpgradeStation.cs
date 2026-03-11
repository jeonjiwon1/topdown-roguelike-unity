using UnityEngine;

public class UpgradeStation : InteractableBase
{
    [Header("임시 업그레이드 설정")]
    [SerializeField] private bool oneTimeUse = true;

    [Header("시각 처리")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color usedColor = Color.gray;

    private bool hasUsed;

    private void Reset()
    {
        interactPrompt = "Press Space to Upgrade";
    }

    public override void Interact(GameObject interactor)
    {
        if (hasUsed && oneTimeUse)
        {
            Debug.Log("UpgradeStation : 이미 사용함");
            return;
        }

        Debug.Log("UpgradeStation : 임시 업그레이드 실행");

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