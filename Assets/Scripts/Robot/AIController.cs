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

    public AudioClip detectionSound;
    public AudioClip alarmSound;
    public AudioClip lostSound;
    private AudioSource audioSource;

    private enum AIState
    {
        Normal,
        Detected,
        Lost,
        Alarm
    }

    private AIState currentState = AIState.Normal;
    private bool isAlarmActivated = false;

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

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void Update()
    {
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
        if (currentState != AIState.Normal)
            return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            nextWaypoint = (nextWaypoint + 1) % waypoints.Length;
            agent.SetDestination(waypoints[nextWaypoint].position);
        }

        if (IsPlayerDetected())
        {
            SetDetectedState();
        }
    }

    private void UpdateDetectedState()
    {
        if (currentState != AIState.Detected)
            return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            SetLostState();
        }
        else if (!IsPlayerDetected())
        {
            SetLostState();
        }
        else if (isPaused)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
            {
                SetAlarmState();
            }
        }
    }

    private void UpdateLostState()
    {
        if (currentState != AIState.Lost)
            return;

        if (IsPlayerDetected())
        {
            SetDetectedState();
            Debug.Log("재발견");
            return;
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
        if (!isAlarmActivated)
        {
            isAlarmActivated = true;
            StageManager.Instance().Time.DecreaseTimeByOneMinute();
            Debug.Log("경보 상태가 활성화되었습니다. 타이머 1분 감소");

            PlaySound(alarmSound);
        }
    }

    private void SetDetectedState()
    {
        currentState = AIState.Detected;
        isPaused = true;
        pauseTimer = pauseDuration;
        agent.isStopped = true;
        animator.SetBool("LostState", false);
        animator.SetBool("DetectPlayer", true);
        //Debug.Log("플레이어 발견!");

        PlaySound(detectionSound);
    }

    private void SetLostState()
    {
        currentState = AIState.Lost;
        isPaused = true;
        pauseTimer = pauseDuration;
        agent.isStopped = true;
        animator.SetBool("LostState", true);
        animator.SetBool("DetectPlayer", false);
        //Debug.Log("플레이어를 놓침");

        PlaySound(lostSound);
    }

    private void SetNormalState()
    {
        currentState = AIState.Normal;
        isPaused = false;
        agent.isStopped = false;
        animator.SetBool("LostState", false);
        animator.SetBool("DetectPlayer", false);
        //Debug.Log("평시 상태로 전환");

    }

    private void SetAlarmState()
    {
        currentState = AIState.Alarm;
        //Debug.Log("알람 상태로 전환");
    }

    private bool IsPlayerDetected()
    {
        //if (GameManager.Instance().IsDaytime()) return false; // 낮에는 플레이어 무시

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, visionRadius);

        foreach (Collider collider in hitColliders)
        {
            if (collider.transform.root.CompareTag(playerTag))
            {
                Vector3 directionToPlayer = collider.transform.position - transform.position;
                float heightDifference = Mathf.Abs(directionToPlayer.y);
                if (heightDifference > 2.4f) continue; // Different floor threshold
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

                    return true;
                }
            }
        }

        return false;
    }
}