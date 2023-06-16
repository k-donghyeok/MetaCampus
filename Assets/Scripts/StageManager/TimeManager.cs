using System;
using UnityEngine;

public class TimeManager
{
    public TimeManager(float remainingTime)
    {
        totalTime = remainingTime;
    }

    private float currentTime = 0f;

    private readonly float totalTime = 180f;

    /// <summary>
    /// 남은 시간 (초)
    /// </summary>
    public float RemainingTime => totalTime - currentTime;
    /// <summary>
    /// 시간 초과 여부
    /// </summary>
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

    public void DecreaseTimeByOneMinute()
    {
        currentTime += 60f;
        if (currentTime > totalTime)
        {
            currentTime = totalTime;
            TimeOver();
        }
    }

    private void TimeOver()
    {
        Debug.Log("시간초과");
        currentTime = totalTime;
        OnTimeOver?.Invoke();
    }

    public Action OnTimeOver = null;
}
