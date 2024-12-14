using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionsMainManagement : MonoBehaviour
{

    public int maxHealth;     
    public int currentHealth; 
    public int attackPower;   
    public int xpReward;      
    public bool isDead=false;
    public enum MinionState { Idle, Aggressive }
    public MinionState currentState;
    private Animator minionAnimator;
    private NavMeshAgent minionAgent;

    void Awake()
    {
        maxHealth = 20;
        currentHealth = maxHealth;
        attackPower = 5;
        xpReward = 10;
        currentState = MinionState.Idle;
        
    }

    void Start()
    {
        minionAnimator = GetComponent<Animator>();
        minionAgent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damage)
    {
        minionAnimator.SetBool("isDamaged", true);
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        minionAnimator.SetLayerWeight(2, 0.5f);
        if (currentHealth == 0)
        {
            EnemyDeath();
        }

       
    }

    public void EnemyDeath()
{
    // Search for the player object using the Player tag
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    
    if (player != null)
    {
        WandererMainManagement wandererMM = player.GetComponent<WandererMainManagement>();
        wandererMM.addXP(xpReward);
        if (currentState == MinionState.Aggressive)
        {
            wandererMM.enemiesFollowing--;

        }

    }
    else
    {
        Debug.LogError("Player object with tag 'Player' not found!");
    }
    
    minionAnimator.SetLayerWeight(3, 1);
    minionAnimator.SetBool("isDead", true);
    isDead = true;
    
}

 public void DestroyMinion(){
        Destroy(gameObject);
        }

        
    public void StopMinion()
    {
        StartCoroutine(StopMinionTemporarily());
    }

    private IEnumerator StopMinionTemporarily()
    {
        Debug.Log("Minion stopped");
        // TODO: fix the stopping of the minion
        minionAgent.isStopped = true;
        minionAgent.velocity = Vector3.zero;
        yield return new WaitForSeconds(5f);
        minionAgent.isStopped = false;
    }

    public void StunMinion()
    {
        StartCoroutine(StunMinionCoroutine());
    }

    private IEnumerator StunMinionCoroutine()
    {
        float originalSpeed = minionAgent.speed;
        minionAgent.speed = originalSpeed / 4;
        yield return new WaitForSeconds(3f);
        minionAgent.speed = originalSpeed;
    }


}
