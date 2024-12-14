using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemonsMainManagement : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public int attackPower;
    public int xpReward;
    public int explosivePower;
    public bool demonIsDead = false;
    public enum DemonState { Idle, Patrolling, Aggressive }
    public DemonState currentState;
    private Animator demonAnimator;
    private NavMeshAgent demonAgent;

    void Awake()
    {
        maxHealth = 40;
        currentHealth = maxHealth;
        attackPower = 10;
        explosivePower = 15;
        xpReward = 30;
        currentState = DemonState.Patrolling;
    }

    void Start()
    {
        demonAnimator = GetComponent<Animator>();
        demonAgent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damage)
    {
        demonAnimator.SetBool("isDamaged", true);
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        demonAnimator.SetLayerWeight(2, 0.5f);
        if (currentHealth == 0)
        {
            enemyDeath();
        }
    }

    public void enemyDeath()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            WandererMainManagement wandererMM = player.GetComponent<WandererMainManagement>();
            wandererMM.addXP(xpReward);
            if (currentState == DemonState.Aggressive)
                {
                    wandererMM.enemiesFollowing--;
                }
        }
        else
        {
            Debug.LogError("Player object with tag 'Player' not found!");
        }

        demonAnimator.SetLayerWeight(3, 1);
        demonIsDead = true;
        demonAnimator.SetBool("isDead", true);
    }

    public void DestroyDemon()
    {
        Destroy(gameObject);
    }

    public void StopDemon()
    {
        StartCoroutine(StopDemonTemporarily());
    }

    private IEnumerator StopDemonTemporarily()
    {
        demonAgent.isStopped = true;
        demonAgent.velocity = Vector3.zero;
        yield return new WaitForSeconds(5f);
        demonAgent.isStopped = false;
    }

    public void StunDemon()
    {
        Debug.Log("Demon Stunned");
        StartCoroutine(StunDemonCoroutine());
    }

    private IEnumerator StunDemonCoroutine()
    {
        float originalSpeed = demonAgent.speed;
        demonAgent.speed = originalSpeed / 4;
        yield return new WaitForSeconds(3f);
        demonAgent.speed = originalSpeed;
    }
}
