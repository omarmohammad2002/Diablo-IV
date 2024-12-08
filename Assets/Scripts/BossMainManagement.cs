using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossMainManagement : MonoBehaviour
{
    private int maxHealth = 50 ;
    private int currentHealth;
    private bool minionsSummoned = false;
    private bool minionsAlive = false;

    private int shieldHealth = 50;
    private bool shieldActive = false;
    private bool reflectiveAuraActive = false;
    private int currentPhase;

    // Attacks damagee
    private int diveBombDamage = 20;
    private int bloodSpikesDamage = 30;
    private int reflectiveAuraDamage = 15;

    //prefabs
    public GameObject minionPrefab;  // Assuming minions are a GameObject prefab.
    public GameObject shieldPrefab; // Assuming shield is a GameObject prefab.
    private GameObject activeShield;
    private Animator animator;
    private bool inPhase;
    private GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentPhase = 1;
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPhase == 1 && currentHealth < maxHealth && !inPhase) //this ensures wanderer attacks first
        {
            StartCombat();
            inPhase = true;

        }

        // check for alive minions
        GameObject minion = GameObject.FindGameObjectWithTag("Minion");
        if (minion == null)
        {
            minionsAlive = false;
            minionsSummoned = false;
        }
       
    }
    public void StartCombat()
    {
        InvokeRepeating("Phase1Behavior", 0f, 30f);

    }

    private void Phase1Behavior()
    {
        if (!minionsSummoned)
        {
            SummonMinions();
        }
        else
        {
            DiveBombAttack();
        }
    }
    private void SummonMinions()
    {
        animator.SetTrigger("Summon");
        Debug.Log("Lilith summons Minions!");

        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
            Instantiate(minionPrefab, spawnPosition, Quaternion.identity);
        }

        minionsSummoned = true;
        minionsAlive = true;
    }
    private void DiveBombAttack()
    {
        // Logic for divebomb attack
        animator.SetTrigger("Divebomb");
        Debug.Log("Lilith performs Divebomb!");
        // Simulating damage to Wanderer
        
        // add ai navigation to try to attack the player
        if (Vector3.Distance(transform.position, Player.transform.position) < 10f)
        {
            Player.GetComponent<WandererMainManagement>().DealDamage(diveBombDamage);
        }
    }

    public void TransitionToNextPhase()
    {
        Debug.Log("Lilith transitions to Phase 2!");
        currentHealth = maxHealth;
        currentPhase = 2;
        shieldActive = true;
        if (shieldPrefab != null)
        {
            // Instantiate the shield as a child of the boss
            activeShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity, transform);
            Debug.Log("Shield instantiated as a child of the boss.");
        }
        CancelInvoke("Phase1Behavior");
        InvokeRepeating("Phase2Behavior", 0f, 30f);
    }
    private void Phase2Behavior()
    {
        int randomInt = UnityEngine.Random.Range(0, 2); 

        if (randomInt == 0 && !reflectiveAuraActive)
        {
            reflectiveAuraActive = true;
            animator.SetTrigger("Aura");
        }
        else if (!reflectiveAuraActive) {
        
            BloodSpikesAttack();
        }

    }

    private void BloodSpikesAttack()
    {
        // Logic for Blood Spikes attack
        animator.SetTrigger("Spikes");
        Debug.Log("Lilith performs Blood Spikes!");
        // Simulate attack (area of effect)
        if (Vector3.Distance(transform.position, Player.transform.position) < 10f)
        {
            Player.GetComponent<WandererMainManagement>().DealDamage(bloodSpikesDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (reflectiveAuraActive)
        {
            Player.GetComponent<WandererMainManagement>().DealDamage(reflectiveAuraDamage);
            reflectiveAuraActive = false;
        }
        else
        {
            if (!minionsAlive)
            {
                if (!shieldActive)
                {
                    currentHealth -= damage;
                    currentHealth = Mathf.Max(currentHealth, 0);
                }
                else
                {
                    if (damage >= shieldHealth)
                    {
                        damage -= shieldHealth;
                        shieldHealth = 0;
                        shieldActive = false;
                        Destroy(activeShield);
                        currentHealth -= damage;
                    }
                    else
                    {
                        shieldHealth -= damage;
                    }

                }
            }
            if (currentPhase == 1 && currentHealth <= 0)
            {
                //death animation
                TransitionToNextPhase();
                //resurrection in the center of the arena
            }
            if (currentPhase == 2 && currentHealth <= 0)
            {
                Die();
            }
        }
    }
    public void Die()
    {
        //death animation
        //Game ends and studio credits roll
        Destroy(gameObject);
    }
}
