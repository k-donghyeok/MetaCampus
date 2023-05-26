using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private int myID = 0;

    [SerializeField] private Transform spawnPoint = null;

    public int GetID() => myID;
    
    protected virtual void PassThrough()
    {
        GameManager.Instance().Spawn.SaveSpawnPoint(GetID());
    }

    private void OnTriggerEnter(Collider _other)
    {
        Debug.Log("�浹");
        if (!_other.CompareTag("Player")) return;
        Debug.Log("�÷��̾� ����");
        PassThrough();
    }
}
