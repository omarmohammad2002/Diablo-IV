using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MinionsMainManagement : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public int attackPower;
    public int xpReward;
    public bool isDead = false;
    public enum MinionState { Idle, Aggressive, Stopped }
    public MinionState currentState;

    private Animator minionAnimator;
    private NavMeshAgent minionAgent;

    private MinionState previousState; // Store the state before stopping

    [SerializeField] private Slider MinionHealthSlider; // Reference to the slider
    private AudioSource AudioSource;
    public AudioClip deathSound;

    void Awake()
    {
        maxHealth = 20;
        currentHealth = maxHealth;
        attackPower = 5;
        xpReward = 10;
        currentState = MinionState.Idle;
        AudioSource = GetComponent<AudioSource>();

         if (MinionHealthSlider != null)
        {
            MinionHealthSlider.maxValue = maxHealth;
            MinionHealthSlider.value = currentHealth;
        }
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

        // Update the slider to reflect the current health
        if (MinionHealthSlider != null)
        {
            MinionHealthSlider.value = currentHealth;
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
        AudioSource.PlayOneShot(deathSound);
        isDead = true;
    }

    public void DestroyMinion()
    {
        Destroy(gameObject);
    }

    public void StopMinion()
    {
        StartCoroutine(StopMinionTemporarily());
    }

    private IEnumerator StopMinionTemporarily()
    {
        Debug.Log("Minion stopped");

        // Save the current state and set to Stopped
        previousState = currentState;
        currentState = MinionState.Stopped;

        // Stop the NavMeshAgent and clear its path
        minionAgent.isStopped = true;
        minionAgent.ResetPath();
        minionAgent.velocity = Vector3.zero;

        // Update animations
        if (minionAnimator != null)
        {
            minionAnimator.SetLayerWeight(4, 1);
            minionAnimator.SetBool("isStunned", true);
            minionAnimator.SetInteger("minionState", 0); // Idle animation
        }

        yield return new WaitForSeconds(5f); // Stopping duration

        // Restore the previous state
        minionAnimator.SetLayerWeight(4, 0);
        minionAnimator.SetBool("isStunned", false);
        currentState = previousState;
        Debug.Log("Minion resumed movement");
    }

    public void StunMinion()
    {
        StartCoroutine(StunMinionCoroutine());
    }

    private IEnumerator StunMinionCoroutine()
    {
        float originalSpeed = minionAgent.speed;
        minionAgent.speed = originalSpeed / 4; // Reduce speed to simulate stun
        Debug.Log("Minion stunned");
        yield return new WaitForSeconds(3f); // Stun duration
        minionAgent.speed = originalSpeed; // Restore original speed
        Debug.Log("Minion stun ended");
    }
}
