using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{
    public Text countdownText; // UI �ؽ�Ʈ ������Ʈ�� ����Ű�� ����

    private void Update()
    {
        var Time = StageManager.Instance().Time;
        if (Time != null)
            UpdateUI(Time); // UI ������Ʈ
    }

    private void UpdateUI(TimeManager timeManager)
    {
        int minutes = Mathf.Max(0, Mathf.FloorToInt(timeManager.RemainingTime / 60)); // �� ����ϰ� 0���� ������ 0���� ����
        int seconds = Mathf.Max(0, Mathf.FloorToInt(timeManager.RemainingTime % 60)); // �� ����ϰ� 0���� ������ 0���� ����

        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // ������ ����Ͽ� �а� �ʸ� ǥ��
    }
}
