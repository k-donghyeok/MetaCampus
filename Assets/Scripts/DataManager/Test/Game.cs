using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Text name;

    void Start()
    {
        name.text += DataManager.instance.nowPlayer.name;
    }

    public void Save()
    {
        DataManager.instance.SaveData();
    }
}