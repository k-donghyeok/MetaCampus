using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{
    public Text countdownText; // UI �ؽ�Ʈ ������Ʈ�� ����Ű�� ����
    private TimeManager timeManager; // TimeManager ��ũ��Ʈ�� ����Ű�� ����

    private void Awake()
    {
        timeManager = new TimeManager(); // TimeManager �ν��Ͻ��� �����Ͽ� ������ �Ҵ�
    }

    private void Start()
    {
        timeManager.StartCountdown(); // Ÿ�� �Ŵ����� StartCountdown �޼��带 ȣ���Ͽ� ī��Ʈ�ٿ� ����
    }

    private void Update()
    {
        timeManager.UpdateCountdown(); // Ÿ�� �Ŵ����� UpdateCountdown �޼��带 ȣ���Ͽ� ���� �ð� ������Ʈ
        UpdateUI(); // UI ������Ʈ
    }

    private void UpdateUI()
    {
        int minutes = Mathf.Max(0, Mathf.FloorToInt(timeManager.RemainingTime / 60)); // �� ����ϰ� 0���� ������ 0���� ����
        int seconds = Mathf.Max(0, Mathf.FloorToInt(timeManager.RemainingTime % 60)); // �� ����ϰ� 0���� ������ 0���� ����

        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // ������ ����Ͽ� �а� �ʸ� ǥ��
    }
}
