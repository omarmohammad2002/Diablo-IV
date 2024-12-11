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
    private readonly float chaseRange = 10f;
    private readonly float suspiciousTime=3f;
    private float timeSinceLastSawPlayer;
    private GameObject player;
    private WandererMainManagement playerManagementScript;

    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        managementScript = GetComponent<DemonsMainManagement>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerManagementScript = player.GetComponent<WandererMainManagement>();
        waitCounter = waitAtPoint;
        timeSinceLastSawPlayer = suspiciousTime;
        enemyAnimator.SetInteger("demonState", 0);


        
    }

     void Update()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch(managementScript.currentState){
            case DemonsMainManagement.DemonState.Idle:
               if(waitCounter > 0)
               {
                   waitCounter -= Time.deltaTime;
               }
               else
               {
                   managementScript.currentState = DemonsMainManagement.DemonState.Patrolling;
                   enemyAnimator.SetInteger("demonState", 1);
                   enemyAgent.SetDestination(patrolPoints.GetChild(currentPatrolPoint).position);
                   
                }
                if(distanceToPlayer <= chaseRange && playerManagementScript.enemiesFollowing<5)
                {
                    managementScript.currentState = DemonsMainManagement.DemonState.Aggressive;
                    enemyAnimator.SetInteger("demonState", 2);
                    playerManagementScript.enemiesFollowing++;
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
                    managementScript.currentState = DemonsMainManagement.DemonState.Idle;
                    enemyAnimator.SetInteger("demonState", 0);
                    waitCounter = waitAtPoint;
                }

                  if(distanceToPlayer <= chaseRange && playerManagementScript.enemiesFollowing < 5 )
                {
                    managementScript.currentState = DemonsMainManagement.DemonState.Aggressive;
                    enemyAnimator.SetInteger("demonState", 2);
                    playerManagementScript.enemiesFollowing++;
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
                    if (timeSinceLastSawPlayer <= 0)
                    {
                        managementScript.currentState = DemonsMainManagement.DemonState.Idle;
                        enemyAnimator.SetInteger("demonState", 0);
                        timeSinceLastSawPlayer = suspiciousTime;
                        enemyAgent.isStopped = false;
                        playerManagementScript.enemiesFollowing--;
                    }
                }
                break;
        }
    
        
    }



    
}
