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
        // ����� ���� ����Ʈ ID�� �ε���
        int spawnPointID = LoadSpawnPoint();
        Debug.Log(spawnPointID);
        if (spawnPointID < 0) spawnPointID = 0; // ����� �� ������ �ʱⰪ ���

        // ����� ���� ��ġ�� �ε���
        GameObject player = FindPlayerPosition();
        Debug.Log($"player: {player != null}");
        var areaPosition = FindAreaPosition(spawnPointID);
        Debug.Log($"areaPosition: {areaPosition != null}");
        player.transform.position = areaPosition.transform.position + new Vector3(0f, 1f, 0f);

        Debug.Log("�����Ϸ�: " + spawnPointID);
    }
}
