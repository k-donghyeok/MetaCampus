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

    public const string playerTag = "Player"; // Player�� �±�

    private Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(startPoint.position);

        animator = GetComponent<Animator>();
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
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, visionRadius);
            bool playerDetected = false;

            foreach (Collider collider in hitColliders)
            {
                // Player���� Ȯ��
                if (collider.transform.root.CompareTag(playerTag))
                {
                    // Player�� ���� ���� ���
                    Vector3 directionToPlayer = collider.transform.position - transform.position;
                    directionToPlayer.y = 0f; // y�� ���� ����

                    // Player�� �þ� ���� ���� �ִ��� Ȯ��
                    if (Vector3.Angle(transform.forward, directionToPlayer) <= visionAngle / 2f)
                    {
                        // Player�� AI ���̿� ��ֹ��� �ִ��� Ȯ��
                        RaycastHit hit;
                        if (Physics.Linecast(transform.position, collider.transform.position, out hit))
                        {
                            // ��ֹ��� ������ Ȯ��
                            if (hit.collider.CompareTag("Wall"))
                            {
                                // �� �ڿ� �ִ� ��� �������� ����
                                continue;
                            }
                        }

                        // Player detected
                        playerDetected = true;
                        break;
                    }
                }
            }

            if (playerDetected)
            {
                // Player detected action
                isPaused = true;
                pauseTimer = pauseDuration;
                agent.isStopped = true;
                animator.SetBool("DetectPlayer", true); // �ִϸ��̼� ���� ����
                Debug.Log("Player ����!");
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
                animator.SetBool("DetectPlayer", false); // �ִϸ��̼� ���� ����
                Debug.Log("�̵� �簳!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // �þ� ������ ������ �󿡼� �ð������� ǥ��
        Gizmos.color = Color.red;

        // �þ��� �߽� ����
        Vector3 direction = transform.forward;

        // ���� �� ������ ȸ�� ����
        Quaternion leftRayRotation = Quaternion.AngleAxis(-visionAngle / 2f, Vector3.up);
        // ������ �� ������ ȸ�� ����
        Quaternion rightRayRotation = Quaternion.AngleAxis(visionAngle / 2f, Vector3.up);

        // ���� ������ ����
        Vector3 leftRayDirection = leftRayRotation * direction;
        // ������ ������ ����
        Vector3 rightRayDirection = rightRayRotation * direction;

        // ���� ������ �þ� ���� ǥ��
        Gizmos.DrawRay(transform.position, leftRayDirection * visionRadius);
        // ������ ������ �þ� ���� ǥ��
        Gizmos.DrawRay(transform.position, rightRayDirection * visionRadius);
        // �߾� ������ �þ� ���� ǥ��
        Gizmos.DrawRay(transform.position, direction * visionRadius);

        // �ﰢ�� �þ� ���� ǥ��
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
