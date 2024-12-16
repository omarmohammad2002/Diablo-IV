using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DemonsMainManagement : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public int attackPower;
    public int xpReward;
    public int explosivePower;
    public bool demonIsDead = false;

    public enum DemonState { Idle, Patrolling, Aggressive, Stopped }
    public DemonState currentState;

    private Animator demonAnimator;
    private NavMeshAgent demonAgent;
    private DemonState previousState; // Store state before stopping

    [SerializeField] private Slider DemonHealthSlider; // Reference to the slider
    private AudioSource AudioSource;
    public AudioClip deathSound;

    void Awake()
    {
        maxHealth = 40;
        currentHealth = maxHealth;
        attackPower = 10;
        explosivePower = 15;
        xpReward = 30;
        currentState = DemonState.Patrolling;

        if (DemonHealthSlider != null)
        {
            DemonHealthSlider.maxValue = maxHealth;
            DemonHealthSlider.value = currentHealth;
        }
        AudioSource = GetComponent<AudioSource>();

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

        // Update the slider to reflect the current health
        if (DemonHealthSlider != null)
        {
            DemonHealthSlider.value = currentHealth;
        }
        
        if (currentHealth == 0)
        {
            EnemyDeath();
        }
    }

    public void EnemyDeath()
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
        AudioSource.PlayOneShot(deathSound);

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
        // Save the current state and switch to Stopped
        previousState = currentState;
        currentState = DemonState.Stopped;

        // Stop the NavMeshAgent and animations
        demonAgent.isStopped = true;
        demonAgent.ResetPath();
        demonAgent.velocity = Vector3.zero;

        if (demonAnimator != null)
        {
            demonAnimator.SetLayerWeight(4, 1);
            demonAnimator.SetBool("isStunned", true);
            demonAnimator.SetInteger("demonState", 0); // Set to idle animation
        }

        Debug.Log("Demon stopped");

        // Wait for the stop duration
        yield return new WaitForSeconds(5f);

        // Resume the previous state
        currentState = previousState;
        demonAgent.isStopped = false;
        demonAnimator.SetLayerWeight(4, 0);
        demonAnimator.SetBool("isStunned", false);
        

        Debug.Log("Demon resumed");
    }

    public void StunDemon()
    {
        Debug.Log("Demon stunned");
        StartCoroutine(StunDemonCoroutine());
    }

    private IEnumerator StunDemonCoroutine()
    {
        float originalSpeed = demonAgent.speed;
        demonAgent.speed = originalSpeed / 4; // Reduce speed
        yield return new WaitForSeconds(3f); // Stun duration
        demonAgent.speed = originalSpeed; // Restore speed
    }
}
