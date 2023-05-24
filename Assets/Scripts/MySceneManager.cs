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

    private int currentSceneID;

    public int CurrentSceneID
    {
        get { return currentSceneID; }

        private set { currentSceneID = value; }
    }

    public void ChangeScene(SCENENAME _name)
    {

        CurrentSceneID = (int)_name;
        SceneManager.LoadScene((int)_name);

        if (_name == SCENENAME.Exterior)
        {
            StageManager.Instance().ChangeExterior(true);
        }

        if (_name != SCENENAME.Exterior)
        {
            StageManager.Instance().ChangeExterior(false);
        }
    }
}