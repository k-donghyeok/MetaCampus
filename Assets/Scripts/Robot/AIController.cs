using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public Transform startPoint; // 시작 지점
    public Transform endPoint; // 도착 지점

    public float visionRadius = 10f; // 시야 반경
    public float visionAngle = 60f; // 시야 각도
    public LayerMask playerLayer; // Player 레이어
    public float pauseDuration = 5f; // 멈추는 시간

    private NavMeshAgent agent;
    private bool isMovingToStartPoint = true;
    private bool isPaused = false;
    private float pauseTimer = 0f;

    public string playerTag = "Player"; // Player의 태그

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(startPoint.position);
    }

    private void Update()
    {
        if (!isPaused)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (isMovingToStartPoint)
                {
                    agent.SetDestination(endPoint.position);
                    isMovingToStartPoint = false;
                }
                else
                {
                    agent.SetDestination(startPoint.position);
                    isMovingToStartPoint = true;
                }
            }

            // Player 감지
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, visionRadius);
            foreach (Collider collider in hitColliders)
            {
                // Player인지 확인
                if (collider.CompareTag(playerTag))
                {
                    // Player의 방향 벡터 계산
                    Vector3 directionToPlayer = collider.transform.position - transform.position;
                    directionToPlayer.y = 0f; // y축 방향 제거

                    // Player가 시야 범위 내에 있는지 확인
                    if (Vector3.Angle(transform.forward, directionToPlayer) <= visionAngle / 2f)
                    {
                        // Player와 AI 사이에 장애물이 있는지 확인
                        RaycastHit hit;
                        if (Physics.Linecast(transform.position, collider.transform.position, out hit))
                        {
                            // 장애물이 벽인지 확인
                            if (hit.collider.CompareTag("Wall"))
                            {
                                // 벽 뒤에 있는 경우 감지하지 않음
                                continue;
                            }
                        }

                        // Player 감지 시 동작
                        isPaused = true;
                        pauseTimer = pauseDuration;
                        agent.isStopped = true;
                        Debug.Log("Player 감지됨!");
                        break;
                    }
                }
            }

        }
        else
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
            {
                // 멈춤 시간 종료 후 다시 이동
                isPaused = false;
                agent.isStopped = false;
                Debug.Log("이동 재개!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 시야 범위를 에디터 상에서 시각적으로 표시
        Gizmos.color = Color.red;

        // 시야의 중심 방향
        Vector3 direction = transform.forward;

        // 왼쪽 끝 레이의 회전 각도
        Quaternion leftRayRotation = Quaternion.AngleAxis(-visionAngle / 2f, Vector3.up);
        // 오른쪽 끝 레이의 회전 각도
        Quaternion rightRayRotation = Quaternion.AngleAxis(visionAngle / 2f, Vector3.up);

        // 왼쪽 레이의 방향
        Vector3 leftRayDirection = leftRayRotation * direction;
        // 오른쪽 레이의 방향
        Vector3 rightRayDirection = rightRayRotation * direction;

        // 왼쪽 레이의 시야 범위 표시
        Gizmos.DrawRay(transform.position, leftRayDirection * visionRadius);
        // 오른쪽 레이의 시야 범위 표시
        Gizmos.DrawRay(transform.position, rightRayDirection * visionRadius);
        // 중앙 레이의 시야 범위 표시
        Gizmos.DrawRay(transform.position, direction * visionRadius);

        // 삼각형 시야 범위 표시
        float halfVisionAngle = visionAngle / 2f;
        Quaternion coneRotation = Quaternion.Euler(0f, -halfVisionAngle, 0f);
        Vector3 leftConeDirection = coneRotation * direction;
        coneRotation = Quaternion.Euler(0f, halfVisionAngle, 0f);
        Vector3 rightConeDirection = coneRotation * direction;

        Gizmos.DrawLine(transform.position, transform.position + leftConeDirection * visionRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightConeDirection * visionRadius);
        Gizmos.DrawLine(transform.position + leftConeDirection * visionRadius, transform.position + rightConeDirection * visionRadius);
    }

}
