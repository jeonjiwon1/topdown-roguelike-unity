using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Projectile projectilePrefab;

    // 플레이어 무기 컨트롤러
    [SerializeField] private PlayerWeaponController weaponController;

    public void Shoot()
    {
        // 탄환 생성
        Projectile projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // 공격 배수 적용
        if (weaponController != null)
        {
            float multiplier = weaponController.GetAttackMultiplier();
            projectile.ApplyDamageMultiplier(multiplier);
        }

        // 발사 방향 전달
        projectile.Initialize(firePoint.right);
    }
}