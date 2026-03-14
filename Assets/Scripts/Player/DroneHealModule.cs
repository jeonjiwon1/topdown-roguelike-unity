using UnityEngine;

public class DroneHealModule : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private DroneController droneController;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("힐 발동 키")]
    [SerializeField] private KeyCode healKey = KeyCode.R;

    [Header("배터리 설정")]
    [SerializeField] private int maxBatteryCount = 3;
    [SerializeField] private int currentBatteryCount = 3;
    [SerializeField] private int batteryCostPerUse = 1;

    [Header("힐 설정")]
    [SerializeField] private float healDuration = 5f;
    [SerializeField] private float healInterval = 1f;
    [SerializeField] private int healAmountPerTick = 3;

    [Header("패널티")]
    [SerializeField] private float speedMultiplierWhileHealing = 0.85f;

    private float healDurationTimer;
    private float healTickTimer;
    private bool isHealing;

    private void Awake()
    {
        if (droneController == null)
        {
            droneController = GetComponent<DroneController>();
        }

        if (playerHealth != null)
        {
            playerHealth.OnDamaged += OnPlayerDamaged;
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDamaged -= OnPlayerDamaged;
        }
    }

    private void Update()
    {
        if (droneController == null || playerHealth == null)
        {
            return;
        }

        // 힐 모드가 아니면 힐 강제 종료
        if (droneController.GetCurrentMode() != DroneMode.Heal)
        {
            StopHealing();
            return;
        }

        // 힐 시작 입력
        if (!isHealing && Input.GetKeyDown(healKey))
        {
            TryStartHealing();
        }

        // 힐 진행
        if (isHealing)
        {
            UpdateHealing();
        }
    }

    private void TryStartHealing()
    {
        if (currentBatteryCount < batteryCostPerUse)
        {
            Debug.Log("Drone Heal : 배터리 부족");
            return;
        }

        if (playerHealth.IsFullHealth())
        {
            Debug.Log("Drone Heal : 체력이 이미 가득 참");
            return;
        }

        currentBatteryCount -= batteryCostPerUse;

        isHealing = true;
        healDurationTimer = 0f;
        healTickTimer = 0f;

        if (playerMovement != null)
        {
            playerMovement.SetSpeedMultiplier(speedMultiplierWhileHealing);
        }

        Debug.Log("Drone Heal Start / Battery Left : " + currentBatteryCount);
    }

    private void UpdateHealing()
    {
        healDurationTimer += Time.deltaTime;
        healTickTimer += Time.deltaTime;

        if (healTickTimer >= healInterval)
        {
            healTickTimer = 0f;

            if (!playerHealth.IsFullHealth())
            {
                playerHealth.Heal(healAmountPerTick);
            }
        }

        // 지속 시간 종료 또는 풀피면 종료
        if (healDurationTimer >= healDuration || playerHealth.IsFullHealth())
        {
            StopHealing();
        }
    }

    private void StopHealing()
    {
        if (!isHealing)
        {
            return;
        }

        isHealing = false;
        healDurationTimer = 0f;
        healTickTimer = 0f;

        if (playerMovement != null)
        {
            playerMovement.SetSpeedMultiplier(1f);
        }

        Debug.Log("Drone Heal Stop");
    }

    private void OnPlayerDamaged()
    {
        if (isHealing)
        {
            Debug.Log("Drone Heal Cancel : 피격으로 중단");
            StopHealing();
        }
    }

    public void AddBattery(int amount)
    {
        currentBatteryCount += amount;
        currentBatteryCount = Mathf.Clamp(currentBatteryCount, 0, maxBatteryCount);

        Debug.Log("Battery Added / Current : " + currentBatteryCount);
    }

    public bool IsHealing()
    {
        return isHealing;
    }

    public int GetCurrentBatteryCount()
    {
        return currentBatteryCount;
    }

    public int GetMaxBatteryCount()
    {
        return maxBatteryCount;
    }
}