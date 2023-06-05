using UnityEngine;

public class TimeManager
{
    public TimeManager(float remainingTime)
    {
        totalTime = remainingTime;
    }

    private float currentTime = 0f;

    private readonly float totalTime = 70f;


    public float RemainingTime => totalTime - currentTime;
    public bool IsCountdownComplete => currentTime >= totalTime;

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
