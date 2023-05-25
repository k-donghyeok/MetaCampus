using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ����
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
    /// ����
    /// </summary>
    public int Score
    {
        get { return score; }
        private set
        {
            if (score == value) return;
            Debug.Log($"������Ƽ �۵� {score} <- {value}");
            score = value;
            Save.SaveValue(SCOREKEY, score);
        }
    }
    private int score = 0;

    /// <summary>
    /// ���� �Ծ��� �뺸
    /// </summary>
    /// <param name="id"></param>
    /// <returns>���� �߰� ���� ����</returns>
    public bool YeilTaken(HowToGetYeil id)
    {
        string strid = Convert.ToString(id);
        
        if (Save.LoadValue(strid, 0) != 0)
        {
            Debug.Log($"{strid} �̰Ŵ� �̹� ����");
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
