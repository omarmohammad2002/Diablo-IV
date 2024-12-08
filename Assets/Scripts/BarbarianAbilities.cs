using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BarbarianAbilities2 : MonoBehaviour
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                MinionsDemonsMainManagement enemyScript = hitCollider.GetComponent<MinionsDemonsMainManagement>();
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                MinionsDemonsMainManagement enemyScript = hitCollider.GetComponent<MinionsDemonsMainManagement>();
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
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Destroy(enemy.gameObject); // Destroy enemy
            }
        }

        // Stop charging if reached the target
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            StopCharging();
        }
    }
    //Ultimate ability ending
    void StopCharging()
    {
        isCharging = false;
        isLocked = false;
        isUltimateActive = false;
        animator.SetBool("isCharging", false);
        // Enable other actions here if needed
    }

}
