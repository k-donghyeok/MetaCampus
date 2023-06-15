using UnityEngine;

public class TriggerEnterDisplay : MonoBehaviour
{
    [SerializeField]
    protected GameObject[] objs = new GameObject[0];

    private void Start()
    {
        foreach (var obj in objs) obj.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.root.CompareTag("Player")) return;
        foreach (var obj in objs) obj.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var obj in objs)
            Gizmos.DrawLine(transform.position, obj.transform.position);
    }
}
