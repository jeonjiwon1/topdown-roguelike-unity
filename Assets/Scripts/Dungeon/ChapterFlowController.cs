using UnityEngine;

public class ChapterFlowController : MonoBehaviour
{
    [Header("Chapter Room 순서")]
    [SerializeField] private RoomData[] chapterRooms;

    [Header("현재 진행 상태")]
    [SerializeField] private int currentRoomIndex = 0;

    private void Start()
    {
        ActivateCurrentRoom();
    }

    private void Update()
    {
        // 테스트용: N키로 다음 Room 이동
        if (Input.GetKeyDown(KeyCode.N))
        {
            MoveToNextRoom();
        }
    }

    public void ActivateCurrentRoom()
    {
        if (chapterRooms == null || chapterRooms.Length == 0)
        {
            Debug.LogWarning("ChapterFlowController : chapterRooms가 비어 있음");
            return;
        }

        for (int i = 0; i < chapterRooms.Length; i++)
        {
            if (chapterRooms[i] != null && chapterRooms[i].roomRoot != null)
            {
                chapterRooms[i].roomRoot.SetActive(i == currentRoomIndex);
            }
        }

        if (chapterRooms[currentRoomIndex] != null)
        {
            Debug.Log("현재 Room : " + chapterRooms[currentRoomIndex].roomName + " / " + chapterRooms[currentRoomIndex].roomType);
        }
    }

    public void MoveToNextRoom()
    {
        if (chapterRooms == null || chapterRooms.Length == 0)
        {
            return;
        }

        currentRoomIndex++;

        if (currentRoomIndex >= chapterRooms.Length)
        {
            Debug.Log("Chapter Clear");
            currentRoomIndex = chapterRooms.Length - 1;
            return;
        }

        ActivateCurrentRoom();
    }

    public RoomData GetCurrentRoomData()
    {
        if (chapterRooms == null || chapterRooms.Length == 0)
        {
            return null;
        }

        if (currentRoomIndex < 0 || currentRoomIndex >= chapterRooms.Length)
        {
            return null;
        }

        return chapterRooms[currentRoomIndex];
    }

    public int GetCurrentRoomIndex()
    {
        return currentRoomIndex;
    }
}