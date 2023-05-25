using UnityEngine;

public class SpawnManager
{
    private const string SPAWN_POINT_PREFIX = "SpawnPoint_";

    private const string SPAWNPOINTID = "SpawnPointID";

    public void SaveSpawnPoint(int spawnPointID, Vector3 position)
    {
        // ���� ����Ʈ �ĺ��ڸ� ���ڿ��� ����
        string spawnPointKey = SPAWN_POINT_PREFIX + spawnPointID.ToString();

        // ��ġ�� �� ��ǥ�� PlayerPrefs�� ����
        PlayerPrefs.SetFloat(spawnPointKey + "X", position.x);
        PlayerPrefs.SetFloat(spawnPointKey + "Y", position.y);
        PlayerPrefs.SetFloat(spawnPointKey + "Z", position.z);

        // PlayerPrefs ���� ������ ����
        PlayerPrefs.Save();
    }

    public Vector3 LoadSpawnPoint(int spawnPointID)
    {
        // ���� ����Ʈ �ĺ��ڸ� ���ڿ��� ����
        string spawnPointKey = SPAWN_POINT_PREFIX + spawnPointID.ToString();

        // PlayerPrefs���� �� ��ǥ ���� �ҷ���
        float x = PlayerPrefs.GetFloat(spawnPointKey + "X", 0f);
        float y = PlayerPrefs.GetFloat(spawnPointKey + "Y", 0f);
        float z = PlayerPrefs.GetFloat(spawnPointKey + "Z", 0f);

        // �ε��� ��ǥ ���� Vector3�� ��ȯ��
        return new Vector3(x, y, z);
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
