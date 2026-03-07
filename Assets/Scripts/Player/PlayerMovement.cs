using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        // 리지드바디 가져오기
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 입력값 초기화
        moveInput = Vector2.zero;

        // W/S 입력
        if (Keyboard.current.wKey.isPressed)
        {
            moveInput.y += 1f;
        }

        if (Keyboard.current.sKey.isPressed)
        {
            moveInput.y -= 1f;
        }

        // A/D 입력
        if (Keyboard.current.aKey.isPressed)
        {
            moveInput.x -= 1f;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            moveInput.x += 1f;
        }

        // 대각선 속도 보정
        moveInput = moveInput.normalized;
    }

    private void FixedUpdate()
    {
        // 실제 이동 처리
        rb.linearVelocity = moveInput * moveSpeed;
    }
}