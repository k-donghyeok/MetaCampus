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
        //지도의확장 정도
        return false;
    }
}
