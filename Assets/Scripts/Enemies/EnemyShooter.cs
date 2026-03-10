using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("발사")]
    [SerializeField] private EnemyProjectile projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("공격")]
    [SerializeField] private float attackRange = 6f;
    [SerializeField] private float attackCooldown = 1.5f;

    private Transform player;
    private float attackTimer;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        if (player == null || firePoint == null || projectilePrefab == null)
        {
            return;
        }

        attackTimer += Time.deltaTime;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {
        if (attackTimer < attackCooldown)
        {
            return;
        }

        attackTimer = 0f;
        Shoot();
    }

    private void Shoot()
    {
        Vector2 direction = (player.position - firePoint.position).normalized;

        EnemyProjectile projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        projectile.Initialize(direction);
    }
}