using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private int myID = 0;

  

    public int GetExitID() => myID;
    //
    protected virtual void PassThrough()
    {
        GameManager.Instance().Spawn.SaveSpawnPoint(GetExitID());
    }


        

        private void OnTriggerEnter(Collider _other)
    {
        Debug.Log("�浹");
        if(_other.tag == "Player")
        {
            Debug.Log("�÷��̾� ����");
            PassThrough();
        }
       
    }
}
