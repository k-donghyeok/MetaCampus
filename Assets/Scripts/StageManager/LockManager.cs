using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEditor.SceneManagement;

public class LockManager
{
    public LockManager()
    {
    }

    private int[] passwords = null;

    public int GetPassword(int id)
    {
        if (passwords == null) CreatePasswords();
        return passwords[id];
    }

    private void CreatePasswords()
    {
        passwords = new int[6];

        // 원래 시드를 저장
        var seedBak = Random.state;

        // 스테이지에 맞는 시드를 생성
        string stageSeed = SceneManager.GetActiveScene().name
            + GameManager.Instance().Save.GetSeed();

        // 시드를 설정
        Random.InitState(stageSeed.GetHashCode());

        // 비밀번호 생성 후 저장
        for (int i = 0; i < passwords.Length; ++i)
            passwords[i] = Random.Range(1000, 9999);

        // 시드 복구
        Random.state = seedBak;
    }


    /*
    /// <summary>
    /// 이 장면의 Lock ID를 게임 파일로 저장
    /// </summary>
    public static void SaveIDs(IHaveLockID[] interfaces)
    {
        // 중복되지 않는 값을 받는 HashSet을 초기화
        HashSet<int> ids = new HashSet<int>();
        // 이 장면의 모든 IHaveLockID 인터페이스에 있는 ID를 받아옴
        foreach (var i in interfaces) ids.Add(i.LockColorID); //수정됨 LockID에서 지금으로

        string sceneName = SceneManager.GetActiveScene().name;
        string path = $"Assets/Resources/{GetIDPath(sceneName)}.txt";

        // ID를 Resources 폴더에 게임 파일로 저장
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(string.Join(',', ids));
            }
        }

        UnityEditor.AssetDatabase.Refresh();
        Debug.Log($"{sceneName}의 Lock 저장 완료");
    }

    /// <summary>
    /// 원하는 장면의 Lock ID를 게임 파일에서 불러옴
    /// </summary>
    public static int[] LoadIDs(string sceneName)
    {
        var text = Resources.Load(GetIDPath(sceneName)) as TextAsset;
        string data = text.text;
        return Array.ConvertAll(data.Split(','), int.Parse);
    }

    /// <summary>
    /// 장면의 Lock ID가 저장된 게임 파일의 Resources 위치를 반환
    /// </summary>
    public static string GetIDPath(string sceneName)
    {
        return $"GameData/{sceneName}-ID";
    }
    */



}

/// <summary>
/// Lock ID를 사용하는 스크립트에 들어가는 인터페이스
/// </summary>
public interface IHaveLockID
{

    public int LockTypeID { get; }
    public int LockColorID { get; }
}