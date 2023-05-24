using UnityEngine;

public class SpawnManager
{
    private const string SPAWN_POINT_PREFIX = "SpawnPoint_";

    public void SaveSpawnPoint(int spawnPointID, Vector3 position)
    {
        string spawnPointKey = SPAWN_POINT_PREFIX + spawnPointID.ToString();
        PlayerPrefs.SetFloat(spawnPointKey + "X", position.x);
        PlayerPrefs.SetFloat(spawnPointKey + "Y", position.y);
        PlayerPrefs.SetFloat(spawnPointKey + "Z", position.z);
        PlayerPrefs.Save();
    }

    public Vector3 LoadSpawnPoint(int spawnPointID)
    {
        string spawnPointKey = SPAWN_POINT_PREFIX + spawnPointID.ToString();
        float x = PlayerPrefs.GetFloat(spawnPointKey + "X", 0f);
        float y = PlayerPrefs.GetFloat(spawnPointKey + "Y", 0f);
        float z = PlayerPrefs.GetFloat(spawnPointKey + "Z", 0f);
        return new Vector3(x, y, z);
    }

    public void SpawnPlayerToSavedLocation(int spawnPointID)
    {
        Vector3 spawnPoint = LoadSpawnPoint(spawnPointID);
        Debug.Log("½ºÆù: " + spawnPoint);
    }
}
