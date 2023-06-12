using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour
{
    private Transform[] waypoints; // ��������Ʈ �迭
    private int currentWaypointIndex; // ���� ��������Ʈ �ε���
    private float walkingSpeed; // NPC�� �̵� �ӵ�

    public void InitializeMovement(Transform[] waypoints)
    {
        this.waypoints = waypoints; // �־��� ��������Ʈ �迭�� �ʱ�ȭ
        currentWaypointIndex = 0; // ���� ��������Ʈ �ε��� �ʱ�ȭ
        walkingSpeed = Random.Range(1.5f, 2.5f); // 1.5���� 2.5 ������ ������ �̵� �ӵ� ����
        MoveToNextWaypoint(); // ���� ��������Ʈ�� �̵�
    }

    private void MoveToNextWaypoint()
    {
        if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = 0; // ������ ��������Ʈ�� �����ϸ� ó�� ��������Ʈ�� �̵�
        }

        Transform targetWaypoint = waypoints[currentWaypointIndex]; // ���� ��ǥ ��������Ʈ ��������
        float distanceToTarget = Vector3.Distance(transform.position, targetWaypoint.position); // ���� ��ġ�� ��ǥ ��������Ʈ ���� �Ÿ� ���

        float movementDuration = distanceToTarget / walkingSpeed; // �Ÿ��� �̵� �ӵ��� ������� �̵��� �ʿ��� �ð� ���

        StartCoroutine(MoveTowardsWaypoint(targetWaypoint, movementDuration)); // ��ǥ ��������Ʈ�� �̵��ϴ� �ڷ�ƾ ����
    }

    private IEnumerator MoveTowardsWaypoint(Transform targetWaypoint, float duration)
    {
        Vector3 startPosition = transform.position; // �̵� ���� ��ġ ����
        float elapsedTime = 0f; // ��� �ð� �ʱ�ȭ

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; // ��� �ð� ������Ʈ
            float t = elapsedTime / duration; // ��� �ð��� ���� ���� ���� ���

            transform.position = Vector3.Lerp(startPosition, targetWaypoint.position, t); // ������ ����Ͽ� ���� ��ġ�� ��ǥ ��������Ʈ�� �̵�
            yield return null; // ���� �����ӱ��� ���
        }

        currentWaypointIndex++; // ���� ��������Ʈ �ε����� ������Ʈ
        MoveToNextWaypoint(); // ���� ��������Ʈ�� �̵�
    }
}
