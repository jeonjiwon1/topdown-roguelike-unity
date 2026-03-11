using UnityEngine;

public class RoomBattleController : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private RoomDoor[] roomDoors;

    [Header("전투 시작 설정")]
    [SerializeField] private bool closeDoorsOnStartBattle = true;
    [SerializeField] private bool autoStartBattle = true;

    private bool hasSubscribed;
    private bool hasBattleStarted;

    private void Start()
    {
        SubscribeRoomClearEvent();

        // 시작 상태 정리
        OpenAllDoors();

        if (autoStartBattle)
        {
            StartBattle();
        }
    }

    private void OnDestroy()
    {
        UnsubscribeRoomClearEvent();
    }

    private void SubscribeRoomClearEvent()
    {
        if (enemySpawner == null || hasSubscribed)
        {
            return;
        }

        enemySpawner.OnRoomCleared += HandleRoomCleared;
        hasSubscribed = true;
    }

    private void UnsubscribeRoomClearEvent()
    {
        if (enemySpawner == null || !hasSubscribed)
        {
            return;
        }

        enemySpawner.OnRoomCleared -= HandleRoomCleared;
        hasSubscribed = false;
    }

    public void StartBattle()
    {
        if (hasBattleStarted)
        {
            return;
        }

        hasBattleStarted = true;

        if (closeDoorsOnStartBattle)
        {
            CloseAllDoors();
        }

        if (enemySpawner != null && !enemySpawner.IsBattleStarted())
        {
            enemySpawner.StartRoomBattle();
        }
    }

    private void HandleRoomCleared()
    {
        OpenAllDoors();
    }

    public void OpenAllDoors()
    {
        if (roomDoors == null)
        {
            return;
        }

        for (int i = 0; i < roomDoors.Length; i++)
        {
            if (roomDoors[i] != null)
            {
                roomDoors[i].OpenDoor();
            }
        }
    }

    public void CloseAllDoors()
    {
        if (roomDoors == null)
        {
            return;
        }

        for (int i = 0; i < roomDoors.Length; i++)
        {
            if (roomDoors[i] != null)
            {
                roomDoors[i].CloseDoor();
            }
        }
    }
}