using UnityEngine;

public class TimeManager
{
    private float countdownDuration = 60f;
    private float currentTime = 0f;

    public float RemainingTime => countdownDuration - currentTime;
    public bool IsCountdownComplete => currentTime >= countdownDuration;

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
