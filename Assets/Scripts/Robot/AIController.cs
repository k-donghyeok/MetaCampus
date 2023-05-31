using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public Transform startPoint; // ���� ����
    public Transform endPoint; // ���� ����

    public float visionRadius = 10f; // �þ� �ݰ�
    public float visionAngle = 60f; // �þ� ����
    public LayerMask playerLayer; // Player ���̾�
    public float pauseDuration = 5f; // ���ߴ� �ð�

    private NavMeshAgent agent;
    private bool isMovingToStartPoint = true;
    private bool isPaused = false;
    private float pauseTimer = 0f;

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

            // Player ����
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, visionRadius, playerLayer);
            foreach (Collider collider in hitColliders)
            {
                // Player�� ���� ���� ���
                Vector3 directionToPlayer = collider.transform.position - transform.position;
                directionToPlayer.y = 0f; // y�� ���� ����

                // Player�� �þ� ���� ���� �ִ��� Ȯ��
                if (Vector3.Angle(transform.forward, directionToPlayer) <= visionAngle / 2f)
                {
                    // Player ���� �� ����
                    isPaused = true;
                    pauseTimer = pauseDuration;
                    agent.isStopped = true;
                    Debug.Log("Player ������!");
                    break;
                }
            }
        }
        else
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
            {
                // ���� �ð� ���� �� �ٽ� �̵�
                isPaused = false;
                agent.isStopped = false;
                Debug.Log("�̵� �簳!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // �þ� ������ ������ �󿡼� �ð������� ǥ��
        Gizmos.color = Color.red;
        Vector3 direction = transform.forward;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-visionAngle / 2f, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(visionAngle / 2f, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * direction;
        Vector3 rightRayDirection = rightRayRotation * direction;

        Gizmos.DrawRay(transform.position, leftRayDirection * visionRadius);
        Gizmos.DrawRay(transform.position, rightRayDirection * visionRadius);
        Gizmos.DrawRay(transform.position, direction * visionRadius);
        
    }
}
