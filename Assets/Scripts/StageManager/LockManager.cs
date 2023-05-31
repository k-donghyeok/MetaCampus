using UnityEngine.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;
using static IHaveLockID;

public class LockManager
{
    public LockManager()
    {
    }

    private int[] passwords = null;

    public int GetPassword(ColorID id)
    {
        if (passwords == null) CreatePasswords();
        return passwords[(int)id];
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
            passwords[i] = Random.Range(1000, 10000);

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

    private static readonly Color[] colors
        = { new Color(1f, 0f, 0f), new Color(0f, 1f, 0f), new Color(0f, 0f, 1f), new Color(1f, 1f, 0.2f), new Color(0f, 1f, 1f), new Color(1f, 0.5f, 1f) };

    public static Color GetColor(ColorID color)
        => colors[(int)color];

    public static void DyeRenderers(ColorID color, MeshRenderer[] dyeRenderers)
    {
        var c = GetColor(color);
        foreach (var r in dyeRenderers)
            if (r.materials[0]) r.materials[0].SetColor("_BaseColor", c);
    }
}

/// <summary>
/// Lock ID를 사용하는 스크립트에 들어가는 인터페이스
/// </summary>
public interface IHaveLockID
{
    /// <summary>
    /// 잠금 종류
    /// </summary>
    public TypeID LockTypeID { get; }
    /// <summary>
    /// 잠금 아이디
    /// </summary>
    public ColorID LockColorID { get; }

    public enum ColorID : int
    {
        Red = 0,
        Green = 1,
        Blue = 2,
        Yellow = 3,
        Cyan = 4,
        Pink = 5
    }

    public enum TypeID
    {
        None = -1,
        OneTime,
        MultiUse,
        Password
    }
}