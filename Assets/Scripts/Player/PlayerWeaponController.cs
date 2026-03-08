using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("현재 무기")]
    [SerializeField] private PlayerWeaponType currentWeapon = PlayerWeaponType.Ranged;

    [Header("원거리 무기")]
    [SerializeField] private PlayerShooter playerShooter;

    [Header("근거리 무기")]
    [SerializeField] private MeleeWeapon meleeWeapon;

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
        // 1번 키 → 근접
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = PlayerWeaponType.Melee;
            Debug.Log("Weapon: Melee");
        }

        // 2번 키 → 원거리
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = PlayerWeaponType.Ranged;
            Debug.Log("Weapon: Ranged");
        }

        // 3번 키 → 마법
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeapon = PlayerWeaponType.Magic;
            Debug.Log("Weapon: Magic");
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
        // 아직 미구현
        Debug.Log("Magic Attack");
    }
}