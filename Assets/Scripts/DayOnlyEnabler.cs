using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayOnlyEnabler : MonoBehaviour
{
    [SerializeField]
    private GameObject[] dayOnlyObjects = new GameObject[0];
    [SerializeField]
    private GameObject[] nightOnlyObjects = new GameObject[0];

    private void Start()
    {
        bool day = GameManager.Instance().IsDaytime();
        if (day) foreach (var o in nightOnlyObjects) o.SetActive(false);
        else foreach (var o in dayOnlyObjects) o.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        foreach (var o in dayOnlyObjects) if(o) Gizmos.DrawLine(transform.position, o.transform.position);
        Gizmos.color = Color.cyan;
        foreach (var o in nightOnlyObjects) if (o) Gizmos.DrawLine(transform.position, o.transform.position);
    }
}
