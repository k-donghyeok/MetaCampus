using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    private Transform[] waypoints; // 웨이포인트 배열
    private int currentWaypointIndex; // 현재 웨이포인트 인덱스
    private NavMeshAgent agent; // NavMeshAgent 컴포넌트

    public void InitializeMovement(Transform[] waypoints)
    {
        this.waypoints = Random.value > 0.5f ?
            waypoints : waypoints.Reverse().ToArray(); // 주어진 웨이포인트 배열로 초기화
        currentWaypointIndex = 0; // 현재 웨이포인트 인덱스 초기화
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent 컴포넌트 가져오기
        float walkingSpeed = Random.Range(1.5f, 2.5f); // 1.5에서 2.5 사이의 무작위 이동 속도 설정
        agent.speed = walkingSpeed; // NPC의 이동 속도 설정
        MoveToNextWaypoint(); // 다음 웨이포인트로 이동
    }

    private void MoveToNextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        // 마지막 웨이포인트에 도달했을 때, 첫 번째 웨이포인트로 이동

        Transform targetWaypoint = waypoints[currentWaypointIndex]; // 현재 목표 웨이포인트 가져오기
        agent.SetDestination(targetWaypoint.position + RNV()); // NavMeshAgent에 목적지 설정
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
            MoveToNextWaypoint(); // 다음 웨이포인트로 이동
        }
    }
}