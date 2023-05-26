using UnityEngine;

public class SpawnManager
{


    private const string SPAWNPOINTID = "SpawnPointID";

    public void SaveSpawnPoint(int _spawnPointID)
    {
        // 스폰 포인트 식별자를 문자열로 생성

        Debug.Log($"아이디 : {_spawnPointID}  저장");
        // 저장
        GameManager.Instance().Save.SaveValue(SPAWNPOINTID, _spawnPointID);
        GameManager.Instance().Save.SaveToPrefs();
    }

    public int LoadSpawnPoint()
    {
        return GameManager.Instance().Save.LoadValue(SPAWNPOINTID, -1);
    }

    public GameObject FindPlayerPosition()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    public GameObject FindAreaPosition(int spawnID)
    {
        SpawnPoint[] spawns = GameObject.FindObjectsOfType<SpawnPoint>();
        foreach (var spawn in spawns)
        {
            if (spawn.GetID() == spawnID) return spawn.gameObject;
        }
        Debug.LogError($"{nameof(spawnID)} {spawnID} does not exist!");
        return spawns[0].gameObject;
    }


    public void SpawnPlayerToSavedLocation()
    {
        // 저장된 스폰 포인트 ID를 로드함
        int spawnPointID = LoadSpawnPoint();
        Debug.Log(spawnPointID);
        if (spawnPointID < 0) spawnPointID = 0; // 저장된 게 없으면 초기값 사용

        // 저장된 스폰 위치를 로드함
        GameObject player = FindPlayerPosition();
        Debug.Log($"player: {player != null}");
        var areaPosition = FindAreaPosition(spawnPointID);
        Debug.Log($"areaPosition: {areaPosition != null}");
        player.transform.position = areaPosition.transform.position + new Vector3(0f, 1f, 0f);

        Debug.Log("스폰완료: " + spawnPointID);
    }
}
