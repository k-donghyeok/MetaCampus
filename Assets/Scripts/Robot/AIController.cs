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
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(waypoints[nextWaypoint].position);

        animator = GetComponent<Animator>();
        spotLight.gameObject.SetActive(true);
        spotLight.spotAngle = visionAngle;
        spotLight.range = visionRadius;

        timeManager = new TimeManager(180f); // Initialize the time manager
    }

    private void Update()
    {
        timeManager.UpdateCountdown(); // Update the time manager

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
                return; // Exit the method to prevent further state transition in the same frame
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
                Debug.Log("Resuming movement!");
            }
        }
    }

    private void UpdateDetectedState()
    {
        if (!isPaused)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                isPaused = true;
                pauseTimer = pauseDuration;
                agent.isStopped = true;
                animator.SetBool("DetectPlayer", true);
                Debug.Log("Player detected!");

                // Stay paused for the specified duration and then transition to alarm state
                Invoke(nameof(SetAlarmState), pauseDuration);
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
        Debug.Log("Alarm state activated. Decreasing time by 1 minute and halting robot operations.");

        // Additional logic for the alarm state can be added here
    }

    private void SetDetectedState()
    {
        currentState = AIState.Detected;
        isPaused = true;
        pauseTimer = pauseDuration;
        agent.isStopped = true;
        animator.SetBool("DetectPlayer", true);
        Debug.Log("Player detected!");
    }

    private void SetLostState()
    {
        currentState = AIState.Lost;
        isPaused = true;
        pauseTimer = pauseDuration;
        agent.isStopped = true;
        animator.SetBool("DetectPlayer", true);
        Debug.Log("Player lost");
    }

    private void SetNormalState()
    {
        currentState = AIState.Normal;
        isPaused = false;
        agent.isStopped = false;
        animator.SetBool("DetectPlayer", false);
        Debug.Log("Switching to normal state");
    }

    private void SetAlarmState()
    {
        currentState = AIState.Alarm;
        Debug.Log("Switching to alarm state");

        // Additional logic for the alarm state can be added here
    }
}
