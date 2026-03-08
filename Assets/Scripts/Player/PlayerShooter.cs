using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Projectile projectilePrefab;

    public void Shoot()
    {
        // 탄환 생성
        Projectile projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // 발사 방향 전달
        projectile.Initialize(firePoint.right);
    }
}