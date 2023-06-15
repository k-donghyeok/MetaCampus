using UnityEngine;

public class SpawnManager
{


    private const string SPAWNPOINTID = "SpawnPointID";

    public void SaveSpawnPoint(int _spawnPointID)
    {
        // ���� ����Ʈ �ĺ��ڸ� ���ڿ��� ����

        Debug.Log($"���̵� : {_spawnPointID}  ����");
        // ����
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
        // ����� ���� ����Ʈ ID�� �ε���
        int spawnPointID = LoadSpawnPoint();
        Debug.Log($"Loaded SpawnPointID: {spawnPointID}");
        if (spawnPointID < 0) spawnPointID = 10; // ����� �� ������ �ʱⰪ ���

        // ����� ���� ��ġ�� �ε���
        var spawnPoint = FindSpawnPoint(spawnPointID);
        PlayerManager.InstanceOrigin().position = spawnPoint.position + new Vector3(0f, 0.5f, 0f);
        var spawnRot = spawnPoint.rotation.eulerAngles.y;
        PlayerManager.InstanceOrigin().rotation = Quaternion.Euler(0f, spawnRot, 0f);

        Debug.Log("�����Ϸ�: " + spawnPointID);
    }
}
