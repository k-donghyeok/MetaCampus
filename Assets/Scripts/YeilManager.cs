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
       
        // ���� id�� Yeil�� �̹� �Ծ��ٸ� return
        // score ++, �׸��� id�� Yeil�� �Ծ������� �ϰ� ����
        
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
