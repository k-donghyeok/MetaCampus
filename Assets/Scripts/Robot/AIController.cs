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
        // �÷��̾ ������ ������ ��� ���� ��ٿ� Ÿ�̸Ӹ� ������Ʈ�ϰ�, ��ٿ��� ������ ���� ���¸� ����
        if (playerDetected)
        {
            detectionCooldownTimer += Time.deltaTime;
            if (detectionCooldownTimer >= detectionCooldownDuration)
            {
                playerDetected = false;
                detectionCooldownTimer = 0f;
            }
        }
        // AI�� ��� ���� �ƴ� ��� ����
        if (!isWaiting)
        {
            // �÷��̾ ���� �������� ���� ���
            if (!playerDetected)
            {
                // AI �ֺ��� �÷��̾� ���̾ ���� �ݶ��̴��� �˻�
                Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
                if (colliders.Length > 0)
                {
                    foreach (Collider collider in colliders)
                    {
                        Vector3 directionToPlayer = collider.transform.position - transform.position;
                        directionToPlayer.y = 0f;
                        float angle = Vector3.Angle(transform.forward, directionToPlayer);
                        // ������ �þ� ������ ���� �����̰�, AI�� �÷��̾� ���̿� ��ֹ��� ���� ���
                        if (angle <= detectionAngle * 0.5f)
                        {
                            if (!HasObstacleInBetween(transform.position, collider.transform.position))
                            {
                                Debug.Log("�÷��̾� �߰�!");
                                playerDetected = true;
                                break;
                            }
                        }
                    }
                }
            }
            // �÷��̾ �������� ���� ��� ���� ��������Ʈ�� ���� ȸ��
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
            // �÷��̾ �������� ���� ��� ���� ��������Ʈ�� �̵�
            if (!playerDetected)
            {
                Vector3 targetPosition = waypoints[currentWaypointIndex].position;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                // ���� ��������Ʈ�� ������ ��� ��� ���·� ��ȯ
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
            // ��� ���� ��� ��� �ð��� �����ϰ�, ��� �ð��� ������ ��� ���¸� �����ϰ� �÷��̾� ���� ���¸� ����
            waitTimer += Time.deltaTime;
            if (waitTimer >= waypointWaitTime)
            {
                isWaiting = false;
                playerDetected = false;
            }
        }
    }
    // �� ���� ���̿� ��ֹ��� �ִ��� Ȯ��
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
    // �ð������� ������ϱ� ���� ����� �׸�
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // AI�� �þ� ������ ������ ǥ��
        Vector3 detectionStart = transform.position + transform.forward * detectionRadius;
        Vector3 detectionEndLeft = transform.position + Quaternion.Euler(0f, -detectionAngle * 0.5f, 0f) * transform.forward * detectionRadius;
        Vector3 detectionEndRight = transform.position + Quaternion.Euler(0f, detectionAngle * 0.5f, 0f) * transform.forward * detectionRadius;
        
        // AI�� �þ� ������ ������ �׸�
        Gizmos.DrawLine(transform.position, detectionStart);
        Gizmos.DrawLine(transform.position, detectionEndLeft);
        Gizmos.DrawLine(transform.position, detectionEndRight);
        Gizmos.DrawLine(detectionEndLeft, detectionEndRight);
    }
}