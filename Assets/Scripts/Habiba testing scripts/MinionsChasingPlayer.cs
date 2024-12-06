using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionsChasingPlayer : MonoBehaviour
{
    [SerializeField] public Transform idlePoint;
    private NavMeshAgent enemyAgent;
    private Animator enemyAnimator;
    private MinionsMainManagement managementScript;
    private MinionsMainManagement.MinionState currentState;
    private readonly float chaseRange = 10f; 
    private readonly float suspiciousTime = 3f; 
    private float timeSinceLastSawPlayer; 
    private GameObject player;

    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        managementScript = GetComponent<MinionsMainManagement>();
        player = GameObject.FindGameObjectWithTag("Player");
        timeSinceLastSawPlayer = suspiciousTime;
        enemyAnimator.SetInteger("minionState", 0);

        if (managementScript != null)
        {
            currentState = managementScript.currentState;
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (currentState)
        {
            case MinionsMainManagement.MinionState.Idle:
                // Set destination to idlePoint
                enemyAgent.SetDestination(idlePoint.position);

                // Check if the agent has reached the idlePoint
                if (enemyAgent.remainingDistance <= 0.1f && !enemyAgent.pathPending)
                {
                    // Stop the agent and reset its velocity
                    enemyAnimator.SetInteger("minionState", 0);
                    enemyAgent.isStopped = true;
                    enemyAgent.velocity = Vector3.zero;
                }
                else
                {
                    // Keep walking if not yet at idlePoint
                    enemyAgent.isStopped = false;
                    enemyAnimator.SetInteger("minionState", 1); // Walking animation
                }

                // Check if the player is in the chase range
                if (distanceToPlayer <= chaseRange)
                {
                    currentState = MinionsMainManagement.MinionState.Aggressive;
                }
                break;

            case MinionsMainManagement.MinionState.Aggressive:
                // Chase the player
                enemyAgent.SetDestination(player.transform.position);
                enemyAgent.isStopped = false; // Ensure the agent is moving
                enemyAnimator.SetInteger("minionState", 1); // Walking animation

                // If the player leaves the chase range
                if (distanceToPlayer > chaseRange)
                {
                    // Stop the agent and start the suspicious timer
                    enemyAgent.isStopped = true;
                    enemyAgent.velocity = Vector3.zero;
                    timeSinceLastSawPlayer -= Time.deltaTime;

                    // Idle animation during suspicious time
                    enemyAnimator.SetInteger("minionState", 0);

                    // If the suspicious time has elapsed, return to Idle state
                    if (timeSinceLastSawPlayer <= 0)
                    {
                        currentState = MinionsMainManagement.MinionState.Idle;
                        timeSinceLastSawPlayer = suspiciousTime; // Reset the timer
                    }
                }
                break;
        }
    }
}
