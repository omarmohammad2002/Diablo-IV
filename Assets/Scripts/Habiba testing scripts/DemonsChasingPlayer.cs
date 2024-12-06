using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemonsChasingPlayer : MonoBehaviour
{
    [SerializeField] public Transform patrolPoints;
    private readonly float waitAtPoint=2f;
    private int currentPatrolPoint;
    private float waitCounter;
    private NavMeshAgent enemyAgent;
    private Animator enemyAnimator;
    private DemonsMainManagement managementScript;
    private DemonsMainManagement.DemonState currentState;
    private readonly float chaseRange = 10f;
    private readonly float suspiciousTime=3f;
    private float timeSinceLastSawPlayer;
    private GameObject player;

     void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        managementScript = GetComponent<DemonsMainManagement>();
        player = GameObject.FindGameObjectWithTag("Player");
        waitCounter = waitAtPoint;
        timeSinceLastSawPlayer = suspiciousTime;
        enemyAnimator.SetInteger("demonState", 0);


        if (managementScript != null)
        {
            currentState = managementScript.currentState;
        }
        
    }

     void Update()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch(currentState){
            case DemonsMainManagement.DemonState.Idle:
               if(waitCounter > 0)
               {
                   waitCounter -= Time.deltaTime;
               }
               else
               {
                   currentState = DemonsMainManagement.DemonState.Patrolling;
                   enemyAnimator.SetInteger("demonState", 1);
                   enemyAgent.SetDestination(patrolPoints.GetChild(currentPatrolPoint).position);
                   
                }
                if(distanceToPlayer <= chaseRange)
                {
                    currentState = DemonsMainManagement.DemonState.Aggressive;
                    enemyAnimator.SetInteger("demonState", 2);
                }
                break;
            case DemonsMainManagement.DemonState.Patrolling:
                if(enemyAgent.remainingDistance <= 0.2f)
                {
                    currentPatrolPoint++;
                    if(currentPatrolPoint >= patrolPoints.childCount)
                    {
                        currentPatrolPoint = 0;
                    }
                    currentState = DemonsMainManagement.DemonState.Idle;
                    enemyAnimator.SetInteger("demonState", 0);
                    waitCounter = waitAtPoint;
                }

                  if(distanceToPlayer <= chaseRange)
                {
                    currentState = DemonsMainManagement.DemonState.Aggressive;
                    enemyAnimator.SetInteger("demonState", 2);
                }
                break;

            case DemonsMainManagement.DemonState.Aggressive:
                enemyAgent.SetDestination(player.transform.position);
                if(distanceToPlayer > chaseRange)
                {
                    enemyAnimator.SetInteger("demonState", 0);
                    enemyAgent.isStopped = true;
                    enemyAgent.velocity = Vector3.zero;
                    timeSinceLastSawPlayer -= Time.deltaTime;
                    if(timeSinceLastSawPlayer <= 0)
                    {
                        currentState = DemonsMainManagement.DemonState.Idle;
                        enemyAnimator.SetInteger("demonState", 0);
                        timeSinceLastSawPlayer = suspiciousTime;
                        enemyAgent.isStopped = false;
                    }
                }
                break;
        }
    
        
    }



    
}
