using UnityEngine;

public class DroneController : MonoBehaviour
{
    [Header("추적 대상")]
    [SerializeField] private Transform player;

    [Header("이동 설정")]
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float distance = 1.8f;

    [Header("드론 움직임")]
    [SerializeField] private float swingAngle = 45f;   // 좌우 최대 각도
    [SerializeField] private float swingSpeed = 1f;    // 왕복 속도

    [Header("현재 드론 모드")]
    [SerializeField] private DroneMode currentMode = DroneMode.Heal;

    private float swingTimer;

    private void Start()
    {
        // 플레이어 자동 찾기
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        LogCurrentMode();
    }

    private void Update()
    {
        HandleModeSwitch();

        // 왕복 타이머
        swingTimer += Time.deltaTime * swingSpeed;
    }

    private void LateUpdate()
    {
        FollowPlayer();
    }

    private void HandleModeSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentMode = DroneMode.Heal;
            LogCurrentMode();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentMode = DroneMode.Buff;
            LogCurrentMode();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentMode = DroneMode.Attack;
            LogCurrentMode();
        }
    }

    private void FollowPlayer()
    {
        if (player == null) return;

        // 플레이어 보는 방향
        Vector2 forward = player.right;

        // 등 방향
        Vector2 back = -forward;

        // -1 ~ 1 사이 값으로 흔들림
        float swing = Mathf.Sin(swingTimer);

        // -45도 ~ +45도 회전
        float angle = swing * swingAngle;

        // 등 방향을 기준으로 각도 회전
        Vector2 rotatedDirection = Quaternion.Euler(0, 0, angle) * back;

        // 목표 위치 계산
        Vector3 targetPosition = player.position + (Vector3)(rotatedDirection.normalized * distance);

        // 부드럽게 이동
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }

    private void LogCurrentMode()
    {
        Debug.Log("Drone Mode: " + currentMode);
    }

    public DroneMode GetCurrentMode()
    {
        return currentMode;
    }
}