using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour
{
    private Transform[] waypoints; // 웨이포인트 배열
    private int currentWaypointIndex; // 현재 웨이포인트 인덱스
    private float walkingSpeed; // NPC의 이동 속도

    public void InitializeMovement(Transform[] waypoints)
    {
        this.waypoints = waypoints; // 주어진 웨이포인트 배열로 초기화
        currentWaypointIndex = 0; // 현재 웨이포인트 인덱스 초기화
        walkingSpeed = Random.Range(1.5f, 2.5f); // 1.5에서 2.5 사이의 임의의 이동 속도 설정
        MoveToNextWaypoint(); // 다음 웨이포인트로 이동
    }

    private void MoveToNextWaypoint()
    {
        if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = 0; // 마지막 웨이포인트에 도달하면 처음 웨이포인트로 이동
        }

        Transform targetWaypoint = waypoints[currentWaypointIndex]; // 현재 목표 웨이포인트 가져오기
        float distanceToTarget = Vector3.Distance(transform.position, targetWaypoint.position); // 현재 위치와 목표 웨이포인트 간의 거리 계산

        float movementDuration = distanceToTarget / walkingSpeed; // 거리와 이동 속도를 기반으로 이동에 필요한 시간 계산

        StartCoroutine(MoveTowardsWaypoint(targetWaypoint, movementDuration)); // 목표 웨이포인트로 이동하는 코루틴 시작
    }

    private IEnumerator MoveTowardsWaypoint(Transform targetWaypoint, float duration)
    {
        Vector3 startPosition = transform.position; // 이동 시작 위치 저장
        float elapsedTime = 0f; // 경과 시간 초기화

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; // 경과 시간 업데이트
            float t = elapsedTime / duration; // 경과 시간에 대한 보간 비율 계산

            transform.position = Vector3.Lerp(startPosition, targetWaypoint.position, t); // 보간을 사용하여 현재 위치를 목표 웨이포인트로 이동
            yield return null; // 다음 프레임까지 대기
        }

        currentWaypointIndex++; // 다음 웨이포인트 인덱스로 업데이트
        MoveToNextWaypoint(); // 다음 웨이포인트로 이동
    }
}
