using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossMainManagement : MonoBehaviour
{
    private int maxHealth = 50 ;
    public int currentHealth;
    public bool minionsSummoned = false;
    public bool minionsAlive = false;

    private int shieldHealth = 50;
    public bool shieldActive = false;
    public bool reflectiveAuraActive = false;
    private int currentPhase;

    // Attacks damagee
    private int diveBombDamage = 20;
    private int bloodSpikesDamage = 30;
    private int reflectiveAuraDamage = 15;

    //prefabs
    public GameObject minionPrefab;  // Assuming minions are a GameObject prefab.
    public GameObject shieldPrefab; // Assuming shield is a GameObject prefab.
    public GameObject bloodSpikesPrefab; // Assign the Blood Spikes prefab in the Inspector
    public GameObject reflectiveAuraPrefab;


    private GameObject activeShield;
    private Animator animator;
    private bool inPhase;
    private GameObject Player;
    private GameObject activeAura;

    public AudioClip summonSound;
    public AudioClip diveBombSound;
    public AudioClip spikesSound;
    public AudioClip auraSound;
    public AudioClip dyingSound;

    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentPhase = 1;
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound(string soundName)
    {
        switch (soundName)
        {
            case "Summon":
                audioSource.PlayOneShot(summonSound);
                break;
            case "DiveBomb":
                audioSource.PlayOneShot(diveBombSound);
                break;
            case "Spikes":
                audioSource.PlayOneShot(spikesSound);
                break;
            case "Aura":
                audioSource.PlayOneShot(auraSound);
                break;
            case "Dying":
                audioSource.PlayOneShot(dyingSound);
                break;
            default:
                Debug.LogWarning("Sound not found: " + soundName);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPhase == 1 && currentHealth < maxHealth && !inPhase) //this ensures wanderer attacks first
        {
            StartCombat();
            inPhase = true;

        }
        //for debugging purposes
        //if (!inPhase)
        //{
        //    inPhase = true;
        //    InvokeRepeating("Phase1Behavior", 0f, 15f);
        //}


        // check for alive minions
        GameObject minion = GameObject.FindGameObjectWithTag("Minion");
        if (minion == null)
        {
            minionsAlive = false;
            minionsSummoned = false;
        }
        // Rotate to face the player
        FacePlayer();
    }

    private void FacePlayer()
    {
        if (Player == null) return; // Ensure the player exists
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Boss Divebomb")) return;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Boss Swinging Hands")) return;

        // Calculate the direction to the player
        Vector3 directionToPlayer = Player.transform.position - transform.position;
        directionToPlayer.y = 0; // Ignore height differences

        // Rotate the boss to face the player
        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // Adjust rotation speed with 5f
        }
    }

    public void StartCombat()
    {
        //InvokeRepeating("Phase1Behavior", 0f, 30f);
        StartCoroutine(InstantiateSpikesWithDelay(2f));
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
            StartCoroutine(InstantiateAuraWithDelay(2f)); // Hardcoded delay for Aura instantiation
        }
        else if (!reflectiveAuraActive)
        {
            StartCoroutine(InstantiateSpikesWithDelay(2f)); // Hardcoded delay for Blood Spikes instantiation
        }
    }
    private IEnumerator InstantiateAuraWithDelay(float delay)
    {
        // Trigger the Aura animation
        animator.SetTrigger("Aura");
        Debug.Log("Lilith prepares Aura!");

        // Wait for the hardcoded delay
        yield return new WaitForSeconds(delay);

        // Instantiate the Aura
        reflectiveAuraActive = true;
        activeAura = Instantiate(reflectiveAuraPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity, transform);
        Debug.Log("Aura instantiated!");
    }


    private IEnumerator InstantiateSpikesWithDelay(float delay)
    {
        // Trigger the Blood Spikes animation
        animator.SetTrigger("Spikes");
        Debug.Log("Lilith prepares Blood Spikes!");

        // Wait for the hardcoded delay
        yield return new WaitForSeconds(delay);

        // Find the spikes GameObject as a child of the boss
        Transform spikesTransform = transform.Find("Spikes"); // Ensure the child is named "BloodSpikes"
        if (spikesTransform != null)
        {
            GameObject bloodSpikes = spikesTransform.gameObject;

            // Detach the spikes to prevent them from following the boss's rotation
            bloodSpikes.transform.SetParent(null);

            // Enable the spikes
            bloodSpikes.SetActive(true);
            Debug.Log("Blood Spikes enabled!");

            // Damage logic (if needed)
            Collider[] hitColliders = Physics.OverlapSphere(bloodSpikes.transform.position, 5);
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    WandererMainManagement player = hitCollider.GetComponent<WandererMainManagement>();
                    if (player != null)
                    {
                        player.DealDamage(bloodSpikesDamage);
                        Debug.Log("Blood Spikes damaged the player!");
                    }
                }
            }

            // Wait for 5 seconds before disabling the spikes
            yield return new WaitForSeconds(5f);

            // Disable the spikes
            bloodSpikes.SetActive(false);
            Debug.Log("Blood Spikes disabled!");

            // Reattach the spikes to the boss
            bloodSpikes.transform.SetParent(transform);
        }
        else
        {
            Debug.LogError("Blood Spikes child object not found!");
        }
    }



    public void TakeDamage(int damage)
    {
        if (reflectiveAuraActive)
        {
            Player.GetComponent<WandererMainManagement>().DealDamage(reflectiveAuraDamage);
            reflectiveAuraActive = false;
            Destroy(activeAura);
        }
        else
        {
            //if (!minionsAlive)
            //{
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
            //}
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
