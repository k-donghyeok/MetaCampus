using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    private Transform[] waypoints; // ��������Ʈ �迭
    private int currentWaypointIndex; // ���� ��������Ʈ �ε���
    private NavMeshAgent agent; // NavMeshAgent ������Ʈ
    public void InitializeMovement(Transform[] waypoints)
    {
        this.waypoints = Random.value > 0.5f ?
            waypoints : waypoints.Reverse().ToArray(); // �־��� ��������Ʈ �迭�� �ʱ�ȭ
        currentWaypointIndex = 0; // ���� ��������Ʈ �ε��� �ʱ�ȭ
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent ������Ʈ ��������
        MoveToNextWaypoint(); // ���� ��������Ʈ�� �̵�
    }

    private void MoveToNextWaypoint()
    {
        if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = 0; // ������ ��������Ʈ�� �������� ��, ù ��° ��������Ʈ�� �̵�
        }

        Transform targetWaypoint = waypoints[currentWaypointIndex]; // ���� ��ǥ ��������Ʈ ��������
        agent.SetDestination(targetWaypoint.position); // NavMeshAgent�� ������ ����

        float walkingSpeed = Random.Range(1.5f, 2.5f); // 1.5���� 2.5 ������ ������ �̵� �ӵ� ����
        agent.speed = walkingSpeed; // NPC�� �̵� �ӵ� ����

        currentWaypointIndex++; // ���� ��������Ʈ �ε����� ������Ʈ
    }

    private void Update()
    {
        if (agent != null && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            MoveToNextWaypoint(); // ���� ��������Ʈ�� �̵�
        }
    }
}