using System;
using UnityEngine;

public class RoomExitPortal : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private Collider2D triggerCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("표시 설정")]
    [SerializeField] private bool startActive = false;
    [SerializeField] private Color activeColor = Color.cyan;
    [SerializeField] private Color inactiveColor = new Color(1f, 1f, 1f, 0.2f);
    [SerializeField] private bool hideWhenInactive = false;

    private bool isPortalActive;

    // 외부에서 포탈 진입 신호를 받을 수 있게 이벤트 제공
    public Action OnPlayerEnteredPortal;

    private void Awake()
    {
        ApplyState(startActive);
    }

    public void ActivatePortal()
    {
        ApplyState(true);
    }

    public void DeactivatePortal()
    {
        ApplyState(false);
    }

    public bool IsPortalActive()
    {
        return isPortalActive;
    }

    private void ApplyState(bool active)
    {
        isPortalActive = active;

        if (triggerCollider != null)
        {
            triggerCollider.enabled = active;
        }

        if (spriteRenderer != null)
        {
            if (hideWhenInactive)
            {
                spriteRenderer.enabled = active;
            }
            else
            {
                spriteRenderer.enabled = true;
                spriteRenderer.color = active ? activeColor : inactiveColor;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 포탈이 비활성 상태면 무시
        if (!isPortalActive)
        {
            return;
        }

        // 플레이어 태그가 아니면 무시
        if (!other.CompareTag("Player"))
        {
            return;
        }

        Debug.Log("RoomExitPortal : 플레이어가 포탈에 진입");

        OnPlayerEnteredPortal?.Invoke();
    }
}