using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        // 메인 카메라 저장
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // 마우스가 없으면 종료
        if (Mouse.current == null)
        {
            return;
        }

        // 마우스 스크린 좌표 가져오기
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

        // 월드 좌표로 변환
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        // 플레이어에서 마우스 방향 계산
        Vector2 aimDirection = mouseWorldPosition - transform.position;

        // 각도 계산
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        // 회전 적용
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}