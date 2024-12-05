using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemiesChasingPlayer : MonoBehaviour
{

    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolAtPoint=2f;
    private int currentPatrolPoint;
    private float waitCounter;
    private NavMeshAgent enemyAgent;
    private MinionsDemonsMainManagement managementScript;
    private MinionsDemonsMainManagement.EnemyState currentState;
    [SerializeField] private float chaseRange = 10f;
    [SerializeField] private float suspiciousTime;
    private float timeSinceLastSawPlayer;
    private GameObject player;

    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        managementScript = GetComponent<MinionsDemonsMainManagement>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (managementScript != null)
        {
            currentState = managementScript.currentState;
        }
        
    }

    void Update()
    {
       // Example: Check the current state from the other script
        if (managementScript != null)
        {
            if (managementScript.currentState == MinionsDemonsMainManagement.EnemyState.Aggressive)
            {
                // Chase the player if in aggressive state
                enemyAgent.SetDestination(player.transform.position);
            }
        }
        
    }
}
