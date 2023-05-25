using UnityEngine;

public class SpawnManager
{
    private const string SPAWN_POINT_PREFIX = "SpawnPoint_";

    private const string SPAWNPOINTID = "SpawnPointID";

    public void SaveSpawnPoint(int spawnPointID, Vector3 position)
    {
        // ���� ����Ʈ �ĺ��ڸ� ���ڿ��� ����
        string spawnPointKey = SPAWN_POINT_PREFIX + spawnPointID.ToString();

        // ����
        GameManager.Instance().Save.SaveValue(spawnPointKey + "X", position.x);
        GameManager.Instance().Save.SaveValue(spawnPointKey + "Y", position.y);
        GameManager.Instance().Save.SaveValue(spawnPointKey + "Z", position.z);
    }

    //public Vector3 LoadSpawnPoint(int spawnPointID)
    //{
    //    // ���� ����Ʈ �ĺ��ڸ� ���ڿ��� ����
    //    string spawnPointKey = SPAWN_POINT_PREFIX + spawnPointID.ToString();

    //    // �� ��ǥ ���� �ҷ���
       
    //   GameManager.Instance().Save.LoadValue(spawnPointKey + "X", 0f);
    //   GameManager.Instance().Save.LoadValue(spawnPointKey + "Y", 0f);
    //   GameManager.Instance().Save.LoadValue(spawnPointKey + "Z", 0f);


    //    // �ε��� ��ǥ ���� Vector3�� ��ȯ��
    //    return new Vector3(x, y, z);
    //}

    public void TestSave(int _id)
    {
        string strid = _id.ToString();
        GameManager.Instance().Save.SaveValue(strid, _id);
    }

    public void SpawnPlayerToSavedLocation()
    {
        // ����� ���� ����Ʈ ID�� �ε���
        int spawnPointID = GameManager.Instance().Save.LoadValue<int>(SPAWNPOINTID, -1);
        // ����� ID�� ���ٸ� �ʱⰪ���� ����
        if (spawnPointID < 0) spawnPointID = 0;

        // ����� ���� ��ġ�� �ε���
        Vector3 spawnPoint = LoadSpawnPoint(spawnPointID);

        Debug.Log("����: " + spawnPoint);
    }
}
