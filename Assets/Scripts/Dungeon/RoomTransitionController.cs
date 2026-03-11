using UnityEngine;

public class RoomTransitionController : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private RoomExitPortal exitPortal;

    [Header("테스트용 설정")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform nextSpawnPoint;
    [SerializeField] private bool movePlayerOnPortalEnter = false;

    private bool hasSubscribed;

    private void Start()
    {
        SubscribeEvents();

        if (exitPortal != null)
        {
            // 시작 시 포탈 비활성
            exitPortal.DeactivatePortal();
        }
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        if (!hasSubscribed)
        {
            if (enemySpawner != null)
            {
                enemySpawner.OnRoomCleared += HandleRoomCleared;
            }

            if (exitPortal != null)
            {
                exitPortal.OnPlayerEnteredPortal += HandlePlayerEnteredPortal;
            }

            hasSubscribed = true;
        }
    }

    private void UnsubscribeEvents()
    {
        if (!hasSubscribed)
        {
            return;
        }

        if (enemySpawner != null)
        {
            enemySpawner.OnRoomCleared -= HandleRoomCleared;
        }

        if (exitPortal != null)
        {
            exitPortal.OnPlayerEnteredPortal -= HandlePlayerEnteredPortal;
        }

        hasSubscribed = false;
    }

    private void HandleRoomCleared()
    {
        Debug.Log("RoomTransitionController : 방 클리어로 포탈 활성화");

        if (exitPortal != null)
        {
            exitPortal.ActivatePortal();
        }
    }

    private void HandlePlayerEnteredPortal()
    {
        Debug.Log("RoomTransitionController : 다음 방 이동 처리");

        // 현재는 테스트용으로 플레이어 위치 이동만 지원
        if (movePlayerOnPortalEnter && player != null && nextSpawnPoint != null)
        {
            player.position = nextSpawnPoint.position;
        }

        // 나중에 여기에 다음 방 생성 / 씬 전환 / 포탈 선택 로직 연결
    }
}