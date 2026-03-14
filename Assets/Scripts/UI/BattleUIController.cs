using TMPro;
using UnityEngine;

public class BattleUIController : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("텍스트 UI")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private TextMeshProUGUI roomClearText;

    [Header("표시 설정")]
    [SerializeField] private string wavePrefix = "Wave : ";
    [SerializeField] private string enemyPrefix = "Enemies Left : ";
    [SerializeField] private string roomClearMessage = "Room Cleared!";

    private bool hasShownRoomClear;

    private void Start()
    {
        // 시작 시 클리어 문구 숨김
        if (roomClearText != null)
        {
            roomClearText.gameObject.SetActive(false);
        }

        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (enemySpawner == null)
        {
            return;
        }

        // 현재 웨이브 표시
        if (waveText != null)
        {
            waveText.text = wavePrefix + enemySpawner.GetCurrentWave();
        }

        // 남은 적 수 표시
        if (enemyCountText != null)
        {
            enemyCountText.text = enemyPrefix + enemySpawner.GetAliveEnemyCount();
        }

        // 방 클리어 표시
        if (!hasShownRoomClear && enemySpawner.IsRoomCleared())
        {
            hasShownRoomClear = true;

            if (roomClearText != null)
            {
                roomClearText.gameObject.SetActive(true);
                roomClearText.text = roomClearMessage;
            }
        }
    }
}