using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BarbarianAbilities : MonoBehaviour
{
    public Camera camera;
    private NavMeshAgent agent;
    private Animator animator;
    private WandererMainManagement mainManagement; // Reference to the main management script

    // Ability states
    private bool isBasicActive = false;
    private bool isWildcardActive = false;
    private bool isUltimateActive = false;

    // Charging state
    private bool isCharging = false;
    private Vector3 targetPosition;
    private bool isLocked = false;

    // Cooldown timers
    private float basicCooldown = 1f;
    private float defensiveCooldown = 10f;
    private float wildcardCooldown = 5f;
    private float ultimateCooldown = 10f;
    // for cooldown
    private Dictionary<string, float> lastUsedTime = new Dictionary<string, float>();

    // Shield prefab
    public GameObject shieldPrefab; // Reference to the shield prefab
    private GameObject activeShield; // To keep track of the instantiated shield

    private AudioSource AudioSource; 
    public AudioClip chargeSound;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        mainManagement = GetComponent<WandererMainManagement>();

        // Initialize the last-used times
        lastUsedTime["Basic"] = -basicCooldown;
        lastUsedTime["Defensive"] = -defensiveCooldown;
        lastUsedTime["Wildcard"] = -wildcardCooldown;
        lastUsedTime["Ultimate"] = -ultimateCooldown;
        AudioSource = GetComponent<AudioSource>();
    }
    public void PlaySound(string soundName)
    {
        switch (soundName)
        {
            case "Charge":
                AudioSource.PlayOneShot(chargeSound);
                break;
            default:
                Debug.LogWarning("Sound not found: " + soundName);
                break;
        }
    }

    void Update()
    {
        float currentTime = Time.time;

        if (!isUltimateActive && !isWildcardActive && Input.GetMouseButtonDown(1)) // Basic Ability
        {
            if (currentTime >= lastUsedTime["Basic"] + basicCooldown)
            {
                animator.SetTrigger("Basic");
                isBasicActive = true;
                lastUsedTime["Basic"] = currentTime;
                BasicAbility();
            }
            else
            {
                Debug.Log("Basic ability is on cooldown.");
            }
        }
        else if (isCharging && Input.GetMouseButtonDown(1)) // Ultimate Ability
        {
           SetChargeTarget();
        }

        if (Input.GetKeyDown(KeyCode.W) && mainManagement.getAbility1Unlock()) // Defensive Ability
        {
            if (currentTime >= lastUsedTime["Defensive"] + defensiveCooldown)
            {
                animator.SetTrigger("Defensive");
                lastUsedTime["Defensive"] = currentTime;
                DefensiveAbility();
            }
            else
            {
                Debug.Log("Defensive ability is on cooldown.");
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && mainManagement.getAbility2Unlock() && !isBasicActive && !isUltimateActive) // Wildcard Ability
        {
            if (currentTime >= lastUsedTime["Wildcard"] + wildcardCooldown)
            {
                animator.SetTrigger("Wildcard");
                isWildcardActive = true;
                lastUsedTime["Wildcard"] = currentTime;
                WildcardAbility();
            }
            else
            {
                Debug.Log("Wildcard ability is on cooldown.");
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && !isCharging && mainManagement.getAbility3Unlock()) // Activate Ultimate Targeting
        {
            if (currentTime >= lastUsedTime["Ultimate"] + ultimateCooldown)
            {
                isCharging = true;
                isUltimateActive = true;
               
            }
            else
            {
                Debug.Log("Ultimate ability is on cooldown.");
            }
        }
        if (isCharging && isLocked)
        {
            lastUsedTime["Ultimate"] = currentTime;
            ChargeTowardsTarget();
        }


    }
    
    void BasicAbility()
    {
        rotateToAttack();
        StartCoroutine(AttackWithDelay(0.5f));
    }

    void rotateToAttack()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.point;
            Vector3 direction = targetPosition - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 400 * Time.deltaTime);
        }
    }

    IEnumerator AttackWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        bash();
    }

    void bash()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Boss"))
            {
                BossMainManagement enemyScript = hitCollider.GetComponent<BossMainManagement>();
                if (enemyScript != null)
                {
                    Vector3 directionToEnemy = (hitCollider.transform.position - transform.position).normalized;
                    float dotProduct = Vector3.Dot(transform.forward, directionToEnemy);
                    if (dotProduct > 0.5f)
                    {
                        Debug.Log("Enemy detected in front: " + hitCollider.name);
                        enemyScript.TakeDamage(5);
                        break;
                    }
                }
            }
            if (hitCollider.CompareTag("Minion"))
            {
                MinionsMainManagement enemyScript = hitCollider.GetComponent<MinionsMainManagement>();
                if (enemyScript != null)
                {
                    Vector3 directionToEnemy = (hitCollider.transform.position - transform.position).normalized;
                    float dotProduct = Vector3.Dot(transform.forward, directionToEnemy);
                    if (dotProduct > 0.5f)
                    {
                        Debug.Log("Enemy detected in front: " + hitCollider.name);
                        enemyScript.TakeDamage(5);
                        break;
                    }
                }
            }
            if (hitCollider.CompareTag("Demon"))
            {
                DemonsMainManagement enemyScript = hitCollider.GetComponent<DemonsMainManagement>();
                if (enemyScript != null)
                {
                    Vector3 directionToEnemy = (hitCollider.transform.position - transform.position).normalized;
                    float dotProduct = Vector3.Dot(transform.forward, directionToEnemy);
                    if (dotProduct > 0.5f)
                    {
                        Debug.Log("Enemy detected in front: " + hitCollider.name);
                        enemyScript.TakeDamage(5);
                        break;
                    }
                }
            }
        }
        isBasicActive = false;
    }

    private void DefensiveAbility()
    {
        Debug.Log("Shield activated. Defensive ability triggered.");

        // Check if the main management script is available
        if (mainManagement != null)
        {
            // Start the coroutine to activate the shield
            StartCoroutine(ActivateShieldForDuration(3f)); // Shield lasts for 3 seconds
        }
    }

    private IEnumerator ActivateShieldForDuration(float duration)
    {
        Debug.Log("Shield activated. Barbarian is invincible.");

        // Set isInvincible to true
        mainManagement.setisInvincible(true);
        Debug.Log("mainManagement.isInvincible: " + mainManagement.getisInvincible());

        // Instantiate the shield prefab as a child of the player
        if (shieldPrefab != null)
        {
            Vector3 shieldSpawnPosition = transform.position + new Vector3(0, 1f, 0); // Adjust 1f to your desired height
            activeShield = Instantiate(shieldPrefab, shieldSpawnPosition, Quaternion.identity, transform);
            Debug.Log("Shield prefab instantiated.");
        }
        else
        {
            Debug.LogError("Shield prefab is not assigned!");
        }

        // Wait for the shield's duration
        yield return new WaitForSeconds(duration);

        // Reset isInvincible to false
        mainManagement.setisInvincible(false);
        Debug.Log("mainManagement.isInvincible: " + mainManagement.getisInvincible());
        Debug.Log("Shield deactivated. Barbarian is no longer invincible.");

        // Destroy the shield prefab
        if (activeShield != null)
        {
            Destroy(activeShield);
            Debug.Log("Shield prefab destroyed.");
        }
    }

    private void WildcardAbility()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Boss"))
            {
                BossMainManagement enemyScript = hitCollider.GetComponent<BossMainManagement>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(10);
                    Debug.Log("Enemy hit by Iron Maelstorm: " + hitCollider.name);
                }
            }
            if (hitCollider.CompareTag("Minion"))
            {
                MinionsMainManagement enemyScript = hitCollider.GetComponent<MinionsMainManagement>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(10);
                    Debug.Log("Enemy hit by Iron Maelstorm: " + hitCollider.name);
                }
            }
            if (hitCollider.CompareTag("Demon"))
            {
                DemonsMainManagement enemyScript = hitCollider.GetComponent<DemonsMainManagement>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(10);
                    Debug.Log("Enemy hit by Iron Maelstorm: " + hitCollider.name);
                }
            }
        }
        isWildcardActive = false;
    }
    //Ultimate Ability, getting target position
    void SetChargeTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            targetPosition = hit.point;
            animator.SetBool("isCharging", true);
            isLocked = true;
        }
    }
    //Ultimate Ability, charging towards target and killing enemies
    void ChargeTowardsTarget()
    {
        // Disable the NavMeshAgent while charging
        if (agent.enabled)
            agent.enabled = false;

        // Calculate the direction to the target
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0; // Ensure the player stays upright

        // Rotate the player to face the target direction
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
        }

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 5 * Time.deltaTime);

        // Check for collisions with enemies
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider hitCollider in hitEnemies)
        {
            if (hitCollider.CompareTag("Boss"))
            {
                DemonsMainManagement enemyScript = hitCollider.GetComponent<DemonsMainManagement>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(20);

                }
            }
            if (hitCollider.CompareTag("Minion"))
            {
                MinionsMainManagement enemyScript = hitCollider.GetComponent<MinionsMainManagement>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(enemyScript.currentHealth);

                }
            }
            if (hitCollider.CompareTag("Demon"))
            {
                DemonsMainManagement enemyScript = hitCollider.GetComponent<DemonsMainManagement>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(enemyScript.currentHealth);
                    
                }
            }
        }

        // Stop charging if reached the target
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            StopCharging();
        }
    }

    void StopCharging()
    {
        isCharging = false;
        isLocked = false;
        isUltimateActive = false;
        animator.SetBool("isCharging", false);

        // Re-enable the NavMeshAgent after charging
        if (!agent.enabled)
            agent.enabled = true;

        // Optional: Reset the NavMeshAgent destination to the current position
        agent.SetDestination(transform.position);
    }

    // Method to get the remaining cooldown time for Basic Ability
    public float GetBasicCooldownRemaining() 
    {
        float remainingTime = Mathf.Max(0, (lastUsedTime["Basic"] + basicCooldown) - Time.time);
        return remainingTime;
    }

    // Method to get the remaining cooldown time for Defensive Ability
    public float GetDefensiveCooldownRemaining() // ability 1
    {
        float remainingTime = Mathf.Max(0, (lastUsedTime["Defensive"] + defensiveCooldown) - Time.time);
        return remainingTime;
    }

    // Method to get the remaining cooldown time for Wildcard Ability
    public float GetWildcardCooldownRemaining() // ability 2
    {
        float remainingTime = Mathf.Max(0, (lastUsedTime["Wildcard"] + wildcardCooldown) - Time.time);
        return remainingTime;
    }

    // Method to get the remaining cooldown time for Ultimate Ability
    public float GetUltimateCooldownRemaining() //ability 3
    {
        float remainingTime = Mathf.Max(0, (lastUsedTime["Ultimate"] + ultimateCooldown) - Time.time);
        return remainingTime;
    }


}
