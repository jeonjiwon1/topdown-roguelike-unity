using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("현재 무기")]
    [SerializeField] private PlayerWeaponType currentWeapon = PlayerWeaponType.Melee;

    [Header("원거리 무기")]
    [SerializeField] private PlayerShooter playerShooter;

    [Header("근거리 무기")]
    [SerializeField] private MeleeWeapon meleeWeapon;

    [Header("마법 무기")]
    [SerializeField] private MagicWeapon magicWeapon;

    private PlayerWeaponType[] weaponOrder =
    {
        PlayerWeaponType.Melee,
        PlayerWeaponType.Ranged,
        PlayerWeaponType.Magic
    };

    // 현재 무기 인덱스 (Q/E로 무기 변경 시 사용)
    private int currentWeaponIndex = 0;

    // 플레이어 공격 배수 (버프/디버프용)
    private float attackMultiplier = 1f;

    private void Start()
    {
        // 시작 무기에 맞는 인덱스 설정
        SetWeaponIndexFromCurrentWeapon();
        LogCurrentWeapon();
    }

    private void Update()
    {
        HandleWeaponSwitch();

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void HandleWeaponSwitch()
    {
        // Q → 이전 무기
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentWeaponIndex--;

            if (currentWeaponIndex < 0)
            {
                currentWeaponIndex = weaponOrder.Length - 1;
            }

            currentWeapon = weaponOrder[currentWeaponIndex];
            LogCurrentWeapon();
        }

        // E → 다음 무기
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentWeaponIndex++;

            if (currentWeaponIndex >= weaponOrder.Length)
            {
                currentWeaponIndex = 0;
            }

            currentWeapon = weaponOrder[currentWeaponIndex];
            LogCurrentWeapon();
        }
    }

    private void Attack()
    {
        switch (currentWeapon)
        {
            case PlayerWeaponType.Melee:
                MeleeAttack();
                break;

            case PlayerWeaponType.Ranged:
                RangedAttack();
                break;

            case PlayerWeaponType.Magic:
                MagicAttack();
                break;
        }
    }

    private void MeleeAttack()
    {
        if (meleeWeapon != null)
        {
            meleeWeapon.Attack();
        }
    }

    private void RangedAttack()
    {
        // 기존 총 시스템 사용
        if (playerShooter != null)
        {
            playerShooter.Shoot();
        }
    }

    private void MagicAttack()
    {
        if (magicWeapon != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            magicWeapon.Attack(mousePos);
        }
    }

    private void SetWeaponIndexFromCurrentWeapon()
    {
        for (int i = 0; i < weaponOrder.Length; i++)
        {
            if (weaponOrder[i] == currentWeapon)
            {
                currentWeaponIndex = i;
                return;
            }
        }

        // 혹시 못 찾으면 기본값으로 설정
        currentWeaponIndex = 0;
        currentWeapon = weaponOrder[currentWeaponIndex];
    }

    private void LogCurrentWeapon()
    {
        Debug.Log("Weapon: " + currentWeapon);
    }

    // 공격 배수 설정
    public void SetAttackMultiplier(float multiplier)
    {
        attackMultiplier = multiplier;
    }

    // 공격 배수 가져오기
    public float GetAttackMultiplier()
    {
        return attackMultiplier;
    }
}