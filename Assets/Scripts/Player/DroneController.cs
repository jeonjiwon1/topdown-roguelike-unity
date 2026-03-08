using UnityEngine;

public class DroneController : MonoBehaviour
{
    [Header("추적 대상")]
    [SerializeField] private Transform player;

    [Header("이동 설정")]
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(1.5f, 1f, 0f);

    [Header("현재 드론 모드")]
    [SerializeField] private DroneMode currentMode = DroneMode.Heal;

    private void Start()
    {
        // 플레이어가 연결되지 않았으면 자동 찾기
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
    }

    private void LateUpdate()
    {
        FollowPlayer();
    }

    private void HandleModeSwitch()
    {
        // 1 → 힐 모드
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentMode = DroneMode.Heal;
            LogCurrentMode();
        }

        // 2 → 버프 모드
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentMode = DroneMode.Buff;
            LogCurrentMode();
        }

        // 3 → 공격 모드
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentMode = DroneMode.Attack;
            LogCurrentMode();
        }
    }

    private void FollowPlayer()
    {
        if (player == null)
        {
            return;
        }

        // 목표 위치 계산
        Vector3 targetPosition = player.position + offset;

        // 부드럽게 따라가기
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
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