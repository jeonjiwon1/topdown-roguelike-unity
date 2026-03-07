using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Projectile projectilePrefab;

    private void Update()
    {
        // 마우스 왼쪽 클릭 시 발사
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        // 탄환 생성
        Projectile projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // 발사 방향 전달
        projectile.Initialize(firePoint.right);
    }
}