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
        // 스폰 포인트 식별자를 문자열로 생성
       

        // 각 좌표 값을 불러옴

         return GameManager.Instance().Save.LoadValue(SPAWNPOINTID,-1 );
       


        // 로드한 좌표 값을 Vector3로 반환함
       
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
        // 저장된 스폰 포인트 ID를 로드함
        int spawnPointID = GameManager.Instance().Save.LoadValue(SPAWNPOINTID, -12345);
        Debug.Log(spawnPointID);
        // 저장된 ID가 없다면 초기값으로 설정
        if (spawnPointID < 0) spawnPointID = 0;


        // 저장된 스폰 위치를 로드함
        GameObject player = FindPlayerPosition();
        Debug.Log($"player: {player != null}");
        var areaPosition = FindAreaPosition(spawnPointID);
        Debug.Log($"areaPosition: {areaPosition != null}");
        player.transform.position = areaPosition.transform.position + new Vector3(0f, 0.5f, 0f);

        Debug.Log("스폰완료: " + spawnPointID);
    }
}
