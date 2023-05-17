using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeilManager
{
    private int score=0;

    public YeilManager()
    {
      
    }

    public int Score
    {
        get { return score; }
        private set
        {
            if (score == value) return;
            score = value;
            PlayerPrefs.SetInt("score", score);
        }
    }
  

    public void YeilTaken(HowToGetYeil id)
    {
        string strId = Convert.ToString(id);
        if(PlayerPrefs.GetInt(strId) ==1)
        {
            //이미획득했을때
            return;
        }
        else
        {
            //획득하지않았을때
            ++score;
            Score = score;
            PlayerPrefs.SetInt(strId, 1);
        }
        // 만약 id의 Yeil을 이미 먹었다면 return
        // score ++, 그리고 id의 Yeil을 먹었음으로 하고 저장
        
        //example
        //PlayerPrefs.SetInt(id.ToString(), 1);
        //int save = PlayerPrefs.GetInt(id.ToString(), 0);
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
    }
    public void SetScore()
    {
        if(GameManager.Instance().Save.CheckIsSaved())
        {
            Score = PlayerPrefs.GetInt("score");
            Debug.Log($"셋 스코어 {score}");
        }
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
