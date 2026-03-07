using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    private Transform player;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 플레이어 찾기
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            return;
        }

        // 플레이어 방향 계산
        Vector2 direction = (player.position - transform.position).normalized;

        // 이동
        rb.linearVelocity = direction * moveSpeed;
    }
}