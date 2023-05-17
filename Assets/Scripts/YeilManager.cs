using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeilManager
{
    private int score=0;

    public YeilManager()
    {
        Debug.Log($"������ �۵� {PlayerPrefs.GetInt("score")}");
        if (PlayerPrefs.HasKey("score"))
        {
            Debug.Log($"������ �۵��� ���������� ������{PlayerPrefs.GetInt("score")}");
            score = PlayerPrefs.GetInt("score");
        }
    }

    public int Score
    {
        get { return score; }
        private set
        {
           
               
            
            Debug.Log($"������Ƽ�۵�");
            PlayerPrefs.SetInt("score", score);
        }
    }
  

    public void YeilTaken(HowToGetYeil id)
    {
        string strid = Convert.ToString(id);
       if(PlayerPrefs.HasKey(strid)) // ���� id�� Yeil�� �̹� �Ծ��ٸ� return
        {
            return;
        }
        // score ++, �׸��� id�� Yeil�� �Ծ������� �ϰ� ����
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
