using System.Collections.Generic;
using UnityEngine;

public class SpawnManager
{
    private const string SPAWN_POINT_KEY = "SpawnPoint";

    private List<Vector3> spawnPoints = new List<Vector3>();

    public SpawnManager()
    {
    }

    public void Initialize()
    {
        LoadSpawnPoints();
    }

    /// <summary>
    /// 스폰 포인트 저장
    /// </summary>
    public void SaveSpawnPoints(List<Vector3> points)
    {
        spawnPoints = points;
        SaveManager.Instance().SaveValue(SPAWN_POINT_KEY, points);
    }

    /// <summary>
    /// 스폰 포인트 불러오기
    /// </summary>
    public List<Vector3> LoadSpawnPoints()
    {
        spawnPoints = SaveManager.Instance().LoadValue<List<Vector3>>(SPAWN_POINT_KEY, new List<Vector3>());
        return spawnPoints;
    }

    /// <summary>
    /// Place the player at a random spawn point.
    /// </summary>
    public void PlacePlayerAtRandomSpawnPoint(GameObject player)
    {
        if (spawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, spawnPoints.Count);
            Vector3 spawnPoint = spawnPoints[randomIndex];
            player.transform.position = spawnPoint;
        }
        else
        {
            Debug.LogWarning("No spawn points available.");
        }
    }
}
