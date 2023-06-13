using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour
{
    private Transform[] waypoints; // array of waypoints
    private int currentWaypointIndex; // current waypoint index
    private float walkingSpeed; // NPC movement speed
    private float distanceToTarget; // distance to target waypoint

    public void InitializeMovement(Transform[] waypoints)
    {
        this.waypoints = waypoints; // Initialize with the given waypoint array
        currentWaypointIndex = 0; // Initialize the current waypoint index
        walkingSpeed = Random.Range(1.5f, 2.5f); // set random movement speed between 1.5 and 2.5
        MoveToNextWaypoint(); // go to next waypoint
    }

    private void MoveToNextWaypoint()
    {
        if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = 0; // When the last waypoint is reached, go to the first waypoint
        }

        Transform targetWaypoint = waypoints[currentWaypointIndex]; // Get the current target waypoint
        distanceToTarget = Vector3.Distance(transform.position, targetWaypoint.position); // Calculate the distance between the current position and the target waypoint

        float movementDuration = distanceToTarget / walkingSpeed; // Calculate time needed to move based on distance and speed of movement

        StartCoroutine(MoveTowardsWaypoint(targetWaypoint, movementDuration)); // start a coroutine that moves to the target waypoint
    }

    private IEnumerator MoveTowardsWaypoint(Transform targetWaypoint, float duration)
    {
        Vector3 startPosition = transform.position; // save the move start position
        float elapsedTime = 0f; // reset elapsed time

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; // update elapsed time
            float t = elapsedTime / duration; // Calculate the interpolation rate for elapsed time

            // Calculate the desired position based on the target waypoint
            Vector3 desiredPosition = Vector3.Lerp(startPosition, targetWaypoint.position, t);

            // Check for obstacles and perform avoidance
            RaycastHit hit;
            if (Physics.Raycast(transform.position, desiredPosition - transform.position, out hit, distanceToTarget))
            {
                // Calculate a new desired position to avoid the obstacle
                desiredPosition = hit.point + hit.normal * 0.5f; // Adjust the desired position by adding the normal of the hit point multiplied by a factor

                // Calculate a new movement duration based on the adjusted desired position
                float newDistanceToTarget = Vector3.Distance(transform.position, desiredPosition);
                float newMovementDuration = newDistanceToTarget / walkingSpeed;

                // Update the movement duration and start the coroutine with the adjusted desired position
                duration = newMovementDuration;
                StartCoroutine(MoveTowardsWaypoint(targetWaypoint, duration));
                yield break; // Exit the current coroutine iteration
            }

            transform.position = desiredPosition; // move the current position to the desired position
            yield return null; // wait until next frame
        }

        currentWaypointIndex++; // update to next waypoint index
        MoveToNextWaypoint(); // go to next waypoint
    }
}
