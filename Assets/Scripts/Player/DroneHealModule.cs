using UnityEngine;

public class DroneHealModule : MonoBehaviour
{
    [SerializeField] private DroneController droneController;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Heal Settings")]
    [SerializeField] private float healInterval = 1f;
    [SerializeField] private int healAmount = 1;

    [Header("Penalty")]
    [SerializeField] private float speedMultiplier = 0.75f;

    [Header("Startup Delay")]
    [SerializeField] private float healStartDelay = 1f;

    private float healTimer;
    private float startTimer;
    private bool healActive;

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

    private void Update()
    {
        if (droneController.GetCurrentMode() != DroneMode.Heal)
        {
            StopHeal();
            return;
        }

        if (!healActive)
        {
            StartHeal();
        }

        startTimer += Time.deltaTime;

        if (startTimer < healStartDelay)
        {
            return;
        }

        healTimer += Time.deltaTime;

        if (healTimer >= healInterval)
        {
            healTimer = 0f;
            playerHealth.Heal(healAmount);
        }
    }

    private void StartHeal()
    {
        healActive = true;
        startTimer = 0f;
        healTimer = 0f;

        if (playerMovement != null)
        {
            playerMovement.SetSpeedMultiplier(speedMultiplier);
        }
    }

    private void StopHeal()
    {
        if (!healActive)
        {
            return;
        }

        healActive = false;

        if (playerMovement != null)
        {
            playerMovement.SetSpeedMultiplier(1f);
        }
    }

    private void OnPlayerDamaged()
    {
        StopHeal();
    }
}