using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DoorKey : MonoBehaviour, IHaveLockID
{
    [SerializeField]
    private int lockTypeID = 0;
    [SerializeField]
    private int lockColorID = 0;

    public int LockTypeID => lockTypeID;
    public int LockColorID => lockColorID;
 
    #region 떠다니는데 필요한 변수
    [SerializeField,Range(1f,10f)]private float floatspeed = 1f;
    [SerializeField,Range(0f, 10f)] private float height = 1f;
    private float angle = 0f;
    #endregion

    [SerializeField]
   // private Color color = Color.magenta;


    private Color[] colors = { new Color(1f, 0f, 0f), new Color(0f, 1f, 0f), new Color(0f, 0f, 1f), new Color(1f, 0.92f, 0.16f), new Color(0f, 1f, 1f) };
    protected virtual void Start()
    {
        SetColors(colors);
        
    }
    private void Update()
    {
        Float();
    }
    private void Float()
    {
        angle += floatspeed;
        if(angle>360f)
        {
            angle = 0f;
        }
        float deg = Mathf.Deg2Rad * angle;
        float sin = Mathf.Sin(deg);
       
        transform.position = new Vector3(transform.position.x, height + sin, transform.position.z);
    }

    private void SetColors(Color[] _colors)
    {
        Color color = _colors[LockColorID];

        MeshRenderer[] ren = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < ren.Length; ++i)
        {
            Debug.Log($"{i}: {ren[i].material.name}, {ren[i].material.GetColor("_BaseColor")}");
            ren[i].material.SetColor("_BaseColor", color);
        }
    }
}
