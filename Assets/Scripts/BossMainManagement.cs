using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI; // For UI elements like sliders

public class BossMainManagement : MonoBehaviour
{
     [Header("UI Sliders")]
    [SerializeField] private Slider bossHealthSlider; // Slider for boss health
    [SerializeField] private Slider shieldHealthSlider; // Slider for shield health
    private int maxHealth = 50;
    public int currentHealth;
    public bool minionsAlive = false;

    public int shieldHealth = 50;
    public bool shieldActive = false;
    public bool reflectiveAuraActive = false;
    private int currentPhase;
    private bool inPhase;

    // Attacks damagee
    private int diveBombDamage = 20;
    private int bloodSpikesDamage = 30;
    private int reflectiveAuraDamage = 15;

    //prefabs
    public GameObject minionPrefab;  // Assuming minions are a GameObject prefab.
    public GameObject shieldPrefab; // Assuming shield is a GameObject prefab.
    public GameObject healingPotionPrefab;
    public GameObject reflectiveAuraPrefab;
    public Transform minionIdlePointPrefab;
    public GameObject arenaGround;
    private GameObject activeShield;
    private Animator animator;

    private GameObject Player;
    private GameObject activeAura;

    // audio
    public AudioClip summonSound;
    public AudioClip diveBombSound;
    public AudioClip spikesSound;
    public AudioClip auraSound;
    public AudioClip dyingSound;
    public AudioClip damagingSound;

    private AudioSource audioSource;

    // for stunnning
    private bool isStunned = false; // Track if the boss is stunned
    private Coroutine stunCoroutine; // Reference to the active stun coroutine
    private bool canRotate = true; // Control whether the boss can rotate

    //for slowing down
    private bool isSlowed = false; // Track if the boss is slowed
    private Coroutine slowCoroutine; // Reference to the active slow coroutine
    private float originalSpeed = 5f; // Original rotation speed
    private float slowedSpeed; // Slowed rotation speed
    public bool Done;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentPhase = 0;
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
        slowedSpeed = originalSpeed * 0.25f;
        SpawnHealingPotions();
        // Initialize sliders
        InitializeSliders();

    }

    public void PlaySound(string soundName)
    {
        switch (soundName)
        {
            case "Summon":
                audioSource.PlayOneShot(summonSound);
                Debug.Log("Summon sound played");
                break;
            case "DiveBomb":
                audioSource.PlayOneShot(diveBombSound);
                Debug.Log("Divebomb sound played");
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
            case "Damaging":
                audioSource.PlayOneShot(damagingSound);
                break;
            case "None":
                break;
            default:
                Debug.LogWarning("Sound not found: " + soundName);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
         UpdateSliders();
        if (currentPhase == 0 && currentHealth < maxHealth && !inPhase) //this ensures wanderer attacks first
        {
            currentPhase = 1;
            StartCoroutine(StartCombat());
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
        }
        // Rotate to face the player
        if (canRotate)
        {
            FacePlayer();
        }
    }

    private void InitializeSliders()
    {
        // Initialize boss health slider
        if (bossHealthSlider != null)
        {
            bossHealthSlider.maxValue = maxHealth;
            bossHealthSlider.value = currentHealth;
        }      
        
    }
     private void UpdateSliders()
    {
        // Update boss health slider
        if (bossHealthSlider != null)
        {
            bossHealthSlider.value = currentHealth;
        }

        // Update shield health slider
        if (shieldHealthSlider != null)
        {
            shieldHealthSlider.maxValue = 50;
            shieldHealthSlider.value = shieldHealth;
            Debug.Log("UPDATE VALUE shieldhealth" + shieldHealthSlider.value );
            Debug.Log("UPDATE SHIELDHEALTH VAL" + shieldHealth);
            Debug.Log("UPDATE Max VAL" + shieldHealthSlider.maxValue);
            shieldHealthSlider.gameObject.SetActive(shieldActive); // Show/hide based on shield status
        }
    }

    private void FacePlayer()
    {
        if (Player == null || isStunned) return; // Ensure the player exists and the boss is not stunned

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Boss Divebomb")) return;

        // Calculate the direction to the player
        Vector3 directionToPlayer = Player.transform.position - transform.position;
        directionToPlayer.y = 0; // Ignore height differences

        // Rotate the boss to face the player using the slowed or original speed
        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * (isSlowed ? slowedSpeed : originalSpeed));
        }
    }
    private void TriggerAttackAnimation(string attackType)
    {
        switch (attackType)
        {
            case "Summon":
                SummonMinions();
                break;

            case "DiveBomb":
                DiveBombAttack();
                break;

            case "Spikes":
                StartCoroutine(Spikes());
                break;

            case "Aura":
                Aura();
                break;
            case "Resurrection":
                Resurrection();
                break;

            default:
                Debug.Log("Unknown attack type: " + attackType);
                break;
        }
    }
    public IEnumerator StartCombat()
    {
        yield return new WaitForSeconds(3f);
        InvokeRepeating("Phase1Behavior", 0f, 30f);
    }
    private void Phase1Behavior()
    {
        int randomInt = UnityEngine.Random.Range(0, 2);

        if (!minionsAlive && randomInt == 0)
        {
            animator.SetTrigger("Summon");
        }
        else
        {
            animator.SetTrigger("Divebomb");
        }
    }


    private void SummonMinions()
    {
        Debug.Log("Lilith summons Minions!");

        for (int i = 0; i < 3; i++)
        {
            // Calculate a random relative position around the boss
            Vector3 relativePosition = new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
            Vector3 spawnPosition = transform.position + relativePosition;

            // Instantiate the idle point and minion as children of arenaGround
            Transform idlePoint = Instantiate(minionIdlePointPrefab, spawnPosition, Quaternion.identity, arenaGround.transform);
            GameObject minion = Instantiate(minionPrefab, spawnPosition, Quaternion.identity, arenaGround.transform);
            minion.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            // Assign the idle point to the minion
            MinionsChasingPlayer minionScript = minion.GetComponent<MinionsChasingPlayer>();
            if (minionScript != null)
            {
                minionScript.idlePoint = idlePoint;
            }
        }

        minionsAlive = true;
    }

    private void SpawnHealingPotions()
    {
        Debug.Log("Lilith spawns healing potions!");

        for (int i = 0; i < 10; i++)
        {
            // Calculate a random position within the arena
            Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(-50, 50), 1.5f, UnityEngine.Random.Range(-50, 50));
            Vector3 spawnPosition = arenaGround.transform.position + randomPosition;

            // Instantiate the healing potion as a child of arenaGround
            Instantiate(healingPotionPrefab, spawnPosition, Quaternion.identity, arenaGround.transform);
        }
    }


    private void DiveBombAttack()
    {
        Debug.Log("Lilith performs Divebomb!");
        if (Vector3.Distance(transform.position, Player.transform.position) < 10f)
        {
            Player.GetComponent<WandererMainManagement>().DealDamage(diveBombDamage);
        }
    }
    private IEnumerator TransitionToNextPhase(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger("Resurrected");
        Debug.Log("Lilith transitions to Phase 2!");
        canRotate = true;

    }
    private void Resurrection()
    {
        // Reset health and update phase
        currentHealth = maxHealth;
        currentPhase = 2;

        // Activate shield
        shieldActive = true;
        if (shieldPrefab != null)
        {
            // Instantiate the shield as a child of the boss
            Vector3 shieldPosition = transform.position; // Get current position
            shieldPosition.y += 5f; // Increase Y position by +5

            activeShield = Instantiate(shieldPrefab, shieldPosition, Quaternion.identity, transform);
            Debug.Log("Shield instantiated as a child of the boss.");

            // Locate the Slider component within the instantiated shield
            Slider shieldSlider = activeShield.GetComponentInChildren<Slider>();
            if (shieldSlider != null)
            {
                shieldHealthSlider = shieldSlider;
                Debug.Log("Shield slider dynamically assigned.");
            }
            else
            {
                Debug.LogWarning("No slider found in the shield prefab.");
            }
        }
        InvokeRepeating("Phase2Behavior", 0f, 30f);
    }
    private void Phase2Behavior()
    {
        int randomInt = UnityEngine.Random.Range(0, 2);

        if (randomInt == 0 && !reflectiveAuraActive)
        {
            animator.SetTrigger("Aura");
        }
        else if (!reflectiveAuraActive)
        {
            animator.SetTrigger("Spikes");
        }
    }
    private void Aura()
    {
        // Instantiate the Aura
        reflectiveAuraActive = true;
        activeAura = Instantiate(reflectiveAuraPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity, transform);
        Debug.Log("Aura instantiated!");
    }

    private IEnumerator Spikes()
    {
        // Find the spikes GameObject as a child of the boss
        Transform spikesTransform = transform.Find("Spikes");
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
            Player.GetComponent<WandererMainManagement>().DealDamage(reflectiveAuraDamage + damage);
            reflectiveAuraActive = false;
            Destroy(activeAura);
        }
        else
        {
            if (!minionsAlive)
            {
                if (!shieldActive)
                {
                    TriggerDamageAnimation(); // Updated call to trigger the animation
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
                        TriggerDamageAnimation(); // Updated call to trigger the animation
                        StartCoroutine(RegenerateShield());
                    }
                    else
                    {
                        shieldHealth -= damage;
                        if (shieldHealth == 0)
                        {
                            shieldActive = false;
                            Destroy(activeShield);
                            StartCoroutine(RegenerateShield());

                        }
                    }
                }
            }

            if (currentPhase == 1 && currentHealth <= 0)
            {
                animator.SetTrigger("Dead");
                CancelInvoke("Phase1Behavior");
                canRotate = false;
                StartCoroutine(TransitionToNextPhase(5f));
            }
            if (currentPhase == 2 && currentHealth <= 0)
            {
                Die();
            }
        }
    }
    private IEnumerator RegenerateShield()
    {
        yield return new WaitForSeconds(10f); // Wait for 10 seconds before regenerating the shield

        if (!shieldActive && currentHealth > 0) // Ensure the shield is still inactive and boss is alive
        {
            // Regenerate the shield
            shieldHealth = 50;
            shieldActive = true;

            // Instantiate the shield prefab
            if (shieldPrefab != null)
            {
                Vector3 shieldPosition = transform.position;
                shieldPosition.y += 5f; // Position the shield slightly above the boss
                activeShield = Instantiate(shieldPrefab, shieldPosition, Quaternion.identity, transform);

                Debug.Log("Shield regenerated with full health!");
            }
        }
    }


    // New method to handle damage animation trigger
    private void TriggerDamageAnimation()
    {
        animator.SetTrigger("Damaged");
        StartCoroutine(ResetTriggerWithDelay("Damaged", 2f));
    }

    // Coroutine to reset the trigger after a delay
    private IEnumerator ResetTriggerWithDelay(string triggerName, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.ResetTrigger(triggerName);
        Debug.Log($"Trigger '{triggerName}' has been reset after {delay} seconds.");
    }

    public void Die()
    {
        animator.SetTrigger("Dead");
        canRotate = false;
        Done = true;
        //Game ends and studio credits roll
        //Destroy(gameObject);
    }
    public void Stun()
    {
        if (isStunned)
        {
            // If already stunned, reset the duration by restarting the coroutine
            if (stunCoroutine != null)
            {
                StopCoroutine(stunCoroutine);
            }
        }

        stunCoroutine = StartCoroutine(HandleStun(5f));
    }
    private IEnumerator HandleStun(float duration)
    {
        isStunned = true;

        // Disable actions like movement, attacking, rotation, and abilities
        DisableActions();

        Debug.Log("Boss is stunned!");

        // Play stun animation if available
        animator.SetTrigger("Stunned");


        // Wait for the stun duration
        yield return new WaitForSeconds(duration);

        // Re-enable actions
        EnableActions();

        Debug.Log("Boss is no longer stunned!");

        isStunned = false;
        stunCoroutine = null;
    }

    private void DisableActions()
    {
        // Disable movement and attacks
        CancelInvoke("Phase1Behavior");
        CancelInvoke("Phase2Behavior");

        // Disable rotation
        canRotate = false;

        // Prevent active animations and abilities
        //animator.speed = 0; // Pause the animator
    }

    private void EnableActions()
    {
        // Resume behavior
        if (currentPhase == 1)
        {
            InvokeRepeating("Phase1Behavior", 0f, 30f);
        }
        else if (currentPhase == 2)
        {
            InvokeRepeating("Phase2Behavior", 0f, 30f);
        }

        // Enable rotation
        canRotate = true;

        // Resume animations
        //animator.speed = 1;
    }

    public void SlowDown()
    {
        if (isSlowed)
        {
            // If already slowed, reset the duration by restarting the coroutine
            if (slowCoroutine != null)
            {
                StopCoroutine(slowCoroutine);
            }
        }

        slowCoroutine = StartCoroutine(HandleSlowDown(3f)); // Slow for 3 seconds
    }

    private IEnumerator HandleSlowDown(float duration)
    {
        isSlowed = true;

        // Apply slowed effects
        ApplySlowDown();

        Debug.Log("Boss is slowed!");

        // Wait for the slow duration
        yield return new WaitForSeconds(duration);

        // Remove the slow effect
        RemoveSlowDown();

        Debug.Log("Boss is no longer slowed!");

        isSlowed = false;
        slowCoroutine = null;
    }

    private void ApplySlowDown()
    {
        // Reduce rotation speed
        originalSpeed = 5f; // Ensure original speed is properly set
        slowedSpeed = originalSpeed * 0.25f;

        // Reduce animation speed
        animator.speed *= 0.25f; // Slow down animation
    }

    private void RemoveSlowDown()
    {
        // Restore rotation speed
        slowedSpeed = originalSpeed;

        // Restore animation speed
        animator.speed = 1f;
    }

}
