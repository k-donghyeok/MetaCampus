using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LockManager
{
    public LockManager()
    {
    }

    public static void SaveIDs(IHaveLockID[] interfaces)
    {
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