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

    public Transform FindSpawnPoint(int spawnID)
    {
        SpawnPoint[] spawns = GameObject.FindObjectsOfType<SpawnPoint>();
        foreach (var spawn in spawns)
        {
            if (spawn.GetID() == spawnID) return spawn.GetSpawn();
        }
        Debug.LogError($"{nameof(spawnID)} {spawnID} does not exist!");
        return spawns[0].GetSpawn();
    }


    public void SpawnPlayerToSavedLocation()
    {
        // 저장된 스폰 포인트 ID를 로드함
        int spawnPointID = LoadSpawnPoint();
        Debug.Log($"Loaded SpawnPointID: {spawnPointID}");
        if (spawnPointID < 0) spawnPointID = 10; // 저장된 게 없으면 초기값 사용

        // 저장된 스폰 위치를 로드함
        var spawnPoint = FindSpawnPoint(spawnPointID);
        PlayerManager.InstanceOrigin().position = spawnPoint.position + new Vector3(0f, 0.5f, 0f);
        var spawnRot = spawnPoint.rotation.eulerAngles.y;
        PlayerManager.InstanceOrigin().rotation = Quaternion.Euler(0f, spawnRot, 0f);

        Debug.Log("스폰완료: " + spawnPointID);
    }
}
