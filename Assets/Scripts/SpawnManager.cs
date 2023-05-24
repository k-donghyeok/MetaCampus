using UnityEngine;

public class SpawnManager
{
    private const string SPAWN_POINT_PREFIX = "SpawnPoint_";

    public void SaveSpawnPoint(int spawnPointID, Vector3 position)
    {
        // 스폰 포인트 식별자를 문자열로 생성
        string spawnPointKey = SPAWN_POINT_PREFIX + spawnPointID.ToString();

        // 위치의 각 좌표를 PlayerPrefs에 저장
        PlayerPrefs.SetFloat(spawnPointKey + "X", position.x);
        PlayerPrefs.SetFloat(spawnPointKey + "Y", position.y);
        PlayerPrefs.SetFloat(spawnPointKey + "Z", position.z);

        // PlayerPrefs 변경 사항을 저장
        PlayerPrefs.Save();
    }

    public Vector3 LoadSpawnPoint(int spawnPointID)
    {
        // 스폰 포인트 식별자를 문자열로 생성
        string spawnPointKey = SPAWN_POINT_PREFIX + spawnPointID.ToString();

        // PlayerPrefs에서 각 좌표 값을 불러옴
        float x = PlayerPrefs.GetFloat(spawnPointKey + "X", 0f);
        float y = PlayerPrefs.GetFloat(spawnPointKey + "Y", 0f);
        float z = PlayerPrefs.GetFloat(spawnPointKey + "Z", 0f);

        // 로드한 좌표 값을 Vector3로 반환함
        return new Vector3(x, y, z);
    }

    public void SpawnPlayerToSavedLocation(int spawnPointID)
    {
        // 저장된 스폰 위치를 로드함
        Vector3 spawnPoint = LoadSpawnPoint(spawnPointID);

        Debug.Log("스폰: " + spawnPoint);
    }
}
