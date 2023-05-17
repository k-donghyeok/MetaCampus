using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject mobPrefab = null;

    [SerializeField, Range(0, 100)]
    private int amount = 1;

    private Mob[] mobPool;

    private bool init = false;

    private void Awake()
    {
        init = false;
    }

    public void Activate()
    {
        if (!init)
        {
            mobPool = new Mob[amount];
            for (int i = 0; i < amount; ++i)
            {
                var go = Instantiate(mobPrefab);
                go.transform.SetParent(transform, true);
                go.transform.SetLocalPositionAndRotation(GetRandomSpawnPos(), Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));

            }

            init = true;
        }
    }

    private Vector3 GetRandomSpawnPos()
    {
        var rad = GetComponent<SphereCollider>().radius;
        Vector3 pos = Vector3.zero;
        while (true)
        {
            //pos = Vector3.up * rad + 
        }

        return pos;
    }

    public void Deactivate()
    {

    }
}
