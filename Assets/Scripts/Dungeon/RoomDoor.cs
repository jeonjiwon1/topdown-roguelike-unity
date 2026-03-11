using UnityEngine;

public class RoomDoor : MonoBehaviour
{
    [Header("문 상태")]
    [SerializeField] private bool startOpened = true;

    [Header("참조")]
    [SerializeField] private Collider2D doorCollider;
    [SerializeField] private SpriteRenderer doorRenderer;

    [Header("표시 설정")]
    [SerializeField] private Color openedColor = new Color(1f, 1f, 1f, 0.25f);
    [SerializeField] private Color closedColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private bool hideWhenOpened = false;

    private bool isOpened;

    private void Awake()
    {
        ApplyState(startOpened);
    }

    public void OpenDoor()
    {
        ApplyState(true);
    }

    public void CloseDoor()
    {
        ApplyState(false);
    }

    public bool IsOpened()
    {
        return isOpened;
    }

    private void ApplyState(bool opened)
    {
        isOpened = opened;

        if (doorCollider != null)
        {
            doorCollider.enabled = !opened;
        }

        if (doorRenderer != null)
        {
            if (hideWhenOpened)
            {
                doorRenderer.enabled = !opened;
            }
            else
            {
                doorRenderer.enabled = true;
                doorRenderer.color = opened ? openedColor : closedColor;
            }
        }
    }
}