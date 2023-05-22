using UnityEngine;

public class AIController : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 3f;
    public float waypointWaitTime = 2f;
    public float detectionRadius = 5f;
    public float detectionAngle = 60f;
    public LayerMask playerLayer;

    private int currentWaypointIndex = 0;
    private bool isWaiting = false;
    private float waitTimer = 0f;

    private void Update()
    {
        if (!isWaiting)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
            if (colliders.Length > 0)
            {
                foreach (Collider collider in colliders)
                {
                    Vector3 directionToPlayer = collider.transform.position - transform.position;
                    directionToPlayer.y = 0f;
                    float angle = Vector3.Angle(transform.forward, directionToPlayer);

                    if (angle <= detectionAngle * 0.5f)
                    {
                        Debug.Log("Player Detected!");
                        break;
                    }
                }
            }

            if (currentWaypointIndex < waypoints.Length)
            {
                Vector3 currentWaypointPosition = waypoints[currentWaypointIndex].position;
                Vector3 directionToWaypoint = currentWaypointPosition - transform.position;
                directionToWaypoint.y = 0f;
                if (directionToWaypoint != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToWaypoint);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360f * Time.deltaTime);
                }
            }

            Vector3 targetPosition = waypoints[currentWaypointIndex].position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (transform.position == targetPosition)
            {
                isWaiting = true;
                waitTimer = 0f;
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
        else
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waypointWaitTime)
            {
                isWaiting = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Vector3 detectionStart = transform.position + transform.forward * detectionRadius;
        Vector3 detectionEndLeft = transform.position + Quaternion.Euler(0f, -detectionAngle * 0.5f, 0f) * transform.forward * detectionRadius;
        Vector3 detectionEndRight = transform.position + Quaternion.Euler(0f, detectionAngle * 0.5f, 0f) * transform.forward * detectionRadius;

        Gizmos.DrawLine(transform.position, detectionStart);
        Gizmos.DrawLine(transform.position, detectionEndLeft);
        Gizmos.DrawLine(transform.position, detectionEndRight);
        Gizmos.DrawLine(detectionEndLeft, detectionEndRight);
    }
}