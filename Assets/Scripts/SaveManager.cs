using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager 
{
    public bool CheckIsSaved()
    {
        if(PlayerPrefs.HasKey("score"))
        {
            return true;
        }
        if (PlayerPrefs.HasKey("respawn"))
        {
            return true;
        }
        if (PlayerPrefs.HasKey("scene"))
        {
            return true;
        }
        //������Ȯ�� ����
        return false;
    }
}
