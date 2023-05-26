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
        // ���� ����Ʈ �ĺ��ڸ� ���ڿ��� ����
       

        // �� ��ǥ ���� �ҷ���

         return GameManager.Instance().Save.LoadValue(SPAWNPOINTID,-1 );
       


        // �ε��� ��ǥ ���� Vector3�� ��ȯ��
       
    }

    public GameObject FindPlayerPosition()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    public GameObject FindAreaPosition(int exitID)
    {
        SpawnPoint[] exits = GameObject.FindObjectsOfType<SpawnPoint>();
    
        foreach (var exit in exits)
        {
            if (exit.GetExitID() == exitID) return exit.gameObject;
        }
        Debug.LogError($"exitID {exitID} does not exist!");
        return null;

    }


    public void SpawnPlayerToSavedLocation()
    {
        // ����� ���� ����Ʈ ID�� �ε���
        int spawnPointID = GameManager.Instance().Save.LoadValue(SPAWNPOINTID, -12345);
        Debug.Log(spawnPointID);
        // ����� ID�� ���ٸ� �ʱⰪ���� ����
        if (spawnPointID < 0) spawnPointID = 0;


        // ����� ���� ��ġ�� �ε���
        GameObject player = FindPlayerPosition();
        Debug.Log($"player: {player != null}");
        var areaPosition = FindAreaPosition(spawnPointID);
        Debug.Log($"areaPosition: {areaPosition != null}");
        player.transform.position = areaPosition.transform.position + new Vector3(0f, 0.5f, 0f);

        Debug.Log("�����Ϸ�: " + spawnPointID);
    }
}
