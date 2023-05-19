using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

[ExecuteInEditMode]
public class LevelEditFunctions : MonoBehaviour
{
    


    public void SaveIDs()
    {
#if UNITY_EDITOR
        var interfaces = FindObjectsOfType<MonoBehaviour>().OfType<IHaveLockID>();
        HashSet<int> ids = new HashSet<int>();
        foreach (var i in interfaces) ids.Add(i.LockID);
        
        string sceneName = SceneManager.GetActiveScene().name;
        string path = GetIDPath(sceneName);

        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(string.Join(',', ids));
            }
        }

        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public int[] LoadIDs()
    {
        string data = "1,4,2"; //loaded
        return Array.ConvertAll(data.Split(','), int.Parse);
    }

    public static string GetIDPath(string sceneName)
    {
        return $"Assets/Resources/GameData/{sceneName}-ID.txt";
    }
}


public interface IHaveLockID
{
    public int LockID { get; }
}