using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public Transform[] waypoints;
    public float visionRadius = 10f;
    public float visionAngle = 60f;
    public LayerMask playerLayer;
    public float pauseDuration = 5f;
    public Light spotLight;

    private NavMeshAgent agent;
    private int nextWaypoint = 0;
    private bool isPaused = false;
    private float pauseTimer = 0f;

    public const string playerTag = "Player";
    public const string wallTag = "Wall";

    private Animator animator;
    private TimeManager timeManager;

    private enum AIState
    {
        Normal,
        Detected,
        Lost,
        Alarm
    }

    private AIState currentState = AIState.Normal;

    private void Start()
    {
        //if (waypoints.Length < 1 || GameManager.Instance().IsDaytime())
        //{
        //    Destroy(this);
        //    return;
        //}
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(waypoints[nextWaypoint].position);

        animator = GetComponent<Animator>();
        spotLight.gameObject.SetActive(true);
        spotLight.spotAngle = visionAngle;
        spotLight.range = visionRadius;

        timeManager = new TimeManager(70f); // 시간 관리자 초기화
    }

    private void Update()
    {
        timeManager.UpdateCountdown(); // 시간 관리자 업데이트

        switch (currentState)
        {
            case AIState.Normal:
                UpdateNormalState();
                break;
            case AIState.Detected:
                UpdateDetectedState();
                break;
            case AIState.Lost:
                UpdateLostState();
                break;
            case AIState.Alarm:
                UpdateAlarmState();
                break;
        }
    }

    private void UpdateNormalState()
    {
        if (!isPaused)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                nextWaypoint = (nextWaypoint + 1) % waypoints.Length;
                agent.SetDestination(waypoints[nextWaypoint].position);
            }

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, visionRadius);
            bool playerDetected = false;

            foreach (Collider collider in hitColliders)
            {
                if (collider.transform.root.CompareTag(playerTag))
                {
                    Vector3 directionToPlayer = collider.transform.position - transform.position;
                    directionToPlayer.y = 0f;

                    if (Vector3.Angle(transform.forward, directionToPlayer) <= visionAngle / 2f)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, directionToPlayer, out hit, visionRadius))
                        {
                            if (hit.collider.CompareTag(wallTag))
                            {
                                continue;
                            }
                        }

                        playerDetected = true;
                        break;
                    }
                }
            }

            if (playerDetected)
            {
                SetDetectedState();
            }
        }
        else
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
            {
                isPaused = false;
                agent.isStopped = false;
                animator.SetBool("DetectPlayer", false);
                Debug.Log("이동 재개!");
            }
        }
    }

    private void UpdateDetectedState()
    {
        if (!isPaused)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                SetLostState();
            }
        }
        else
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
            {
                SetLostState();
            }
        }
    }

    private void UpdateLostState()
    {
        if (!isPaused)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                SetNormalState();
            }
        }
        else
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
            {
                SetNormalState();
            }
        }
    }

    private void UpdateAlarmState()
    {
        StageManager.Instance().Time.DecreaseTimeByOneMinute();
        Debug.Log("알람 상태입니다. 시간을 1분 감소시키고 로봇 작동을 중지합니다.");
    }

    private void SetDetectedState()
    {
        currentState = AIState.Detected;
        isPaused = true;
        pauseTimer = pauseDuration;
        agent.isStopped = true;
        animator.SetBool("DetectPlayer", true);
        Debug.Log("Player 감지!");
    }

    private void SetLostState()
    {
        currentState = AIState.Lost;
        isPaused = true;
        pauseTimer = pauseDuration;
        agent.isStopped = true;
        animator.SetBool("DetectPlayer", true);
        Debug.Log("플레이어를 놓침");
    }

    private void SetNormalState()
    {
        currentState = AIState.Normal;
        isPaused = false;
        agent.isStopped = false;
        animator.SetBool("DetectPlayer", false);
        Debug.Log("평시 상태로 전환");
    }
}
