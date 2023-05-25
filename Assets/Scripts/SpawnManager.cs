using UnityEngine;

public class SpawnManager
{
    

    private const string SPAWNPOINTID = "SpawnPointID";

    public void SaveSpawnPoint(int _spawnPointID)
    {
        // ���� ����Ʈ �ĺ��ڸ� ���ڿ��� ����

        Debug.Log($"���̵� : {_spawnPointID}  ����");
        // ����
        GameManager.Instance().Save.SaveValue("ASD", _spawnPointID);
        GameManager.Instance().Save.SaveToPrefs();
    }

    public int LoadSpawnPoint()
    {
        // ���� ����Ʈ �ĺ��ڸ� ���ڿ��� ����
       

        // �� ��ǥ ���� �ҷ���

         return GameManager.Instance().Save.LoadValue(SPAWNPOINTID,-1 );
       


        // �ε��� ��ǥ ���� Vector3�� ��ȯ��
       
    }

 

    public void SpawnPlayerToSavedLocation()
    {
        // ����� ���� ����Ʈ ID�� �ε���
        int spawnPointID = GameManager.Instance().Save.LoadValue(SPAWNPOINTID, -1);
        // ����� ID�� ���ٸ� �ʱⰪ���� ����
        if (spawnPointID < 0) spawnPointID = 0;


        // ����� ���� ��ġ�� �ε���
        GameObject player = GameManager.Instance().FindPlayerPosition();
        Debug.Log($"player: {player != null}");
        var areaPosition = GameManager.Instance().FindAreaPosition(spawnPointID);
        Debug.Log($"areaPosition: {areaPosition != null}");
        player.transform.position = areaPosition.transform.position +new Vector3(2f,0f,2f);

        Debug.Log("�����Ϸ�: " + spawnPointID);
    }
}
