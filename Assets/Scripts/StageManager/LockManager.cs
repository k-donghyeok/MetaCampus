using UnityEngine.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;
using static IHaveLockID;

public class LockManager
{
    public LockManager()
    {
    }

    #region Password

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

    #endregion Password

    #region Remote

    private readonly bool[] remotes = new bool[6];

    /// <summary>
    /// Remote 문류를 열기
    /// </summary>
    /// <param name="color"></param>
    public void OpenRemote(ColorID color)
    {
        if (remotes[(int)color]) return;
        remotes[(int)color] = true;
        OnRemoteOpened(color);
    }

    public delegate void RemoteOpenHandler(ColorID color);

    /// <summary>
    /// Remote 잠금이 해제됐을 때 발생하는 이벤트
    /// </summary>
    public RemoteOpenHandler OnRemoteOpened;

    #endregion Remote

    private static readonly Color[] colors
        = { new Color(1f, 0f, 0f), new Color(0f, 1f, 0f), new Color(0f, 0f, 1f), new Color(1f, 1f, 0.2f), new Color(0f, 1f, 1f), new Color(1f, 0.5f, 1f) };

    public static Color GetColor(ColorID color)
        => color == ColorID.None ? Color.white : colors[(int)color];

    public static void DyeRenderers(ColorID color, MeshRenderer[] dyeRenderers)
    {
        if (color == ColorID.None) return;
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
        None = -1,
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
        Password,
        Remote
    }
}