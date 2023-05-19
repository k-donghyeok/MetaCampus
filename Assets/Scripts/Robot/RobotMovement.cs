using System.Collections;
using UnityEngine;

public class RobotMovement: MonoBehaviour
{
    public Transform[] targetPoints;
    private int currentTargetIndex = 0;
    public float movementSpeed = 1.0f;
    public float pauseTime = 1.0f;

    private IEnumerator Start()
    {
        while (true)
        {
            yield return StartCoroutine(MoveToTarget(targetPoints[currentTargetIndex]));
            yield return new WaitForSeconds(pauseTime);
            currentTargetIndex = (currentTargetIndex + 1) % targetPoints.Length;
        }
    }

    private IEnumerator MoveToTarget(Transform target)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = target.position;
        float elapsedTime = 0;

        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * movementSpeed;
            yield return null;
        }

        transform.position = targetPosition;
    }
}
