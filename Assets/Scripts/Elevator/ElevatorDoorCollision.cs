using UnityEngine;

public class ElevatorDoorCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GetComponentInParent<ElevatorController>().OnDoorCollision();
    }
}
