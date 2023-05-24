using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager
{
    public enum SCENENAME
    {
        Interior,
        Exterior
       
    }

    public void ChangeScene(SCENENAME _name)
    {
        
        SceneManager.LoadScene((int)_name);

        if(_name==SCENENAME.Exterior)
        {
            StageManager.Instance().ChangeExterior(true);
        }

        if(_name==SCENENAME.Interior)
        {
            StageManager.Instance().ChangeExterior(false);
        }
       
    }


}

    

