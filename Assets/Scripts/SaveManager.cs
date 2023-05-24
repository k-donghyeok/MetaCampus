using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

/// <summary>
/// 저장 관리
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
    /// 세이브 리셋: 새로운 세이브를 생성
    /// </summary>
    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        SaveValue(SAVEEXIST, 1);
        SaveValue(SAVESEED, (int)DateTime.Now.Ticks);
        OnSaveReset?.Invoke(this);
    }

    public delegate void ResetEventHandler(SaveManager save);

    public ResetEventHandler OnSaveReset = null;


    private const string SAVEEXIST = "SaveExists";
    private const string SAVESEED = "SaveSeed";

    /// <summary>
    /// 세이브 파일의 시드값
    /// </summary>
    public int GetSeed() => LoadValue(SAVESEED, 0);

    /// <summary>
    /// 값 저장
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SaveValue<T>(string key, T value)
    {
        if (value is int intValue) PlayerPrefs.SetInt(key, intValue);
        else if (value is float floatValue) PlayerPrefs.SetFloat(key, floatValue);
        else PlayerPrefs.SetString(key, value.ToString());
    }

    /// <summary>
    /// 값 불러오기
    /// </summary>
    /// <exception cref="ArgumentException">들어오는 값이 <see cref="int"/>나 <see cref="float"/>나 <see cref="string"/>이 아닌 경우</exception>
    public T LoadValue<T>(string key, T defaultValue = default)
    {
        if (typeof(T) == typeof(int))
            return (T)(object)PlayerPrefs.GetInt(key, Convert.ToInt32(defaultValue));
        else if (typeof(T) == typeof(float))
            return (T)(object)PlayerPrefs.GetFloat(key, Convert.ToSingle(defaultValue));
        else if (typeof(T) == typeof(string))
        return (T)(object)PlayerPrefs.GetString(key, Convert.ToString(defaultValue));

        throw new ArgumentException("Unsupported type!");
    }

}
