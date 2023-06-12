using UnityEngine;

public class CollisionInteraction : MonoBehaviour
{
    public GameObject[] toggleObjects;
    private bool isColliding = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.root.CompareTag("Player")) return;
        if (isColliding) return;
        Debug.Log("�ð�ǥ Ȯ��");
        foreach (var obj in toggleObjects) obj.SetActive(true);
        isColliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isColliding) return;
        if (!other.transform.root.CompareTag("Player")) return;
        isColliding = false;
        foreach (var obj in toggleObjects) obj.SetActive(false);
    }
}
