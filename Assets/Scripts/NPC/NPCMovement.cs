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
        float walkingSpeed = Random.Range(1.5f, 2.5f); // 1.5���� 2.5 ������ ������ �̵� �ӵ� ����
        agent.speed = walkingSpeed; // NPC�� �̵� �ӵ� ����
        MoveToNextWaypoint(); // ���� ��������Ʈ�� �̵�
    }

    private void MoveToNextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        // ������ ��������Ʈ�� �������� ��, ù ��° ��������Ʈ�� �̵�

        Transform targetWaypoint = waypoints[currentWaypointIndex]; // ���� ��ǥ ��������Ʈ ��������
        agent.SetDestination(targetWaypoint.position + RNV()); // NavMeshAgent�� ������ ����
    }

    public static Vector3 RNV()
    {
        const float PI2 = Mathf.PI * 2f;
        float rndAngle = Random.Range(0f, PI2);
        float rndDist = Random.Range(0f, 3f);
        return rndDist * new Vector3(Mathf.Sin(rndAngle), 0f, Mathf.Cos(rndAngle));
    }

    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            MoveToNextWaypoint(); // ���� ��������Ʈ�� �̵�
        }
    }
}