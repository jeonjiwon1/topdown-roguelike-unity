using UnityEngine;

public class RoomSetupController : MonoBehaviour
{
    [Header("Room 설정")]
    [SerializeField] private RoomType roomType = RoomType.Combat;

    [Header("전투 관련")]
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private RoomBattleController roomBattleController;

    [Header("정비 관련")]
    [SerializeField] private GameObject[] restRoomObjects;

    [Header("보상 / 증강 관련")]
    [SerializeField] private GameObject[] rewardRoomObjects;

    [Header("전투 전용 오브젝트")]
    [SerializeField] private GameObject[] combatRoomObjects;

    private void Start()
    {
        ApplyRoomSetup();
    }

    public void ApplyRoomSetup()
    {
        // 전부 기본 비활성 처리 후 필요한 것만 켜기
        SetObjectsActive(restRoomObjects, false);
        SetObjectsActive(rewardRoomObjects, false);
        SetObjectsActive(combatRoomObjects, false);

        switch (roomType)
        {
            case RoomType.Combat:
                SetupCombatRoom();
                break;

            case RoomType.Rest:
                SetupRestRoom();
                break;

            case RoomType.Elite:
                SetupEliteRoom();
                break;

            case RoomType.Boss:
                SetupBossRoom();
                break;

            case RoomType.Augment:
                SetupAugmentRoom();
                break;
        }
    }

    private void SetupCombatRoom()
    {
        SetObjectsActive(combatRoomObjects, true);

        if (enemySpawner != null)
        {
            enemySpawner.gameObject.SetActive(true);
        }

        if (roomBattleController != null)
        {
            roomBattleController.gameObject.SetActive(true);
        }

        Debug.Log("Room Setup : Combat Room");
    }

    private void SetupRestRoom()
    {
        SetObjectsActive(restRoomObjects, true);

        if (enemySpawner != null)
        {
            enemySpawner.gameObject.SetActive(false);
        }

        if (roomBattleController != null)
        {
            roomBattleController.gameObject.SetActive(false);
        }

        Debug.Log("Room Setup : Rest Room");
    }

    private void SetupEliteRoom()
    {
        SetObjectsActive(combatRoomObjects, true);

        if (enemySpawner != null)
        {
            enemySpawner.gameObject.SetActive(true);
        }

        if (roomBattleController != null)
        {
            roomBattleController.gameObject.SetActive(true);
        }

        Debug.Log("Room Setup : Elite Room");
    }

    private void SetupBossRoom()
    {
        SetObjectsActive(combatRoomObjects, true);

        if (enemySpawner != null)
        {
            enemySpawner.gameObject.SetActive(true);
        }

        if (roomBattleController != null)
        {
            roomBattleController.gameObject.SetActive(true);
        }

        Debug.Log("Room Setup : Boss Room");
    }

    private void SetupAugmentRoom()
    {
        SetObjectsActive(rewardRoomObjects, true);

        if (enemySpawner != null)
        {
            enemySpawner.gameObject.SetActive(false);
        }

        if (roomBattleController != null)
        {
            roomBattleController.gameObject.SetActive(false);
        }

        Debug.Log("Room Setup : Augment Room");
    }

    private void SetObjectsActive(GameObject[] objects, bool active)
    {
        if (objects == null)
        {
            return;
        }

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
            {
                objects[i].SetActive(active);
            }
        }
    }

    public RoomType GetRoomType()
    {
        return roomType;
    }
}