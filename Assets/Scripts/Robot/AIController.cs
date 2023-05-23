using UnityEngine;

public class AIController : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 3f;
    public float waypointWaitTime = 2f;
    public float detectionRadius = 5f;
    public float detectionAngle = 60f;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    private int currentWaypointIndex = 0;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private bool playerDetected = false;
    private float detectionCooldownTimer = 0f;
    private float detectionCooldownDuration = 5f;

    private void Update()
    {
        // 플레이어를 감지한 상태인 경우 감지 쿨다운 타이머를 업데이트하고, 쿨다운이 끝나면 감지 상태를 해제
        if (playerDetected)
        {
            detectionCooldownTimer += Time.deltaTime;
            if (detectionCooldownTimer >= detectionCooldownDuration)
            {
                playerDetected = false;
                detectionCooldownTimer = 0f;
            }
        }
        // AI가 대기 중이 아닌 경우 동작
        if (!isWaiting)
        {
            // 플레이어를 아직 감지하지 않은 경우
            if (!playerDetected)
            {
                // AI 주변의 플레이어 레이어를 가진 콜라이더를 검사
                Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
                if (colliders.Length > 0)
                {
                    foreach (Collider collider in colliders)
                    {
                        Vector3 directionToPlayer = collider.transform.position - transform.position;
                        directionToPlayer.y = 0f;
                        float angle = Vector3.Angle(transform.forward, directionToPlayer);
                        // 각도가 시야 각도의 절반 이하이고, AI와 플레이어 사이에 장애물이 없는 경우
                        if (angle <= detectionAngle * 0.5f)
                        {
                            if (!HasObstacleInBetween(transform.position, collider.transform.position))
                            {
                                Debug.Log("플레이어 발견!");
                                playerDetected = true;
                                break;
                            }
                        }
                    }
                }
            }
            // 플레이어를 감지하지 않은 경우 현재 웨이포인트를 향해 회전
            if (!playerDetected && currentWaypointIndex < waypoints.Length)
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
            // 플레이어를 감지하지 않은 경우 현재 웨이포인트로 이동
            if (!playerDetected)
            {
                Vector3 targetPosition = waypoints[currentWaypointIndex].position;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                // 현재 웨이포인트에 도착한 경우 대기 상태로 전환
                if (transform.position == targetPosition)
                {
                    isWaiting = true;
                    waitTimer = 0f;
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                }
            }
        }
        else
        {
            // 대기 중인 경우 대기 시간을 측정하고, 대기 시간이 끝나면 대기 상태를 해제하고 플레이어 감지 상태를 해제
            waitTimer += Time.deltaTime;
            if (waitTimer >= waypointWaitTime)
            {
                isWaiting = false;
                playerDetected = false;
            }
        }
    }
    // 두 지점 사이에 장애물이 있는지 확인
    private bool HasObstacleInBetween(Vector3 startPoint, Vector3 targetPoint)
    {
        Vector3 direction = targetPoint - startPoint;
        Ray ray = new Ray(startPoint, direction);
        float distance = Vector3.Distance(startPoint, targetPoint);

        RaycastHit[] hits = Physics.RaycastAll(ray, distance, obstacleLayer);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null && !hit.collider.isTrigger)
            {
                return true;
            }
        }

        return false;
    }
    // 시각적으로 디버그하기 위해 기즈모를 그림
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // AI의 시야 범위를 기즈모로 표시
        Vector3 detectionStart = transform.position + transform.forward * detectionRadius;
        Vector3 detectionEndLeft = transform.position + Quaternion.Euler(0f, -detectionAngle * 0.5f, 0f) * transform.forward * detectionRadius;
        Vector3 detectionEndRight = transform.position + Quaternion.Euler(0f, detectionAngle * 0.5f, 0f) * transform.forward * detectionRadius;
        
        // AI의 시야 범위를 선으로 그림
        Gizmos.DrawLine(transform.position, detectionStart);
        Gizmos.DrawLine(transform.position, detectionEndLeft);
        Gizmos.DrawLine(transform.position, detectionEndRight);
        Gizmos.DrawLine(detectionEndLeft, detectionEndRight);
    }
}