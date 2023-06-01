using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{
    public Text countdownText; // UI 텍스트 컴포넌트를 가리키는 변수
    private TimeManager timeManager; // TimeManager 스크립트를 가리키는 변수

    private void Awake()
    {
        timeManager = new TimeManager(); // TimeManager 인스턴스를 생성하여 변수에 할당
    }

    private void Start()
    {
        timeManager.StartCountdown(); // 타임 매니저의 StartCountdown 메서드를 호출하여 카운트다운 시작
    }

    private void Update()
    {
        timeManager.UpdateCountdown(); // 타임 매니저의 UpdateCountdown 메서드를 호출하여 남은 시간 업데이트
        UpdateUI(); // UI 업데이트
    }

    private void UpdateUI()
    {
        int minutes = Mathf.Max(0, Mathf.FloorToInt(timeManager.RemainingTime / 60)); // 분 계산하고 0보다 작으면 0으로 설정
        int seconds = Mathf.Max(0, Mathf.FloorToInt(timeManager.RemainingTime % 60)); // 초 계산하고 0보다 작으면 0으로 설정

        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // 포맷을 사용하여 분과 초를 표시
    }
}
