using System.Collections.Generic;
using UnityEngine;

public class SpawnManager
{
    private const string SPAWN_POINT_KEY = "SpawnPoint";

    private Dictionary<string, Vector3> spawnPoints = new Dictionary<string, Vector3>();

    public SpawnManager()
    {
    }

    public void Initialize()
    {
        LoadSpawnPoints();
    }

    public void SaveSpawnPoints(Dictionary<string, Vector3> points)
    {
        spawnPoints = points;
        SaveManager.Instance().SaveValue(SPAWN_POINT_KEY, points);
    }

    public void LoadSpawnPoints()
    {
        spawnPoints = SaveManager.Instance().LoadValue<Dictionary<string, Vector3>>(SPAWN_POINT_KEY, new Dictionary<string, Vector3>());
    }

    public void PlacePlayerAtSpawnPoint(GameObject player, string id)
    {
        if (spawnPoints.ContainsKey(id))
        {
            Vector3 spawnPoint = spawnPoints[id];
            player.transform.position = spawnPoint;
        }
    }
}
