using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeilManager
{
    private int score=0;

    public YeilManager()
    {
        Debug.Log($"생성자 작동 {PlayerPrefs.GetInt("score")}");
        if (PlayerPrefs.HasKey("score"))
        {
            Debug.Log($"생성자 작동후 저장파일이 있을떄{PlayerPrefs.GetInt("score")}");
            score = PlayerPrefs.GetInt("score");
        }
    }

    public int Score
    {
        get { return score; }
        private set
        {
           
               
            
            Debug.Log($"프로퍼티작동");
            PlayerPrefs.SetInt("score", score);
        }
    }
  

    public void YeilTaken(HowToGetYeil id)
    {
        string strid = Convert.ToString(id);
       if(PlayerPrefs.HasKey(strid)) // 만약 id의 Yeil을 이미 먹었다면 return
        {
            return;
        }
        // score ++, 그리고 id의 Yeil을 먹었음으로 하고 저장
        ++score;
        Score = score;
        PlayerPrefs.SetInt(strid, 1);
        
      
        
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
