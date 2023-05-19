using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
   
   
    public static GameManager Instance() => instance;

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Initiate();
        }
        else if (instance != this) Destroy(gameObject);
    }

    public YeilManager Yeil { get; private set; } = null;

    public LockManager Lock { get; private set; } = null;
    private void Initiate()
    {
        Yeil = new YeilManager();
        Lock = new LockManager(); 
       
    }

}
