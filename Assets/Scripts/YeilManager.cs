using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 점수 관리
/// </summary>
public class YeilManager
{
    public YeilManager()
    {
        //score = Save.LoadValue(SCOREKEY, 0);
        Save.OnSaveReset += OnNewSave;
    }

    private SaveManager Save => GameManager.Instance().Save;

    private void OnNewSave(SaveManager save)
    {
        save.SaveValue(SCOREKEY, 0);
    }

    private const string SCOREKEY = "score";

    /// <summary>
    /// 점수
    /// </summary>
    public int Score
    {
        get { return score; }
        private set
        {
            if (score == value) return;
            Debug.Log($"프로퍼티 작동 {score} <- {value}");
            score = value;
            Save.SaveValue(SCOREKEY, score);
        }
    }
    private int score = 0;

    /// <summary>
    /// 점수 먹었음 통보
    /// </summary>
    /// <param name="id"></param>
    /// <returns>점수 추가 성공 여부</returns>
    public bool YeilTaken(HowToGetYeil id)
    {
        string strid = Convert.ToString(id);
        
        if (Save.LoadValue(strid, 0) != 0)
        {
            Debug.Log($"{strid} 이거는 이미 먹음");
            return false;
        }
        ++Score;
        Save.SaveValue(strid, 1);
        return true;
    }

   
    
    public enum HowToGetYeil
    {
        TakenApplePhoto,
        TakenPondPhoto,
        TaKenTestPhoto1,
        TaKenTestPhoto2,
        TaKenTestPhoto3,
        TaKenTestPhoto4
    }
}
