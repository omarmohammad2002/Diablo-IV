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
    private readonly float chaseRange = 15f;
    private readonly float suspiciousTime = 3f;
    private float timeSinceLastSawPlayer;
    public GameObject player;
    private WandererMainManagement WandererMainManagement;

    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        managementScript = GetComponent<MinionsMainManagement>();
        player = GameObject.FindGameObjectWithTag("Player");
        WandererMainManagement = player.GetComponent<WandererMainManagement>();
        timeSinceLastSawPlayer = suspiciousTime;
        enemyAnimator.SetInteger("minionState", 0);

        if (enemyAgent == null)
        {
            Debug.LogError("NavMeshAgent component not found attached to " + gameObject.name);
        }
    }

    void Update()
    {
        if (managementScript.isDead || managementScript.currentState == MinionsMainManagement.MinionState.Stopped)
        {
            // If the minion is dead or stopped, do nothing
            enemyAgent.isStopped = true;
            enemyAgent.velocity = Vector3.zero;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (managementScript.currentState)
        {
            case MinionsMainManagement.MinionState.Idle:
                enemyAgent.SetDestination(idlePoint.position);

                if (enemyAgent.remainingDistance <= 0.1f && !enemyAgent.pathPending)
                {
                    enemyAnimator.SetInteger("minionState", 0); // Idle animation
                    enemyAgent.isStopped = true;
                    enemyAgent.velocity = Vector3.zero;
                }
                else
                {
                    enemyAgent.isStopped = false;
                    enemyAnimator.SetInteger("minionState", 1); // Walking animation
                }

                if (distanceToPlayer <= chaseRange && WandererMainManagement.enemiesFollowing < 5)
                {
                    managementScript.currentState = MinionsMainManagement.MinionState.Aggressive;
                    WandererMainManagement.enemiesFollowing++;
                }
                break;

            case MinionsMainManagement.MinionState.Aggressive:
                enemyAgent.SetDestination(player.transform.position);
                enemyAgent.isStopped = false;
                enemyAnimator.SetInteger("minionState", 1); // Running animation

                if (distanceToPlayer > chaseRange)
                {
                    enemyAgent.isStopped = true;
                    enemyAgent.velocity = Vector3.zero;
                    timeSinceLastSawPlayer -= Time.deltaTime;
                    enemyAnimator.SetInteger("minionState", 0);

                    if (timeSinceLastSawPlayer <= 0)
                    {
                        WandererMainManagement.enemiesFollowing--;
                        managementScript.currentState = MinionsMainManagement.MinionState.Idle;
                        timeSinceLastSawPlayer = suspiciousTime;
                    }
                }
                break;
        }
    }
}
