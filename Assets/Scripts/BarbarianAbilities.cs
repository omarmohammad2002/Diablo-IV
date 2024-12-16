 using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    private bool bashBool = false;  

    // Cooldown timers
    public float basicCooldown = 1f;
    public float defensiveCooldown = 10f;
    public float wildcardCooldown = 5f; //update
    public float ultimateCooldown = 1f; //update

    // Cooldown UI sliders
    [SerializeField] private Slider defensiveCooldownSlider;
    [SerializeField] private Slider wildcardCooldownSlider;
    [SerializeField] private Slider ultimateCooldownSlider;

    // for cooldown
    private Dictionary<string, float> lastUsedTime = new Dictionary<string, float>();

    private AudioSource AudioSource; 
    public AudioClip chargeSound;

    [SerializeField] private GameObject destructionPrefab;

    void Awake(){
        camera = Camera.main;
    }
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

        // Initialize slider values
        if (defensiveCooldownSlider != null)
    {
        defensiveCooldownSlider.maxValue = defensiveCooldown;
        defensiveCooldownSlider.value = defensiveCooldown;
        defensiveCooldownSlider.gameObject.SetActive(false); // Hide initially
    }
    if (wildcardCooldownSlider != null)
    {
        wildcardCooldownSlider.maxValue = wildcardCooldown;
        wildcardCooldownSlider.value = wildcardCooldown;
        wildcardCooldownSlider.gameObject.SetActive(false); // Hide initially
    }
    if (ultimateCooldownSlider != null)
    {
        ultimateCooldownSlider.maxValue = ultimateCooldown;
        ultimateCooldownSlider.value = ultimateCooldown;
        ultimateCooldownSlider.gameObject.SetActive(false); // Hide initially
    }
    }
    public void PlaySound(string soundName)
    {
        switch (soundName)
        {
            case "Charge":
                AudioSource.PlayOneShot(chargeSound);
                break;
            default:
                break;
        }
    }

    void Update()
    {
        
        float currentTime = Time.time;

        // Update sliders to reflect remaining cooldown times
        UpdateCooldownSliders(currentTime);

        if (!isUltimateActive && !isWildcardActive && Input.GetMouseButtonDown(1)) // Basic Ability
        {
            if (currentTime >= lastUsedTime["Basic"] + basicCooldown)
            {
                
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

     void UpdateCooldownSliders(float currentTime)
{
    if (defensiveCooldownSlider != null)
    {
        float remainingDefensiveCooldown = Mathf.Max(0, defensiveCooldown - (currentTime - lastUsedTime["Defensive"]));
        defensiveCooldownSlider.value = remainingDefensiveCooldown;
        defensiveCooldownSlider.gameObject.SetActive(remainingDefensiveCooldown > 0); // Show slider during cooldown
    }

    if (wildcardCooldownSlider != null)
    {
        float remainingWildcardCooldown = Mathf.Max(0, wildcardCooldown - (currentTime - lastUsedTime["Wildcard"]));
        wildcardCooldownSlider.value = remainingWildcardCooldown;
        wildcardCooldownSlider.gameObject.SetActive(remainingWildcardCooldown > 0); // Show slider during cooldown
    }

    if (ultimateCooldownSlider != null)
    {
        float remainingUltimateCooldown = Mathf.Max(0, ultimateCooldown - (currentTime - lastUsedTime["Ultimate"]));
        ultimateCooldownSlider.value = remainingUltimateCooldown;
        ultimateCooldownSlider.gameObject.SetActive(remainingUltimateCooldown > 0); // Show slider during cooldown
    }
}
    
    void BasicAbility()
    {
        rotateToAttack();
        if (bashBool)
        {
            animator.SetTrigger("Basic");
            StartCoroutine(AttackWithDelay(1f));
        }
    }

    void rotateToAttack()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        float bashRange = 4f; // Define the maximum range for the bash ability

        if (Physics.Raycast(ray, out hit))
        {
            GameObject targetHit = hit.transform.gameObject;
            Debug.Log("Target hit: " + targetHit.name);

            // Check if the target is a valid enemy and within range
            if ((targetHit.CompareTag("Minion") || targetHit.CompareTag("Demon") || targetHit.CompareTag("Boss")) )
            {
                bashBool = true; // Set bashBool to true only if both conditions are met
                Debug.Log("Valid enemy within range. BashBool set to true.");

                // Rotate to face the target
                Vector3 targetPosition = hit.point;
                Vector3 direction = targetPosition - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 400 * Time.deltaTime);
            }
            else
            {
                // Either not a valid enemy or out of range
                bashBool = false;
                Debug.Log("Enemy is out of range or not valid. BashBool set to false.");
                ResetAttack();
            }
        }
        else
        {
            // Raycast didn't hit anything
            bashBool = false;
            Debug.Log("Attack failed: Nothing was hit.");
            ResetAttack();
        }
    }

    // Reset the cooldown and attack parameters
    void ResetAttack()
    {
        isBasicActive = false; // Reset the attack active flag
        lastUsedTime["Basic"] = -basicCooldown; //to reset the cooldown
        Debug.Log("Attack cooldown and parameters have been reset.");
    }


    IEnumerator AttackWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        bash();
    }

    void bash()
    {
        Debug.Log("bashbool" + bashBool);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 4f);
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
        bashBool = false;
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

        // Find the shield as a child of the player
        Transform shieldTransform = transform.Find("Shield");
        if (shieldTransform != null)
        {
            // Activate the shield
            GameObject shield = shieldTransform.gameObject;
            shield.SetActive(true);
            Debug.Log("Shield activated from child object.");
        }
        else
        {
            Debug.LogError("Shield child object not found!");
        }

        // Wait for the shield's duration
        yield return new WaitForSeconds(duration);

        // Reset isInvincible to false
        mainManagement.setisInvincible(false);
        Debug.Log("mainManagement.isInvincible: " + mainManagement.getisInvincible());
        Debug.Log("Shield deactivated. Barbarian is no longer invincible.");

        // Deactivate the shield
        if (shieldTransform != null)
        {
            GameObject shield = shieldTransform.gameObject;
            shield.SetActive(false);
            Debug.Log("Shield deactivated.");
        }
    }


    private void WildcardAbility()
    {
        StartCoroutine(WildcardAbilityWithDelay(1f)); // Delay of 1 seconds
    }

    private IEnumerator WildcardAbilityWithDelay(float delay)
    {
        Debug.Log("Iron Maelstrom ability triggered. Waiting for delay...");

        // Wait for the delay
        yield return new WaitForSeconds(delay);

        // Damage logic
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 4f);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Boss"))
            {
                BossMainManagement enemyScript = hitCollider.GetComponent<BossMainManagement>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(10);
                    Debug.Log("Boss hit by Iron Maelstrom: " + hitCollider.name);
                }
            }
            if (hitCollider.CompareTag("Minion"))
            {
                MinionsMainManagement enemyScript = hitCollider.GetComponent<MinionsMainManagement>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(10);
                    Debug.Log("Minion hit by Iron Maelstrom: " + hitCollider.name);
                }
            }
            if (hitCollider.CompareTag("Demon"))
            {
                DemonsMainManagement enemyScript = hitCollider.GetComponent<DemonsMainManagement>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(10);
                    Debug.Log("Demon hit by Iron Maelstrom: " + hitCollider.name);
                }
            }
        }

        Debug.Log("Iron Maelstrom damage applied after delay.");
        isWildcardActive = false;
    }

    //Ultimate Ability, getting target position
    void SetChargeTarget()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Check if the target is within the medium range
            float distanceToTarget = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                                                       new Vector3(hit.point.x, 0, hit.point.z));

            if (distanceToTarget > 20f) // Medium range is 20 units
            {
                lastUsedTime["Ultimate"] = -ultimateCooldown; //to reset the cooldown
                StopCharging();
                Debug.Log("Target is out of medium range. Charge ability cannot be activated.");

                return; // Exit without activating charge
            }

            targetPosition = hit.point;
            animator.SetBool("isCharging", true);
            isLocked = true;
        }
    }

    //Ultimate Ability, charging towards target and killing enemies
    private HashSet<GameObject> damagedEnemies = new HashSet<GameObject>(); // Tracks damaged enemies

    void ChargeTowardsTarget()
    {
        // Disable the NavMeshAgent while charging
        if (agent.enabled)
            agent.enabled = false;

        // Store the player's original Y position
        float originalY = transform.position.y;

        // Calculate the direction to the target
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0; // Ensure the player stays upright

        // Rotate the player to face the target direction
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
        }

        // Move towards the target position while maintaining the original Y position
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, 10 * Time.deltaTime);
        newPosition.y = originalY; // Lock the Y-axis
        transform.position = newPosition;

        // Check for collisions with enemies
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider hitCollider in hitEnemies)
        {
            // Ensure the enemy hasn't been damaged already
            if (!damagedEnemies.Contains(hitCollider.gameObject))
            {
                print(hitCollider.tag);
                if (hitCollider.CompareTag("Boss"))
                {
                    BossMainManagement enemyScript = hitCollider.GetComponent<BossMainManagement>();
                    if (enemyScript != null)
                    {
                        Debug.Log("Enemy hit by Charge: " + hitCollider.name);
                        enemyScript.TakeDamage(20);
                        damagedEnemies.Add(hitCollider.gameObject); // Mark as damaged
                    }
                }

                else if (hitCollider.CompareTag("Minion"))
                {
                    MinionsMainManagement enemyScript = hitCollider.GetComponent<MinionsMainManagement>();
                    if (enemyScript != null)
                    {
                        enemyScript.TakeDamage(enemyScript.currentHealth);
                        damagedEnemies.Add(hitCollider.gameObject); // Mark as damaged
                    }
                }

                else if (hitCollider.CompareTag("Demon"))
                {
                    DemonsMainManagement enemyScript = hitCollider.GetComponent<DemonsMainManagement>();
                    if (enemyScript != null)
                    {
                        enemyScript.TakeDamage(enemyScript.currentHealth);
                        damagedEnemies.Add(hitCollider.gameObject); // Mark as damaged
                    }
                }
                else if (hitCollider.CompareTag("Untagged"))
                {
                    // Store the position of the object to be destroyed
                    Vector3 spawnPosition = hitCollider.transform.position;

                    // Ensure the Y position is set to 0
                    spawnPosition.y = 0f;

                    // Destroy the object
                    Destroy(hitCollider.gameObject);

                    // Spawn the destruction prefab at the same position with specific rotation
                    if (destructionPrefab != null)
                    {
                        Quaternion spawnRotation = Quaternion.Euler(-90f, 0f, 0f); // Set the desired rotation
                        Instantiate(destructionPrefab, spawnPosition, spawnRotation);
                    }
                }

            }
        }

        // Stop charging if reached the target
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                     new Vector3(targetPosition.x, 0, targetPosition.z)) < 0.1f)
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

        // Clear the damaged enemies list for the next charge
        damagedEnemies.Clear();

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
