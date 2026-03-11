using UnityEngine;

public class RoomClearListener : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("방 클리어 시 활성화할 오브젝트")]
    [SerializeField] private GameObject[] objectsToActivate;

    [Header("방 클리어 시 비활성화할 오브젝트")]
    [SerializeField] private GameObject[] objectsToDeactivate;

    private void OnEnable()
    {
        if (enemySpawner != null)
        {
            enemySpawner.OnRoomCleared += HandleRoomCleared;
        }
    }

    private void OnDisable()
    {
        if (enemySpawner != null)
        {
            enemySpawner.OnRoomCleared -= HandleRoomCleared;
        }
    }

    private void HandleRoomCleared()
    {
        Debug.Log("RoomClearListener : 방 클리어 신호 받음");

        if (objectsToActivate != null)
        {
            for (int i = 0; i < objectsToActivate.Length; i++)
            {
                if (objectsToActivate[i] != null)
                {
                    objectsToActivate[i].SetActive(true);
                }
            }
        }

        if (objectsToDeactivate != null)
        {
            for (int i = 0; i < objectsToDeactivate.Length; i++)
            {
                if (objectsToDeactivate[i] != null)
                {
                    objectsToDeactivate[i].SetActive(false);
                }
            }
        }
    }
}