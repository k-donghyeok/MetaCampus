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
       
        // 만약 id의 Yeil을 이미 먹었다면 return
        // score ++, 그리고 id의 Yeil을 먹었음으로 하고 저장
        
        //example
        //PlayerPrefs.SetInt(id.ToString(), 1);
        //int save = PlayerPrefs.GetInt(id.ToString(), 0);
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
