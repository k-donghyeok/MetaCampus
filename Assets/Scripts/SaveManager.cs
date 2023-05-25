using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ����
/// </summary>
public class SaveManager
{
    public SaveManager()
    {
    }

    /// <summary>
    /// ���̺긦 ���Ͽ��� �ҷ���
    /// </summary>
    public void LoadFromPrefs()
    {
        string json = PlayerPrefs.GetString(SAVEDATAONPREFS, string.Empty);
        if (string.IsNullOrEmpty(json)) { Reset(); return; }
        saveData = (Dictionary<string, object>)JsonConvert.DeserializeObject(json, typeof(Dictionary<string, object>));
    }

    /// <summary>
    /// ���̺긦 ���Ϸ� ����
    /// </summary>
    public void SaveToPrefs()
    {
        string json = JsonConvert.SerializeObject(saveData);
        PlayerPrefs.SetString(SAVEDATAONPREFS, json);
    }

    /// <summary>
    /// ���̺� ����: ���ο� ���̺긦 ����
    /// </summary>
    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        saveData = new Dictionary<string, object>();
        SaveValue(SAVESEED, (int)DateTime.Now.Ticks);
        OnSaveReset?.Invoke(this);
        SaveToPrefs();
    }

    public delegate void ResetEventHandler(SaveManager save);

    public ResetEventHandler OnSaveReset = null;

    private Dictionary<string, object> saveData;

    private const string SAVEDATAONPREFS = "SaveData";

    private const string SAVESEED = "Seed";

    /// <summary>
    /// ���̺� ������ �õ尪
    /// </summary>
    public int GetSeed() => LoadValue(SAVESEED, 0);

    /// <summary>
    /// �� ����
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SaveValue<T>(string key, T value)
    {
        saveData.Remove(key);
        saveData.Add(key, value);
    }

    /// <summary>
    /// �� �ҷ�����
    /// </summary>
    public T LoadValue<T>(string key, T defaultValue = default)
    {
        if (saveData.TryGetValue(key, out var value))
        {
            if (value is T t) return t;
            Debug.LogError($"Saved value is {value.GetType()}, not {typeof(T)}!");
        }
        SaveValue(key, defaultValue);
        return defaultValue;
    }

}
