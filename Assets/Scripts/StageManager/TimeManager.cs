using UnityEngine;

public class TimeManager
{
    private float currentTime = 0f;

    public float RemainingTime => StageManager.Instance().CountdownDuration - currentTime;
    public bool IsCountdownComplete => currentTime >= StageManager.Instance().CountdownDuration;

    public void StartCountdown()
    {
        currentTime = 0f;
    }

    public void UpdateCountdown()
    {
        if (IsCountdownComplete) return;

        currentTime += Time.deltaTime;
        if (IsCountdownComplete) TimeOver();
    }

    private void TimeOver()
    {
        Debug.Log("시간초과");
    }
}
