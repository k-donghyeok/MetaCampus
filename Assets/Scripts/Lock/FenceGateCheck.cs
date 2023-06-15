using UnityEngine;

public abstract class FenceGateCheck : MonoBehaviour
{
    private void Start()
    {
        if (CheckGate())
            GetComponent<Animator>().SetTrigger("Open");
    }

    protected abstract bool CheckGate();
}
