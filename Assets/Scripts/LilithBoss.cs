using System.Collections;
using UnityEngine;

public class LilithBoss : AbstractBoss
{
    // Phase 1 Variables
    public int phase1Health = 50;
    public bool minionsSummoned = false;

    // Phase 2 Variables
    public int phase2Health = 50;
    private int shieldHealth = 50;
    private bool shieldActive = false;
    private bool reflectiveAuraActive = false;

    // Attacks
    public int diveBombDamage = 20;
    public int bloodSpikesDamage = 15;
    public int reflectiveAuraDamage = 15;

    public GameObject minionPrefab;  // Assuming minions are a GameObject prefab.

    public override void Start()
    {
        base.Start();
        maxHealth = phase1Health;
        bossName = "Lilith";
    }

    public override void StartCombat()
    {
        currentState = BossState.InCombat;
        // Transition to Phase 1 behaviors
        InvokeRepeating("Phase1Behavior", 0f, 3f);
    }

    private void Phase1Behavior()
    {
        if (minionsSummoned)
        {
            SummonMinions();
        }
        else
        {
            DiveBombAttack();
        }
    }

    private void DiveBombAttack()
    {
        // Logic for divebomb attack
        Debug.Log("Lilith performs Divebomb!");
        // Simulating damage to Wanderer
        if (Vector3.Distance(transform.position, WandererMainManagement.WandererMM.transform.position) < 10f)
        {
            // WandererMainManagement.WandererMM.TakeDamage(diveBombDamage);
        }
    }

    private void SummonMinions()
    {
        // Logic to summon Minions
        if (minionsSummoned) return;

        Debug.Log("Lilith summons Minions!");
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Instantiate(minionPrefab, spawnPosition, Quaternion.identity);
        }

        minionsSummoned = true;
    }

    public void DestroyMinions()
    {
        minionsSummoned = false;
        // Logic to destroy summoned minions
        var minions = FindObjectsOfType<MinionsMainManagement>();
        foreach (var minion in minions)
        {
            Destroy(minion.gameObject);
        }
    }

    public override void TransitionToNextPhase()
    {
        if (currentHealth <= 0)
        {
            // Transition to Phase 2
            if (maxHealth == phase1Health)
            {
                Debug.Log("Lilith transitions to Phase 2!");
                maxHealth = phase2Health;
                currentHealth = phase2Health;
                shieldActive = true;
                CancelInvoke("Phase1Behavior");
                InvokeRepeating("Phase2Behavior", 0f, 4f);
            }
            else
            {
                Die();
            }
        }
    }

    private void Phase2Behavior()
    {
        if (shieldActive)
        {
            // Handle Shield attack behavior (Reflective Aura and Blood Spikes)
            if (!reflectiveAuraActive)
            {
                StartReflectiveAura();
            }
            else
            {
                BloodSpikesAttack();
            }
        }
    }

    private void BloodSpikesAttack()
    {
        // Logic for Blood Spikes attack
        Debug.Log("Lilith performs Blood Spikes!");
        // Simulate attack (area of effect)
        // WandererMainManagement.WandererMM.TakeDamage(bloodSpikesDamage);
    }

    private void StartReflectiveAura()
    {
        // Start Reflective Aura behavior
        Debug.Log("Lilith casts Reflective Aura!");
        // Reflects damage logic
        reflectiveAuraActive = true;
    }

    public void DestroyShield()
    {
        shieldActive = false;
        // Shield destroyed logic
        Debug.Log("Lilith's shield is destroyed.");
    }

    public void RegenerateShield()
    {
        if (!shieldActive)
        {
            shieldActive = true;
            shieldHealth = 50;
            Debug.Log("Lilith's shield regenerates.");
        }
    }

    public override void TakeDamage(int damage)
    {
        if (shieldActive)
        {
            // Check if shield is active and reduce shield health
            shieldHealth -= damage;
            if (shieldHealth <= 0)
            {
                DestroyShield();
            }
            else
            {
                Debug.Log("Shield absorbs damage.");
            }
        }
        else
        {
            // Regular damage to Lilith
            base.TakeDamage(damage);
        }
    }
}
