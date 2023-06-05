using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{
    public Text countdownText; // UI 텍스트 컴포넌트를 가리키는 변수

    private void Update()
    {
        var Time = StageManager.Instance().Time;
        if (Time != null)
            UpdateUI(Time); // UI 업데이트
    }

    private void UpdateUI(TimeManager timeManager)
    {
        int minutes = Mathf.Max(0, Mathf.FloorToInt(timeManager.RemainingTime / 60)); // 분 계산하고 0보다 작으면 0으로 설정
        int seconds = Mathf.Max(0, Mathf.FloorToInt(timeManager.RemainingTime % 60)); // 초 계산하고 0보다 작으면 0으로 설정

        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // 포맷을 사용하여 분과 초를 표시
    }
}
