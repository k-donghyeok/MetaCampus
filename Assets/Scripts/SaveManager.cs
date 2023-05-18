using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

/// <summary>
/// 历厘 包府
/// </summary>
public class SaveManager
{
    public SaveManager()
    {
    }

    public void Initialize()
    {
        int exists = LoadValue(SAVEEXIST, 0);
        if (exists == 0) Reset();
    }

    /// <summary>
    /// 技捞宏 府悸: 货肺款 技捞宏甫 积己
    /// </summary>
    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        SaveValue(SAVEEXIST, 1);
        OnSaveReset?.Invoke(this);
    }

    public delegate void ResetEventHandler(SaveManager save);

    public ResetEventHandler OnSaveReset = null;


    private const string SAVEEXIST = "SaveExists";

    /// <summary>
    /// 蔼 历厘
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SaveValue<T>(string key, T value)
    {
        if (value is int intValue) PlayerPrefs.SetInt(key, intValue);
        else if (value is float floatValue) PlayerPrefs.SetFloat(key, floatValue);
        else PlayerPrefs.SetString(key, value.ToString());
    }

    public int LoadValue(string key, int defaultIntValue = default) =>
        PlayerPrefs.GetInt(key, defaultIntValue);
    public float LoadValue(string key, float defaultFloatValue) =>
        PlayerPrefs.GetFloat(key, defaultFloatValue);
    public string LoadValue(string key, string defaultStringValue) =>
        PlayerPrefs.GetString(key, defaultStringValue);

}
