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
            //�̹�ȹ��������
            return;
        }
        else
        {
            //ȹ�������ʾ�����
            ++score;
            Score = score;
            PlayerPrefs.SetInt(strId, 1);
        }
        // ���� id�� Yeil�� �̹� �Ծ��ٸ� return
        // score ++, �׸��� id�� Yeil�� �Ծ������� �ϰ� ����
        
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
            Debug.Log($"�� ���ھ� {score}");
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
