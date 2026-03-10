using UnityEngine;

public class DroneBuffModule : MonoBehaviour
{
    [SerializeField] private DroneController droneController;
    [SerializeField] private PlayerWeaponController playerWeaponController;

    [Header("Buff 설정")]
    [SerializeField] private float attackBuffMultiplier = 1.3f;

    private bool buffApplied = false;

    private void Awake()
    {
        if (droneController == null)
        {
            droneController = GetComponent<DroneController>();
        }
    }

    private void Update()
    {
        if (droneController == null || playerWeaponController == null)
        {
            return;
        }

        // Buff 모드일 때
        if (droneController.GetCurrentMode() == DroneMode.Buff)
        {
            if (!buffApplied)
            {
                playerWeaponController.SetAttackMultiplier(attackBuffMultiplier);
                buffApplied = true;
            }
        }
        else
        {
            // Buff 모드가 아니면 원래대로
            if (buffApplied)
            {
                playerWeaponController.SetAttackMultiplier(1f);
                buffApplied = false;
            }
        }
    }
}