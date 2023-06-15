using UnityEngine;

public class TriggerToggleDisplay : TriggerEnterDisplay
{
    private void OnTriggerExit(Collider other)
    {
        if (!other.transform.root.CompareTag("Player")) return;
        foreach (var obj in objs) obj.SetActive(false);
    }

}
