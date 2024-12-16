using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RougeAbilities : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowPosition;
    [SerializeField] float arrowSpeed = 10f;
    Rigidbody rb;
    [SerializeField] GameObject showerOfArrow;
    [SerializeField] GameObject smokeBombPrefab;
    [SerializeField] float smokeBombRadius = 10f;
    [SerializeField] float stunDuration = 5f;


    public Camera camera;
    private NavMeshAgent agent;
    private Animator animator;
    private WandererMainManagement mainManagement; // Reference to the main management script

    // Ability states
    private bool isBasicActive = false;
    private bool isWildcardActive = false;
    private bool isUltimateActive = false;

    bool isDefensiveAbility = false;
    // Dashing state
    private bool isDashing = false;
    private Vector3 targetPosition;
    private bool isLocked = false;

    // Cooldown timers
    public float basicCooldown = 1f;
    public float defensiveCooldown = 10f;
    public float wildcardCooldown = 5f;
    public float ultimateCooldown = 10f;
    // for cooldown
    private Dictionary<string, float> lastUsedTime = new Dictionary<string, float>();
    
    private AudioSource AudioSource; 
    public AudioClip chargeSound;

    public AudioClip arrowSound;

    [SerializeField] private Slider defensiveCooldownSlider;
    [SerializeField] private Slider wildcardCooldownSlider;
    [SerializeField] private Slider ultimateCooldownSlider;

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
                if (chargeSound != null)
                {
                    AudioSource.PlayOneShot(chargeSound);
                }
                else
                {
                    Debug.LogError("chargeSound AudioClip is not assigned!");
                }
                break;

            case "Arrow":
                if (arrowSound != null)
                {
                    AudioSource.PlayOneShot(arrowSound);
                }
                else
                {
                    Debug.LogError("arrowSound AudioClip is not assigned!");
                }
                break;
            default:
                Debug.LogWarning("Sound name not recognized: " + soundName);
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
                if (rotateToAttack())
                {
                    animator.SetTrigger("Basic");
                    isBasicActive = true;
                    lastUsedTime["Basic"] = currentTime;
                    BasicAbility();
                }
                
            }
            else          
            {
                Debug.Log("Basic ability is on cooldown.");
            }
        }
        else if (isDashing && Input.GetMouseButtonDown(1))
        {
           SetDashTarget();
        }

        if (Input.GetKeyDown(KeyCode.W) && mainManagement.getAbility1Unlock()) // Defensive Ability
        {
            if (currentTime >= lastUsedTime["Defensive"] + defensiveCooldown)
            {
                DefensiveAbility();
                lastUsedTime["Defensive"] = currentTime;
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
                isDashing = true;
                isWildcardActive = true;
                lastUsedTime["Wildcard"] = currentTime;

            } else
            {
                Debug.Log("Wildcard ability is on cooldown.");
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && !isDashing && mainManagement.getAbility3Unlock()) // Activate Ultimate Targeting
        {
            if (currentTime >= lastUsedTime["Ultimate"] + ultimateCooldown)
            {
                Debug.Log("ultimate");
                isUltimateActive = true;
            }
            else
            {
                Debug.Log("Ultimate ability is on cooldown.");
            }
        }
        if (Input.GetMouseButtonDown(1) && isUltimateActive)
        {
            lastUsedTime["Ultimate"] = currentTime;
            UltimateAbility();
        }
    
        if (isDashing && isLocked)
        {
            DashTowardsTarget();
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
    if (!isBasicActive) return;

    // Start the coroutine for handling the arrow
    StartCoroutine(ThrowArrowWithDelay());
}
    IEnumerator ThrowArrowWithDelay()
    {
        yield return new WaitForSeconds(0.9f); // Delay before activating the arrow

        // Find the GameObject with tag "ARROWMO" in the current object's hierarchy
        GameObject arrow = FindChildWithTag(transform, "ARROWMO");
        if (arrow != null)
        {
            Transform parentTransform = arrow.transform.parent;
            GameObject clonedArrow = Instantiate(arrow, arrow.transform.position, arrow.transform.rotation, parentTransform);
            clonedArrow.SetActive(false);   
            // Activate the arrow GameObject
            arrow.SetActive(true);

            // Fire the arrow
            FireArrow(arrow);

            Debug.Log("Arrow activated and fired.");
        }
        else
        {
            Debug.LogError("No GameObject with tag 'ARROWMO' found in the hierarchy!");
        }

        isBasicActive = false; // Reset state
    }

    void FireArrow(GameObject arrow)
    {
        // Detach the arrow from the parent (if any)
        arrow.transform.SetParent(null);

        // Add force to fire the arrow
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * 40, ForceMode.Impulse); // Adjust force as needed
        }
        else
        {
            Debug.LogError("Arrow GameObject does not have a Rigidbody component!");
        }

        Debug.Log("Arrow fired!");
    }

    GameObject FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }

            // Recursively search in the child's children
            GameObject result = FindChildWithTag(child, tag);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }


    private Transform FindArrowPosition()
{
    // Find the hand bone by its tag
    GameObject handBoneObject = GameObject.FindWithTag("arrowloc");

    if (handBoneObject == null)
    {
        Debug.LogError("Hand bone with tag 'RightHand' not found!");
        return null;
    }

    return handBoneObject.transform;
}



    bool rotateToAttack()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject targetHit = hit.transform.gameObject;
            if (targetHit.CompareTag("Minion") || targetHit.CompareTag("Demon") || targetHit.CompareTag("Boss"))
            {
                Vector3 targetPosition = hit.point;
                Vector3 direction = targetPosition - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 400 * Time.deltaTime);
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }
    private void DefensiveAbility()
    {

        GameObject smokeBomb = Instantiate(smokeBombPrefab, transform.position, Quaternion.identity);
        Destroy(smokeBomb, 3f);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, smokeBombRadius);
        foreach (Collider hitCollider in hitColliders)
        {

            Debug.Log("Enemy hit by Smoke Bomb: " + hitCollider.name);
            if (hitCollider.CompareTag("Boss"))
            {
                BossMainManagement bossMainManagement = hitCollider.GetComponent<BossMainManagement>();
                if (bossMainManagement != null)
                {
                    bossMainManagement.Stun();
                }
            }

            if (hitCollider.CompareTag("Minion"))
            {
                Debug.Log("Minion hit by Smoke Bomb: " + hitCollider.name);
                MinionsMainManagement minionScript = hitCollider.GetComponent<MinionsMainManagement>();
                if (minionScript != null)
                {
                    minionScript.StopMinion();
                }
            }

            if (hitCollider.CompareTag("Demon"))
            {
                DemonsMainManagement demonScript = hitCollider.GetComponent<DemonsMainManagement>();
                if (demonScript != null)
                {
                    demonScript.StopDemon();
                }
            }

        }
    }
    //Ultimate Ability, getting target position
    void SetDashTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            targetPosition = hit.point;
            animator.SetBool("isDashing", true);
            isLocked = true;
        }
    }
    //Ultimate Ability, dashing towards target and killing enemies
    void DashTowardsTarget()
    {
        // Disable the NavMeshAgent while charging
        // if (agent.enabled)
        //     agent.enabled = false;

        // Store the player's original Y position
        float originalY = transform.position.y;

        // Calculate the direction to the target
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0; // Ensure the player stays upright

        // Rotate the player to face the target direction
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 30 * Time.deltaTime);
        }

        // Move towards the target position while maintaining the original Y position
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, 10 * Time.deltaTime);
        newPosition.y = originalY; // Lock the Y-axis
        transform.position = newPosition;
        

        // Stop charging if reached the target
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                     new Vector3(targetPosition.x, 0, targetPosition.z)) < 0.8f)
        {
            StopDashing();
        }
    }
    //Ultimate ability ending
    void StopDashing()
    {
        isDashing = false;
        isLocked = false;
        isWildcardActive = false;
        animator.SetBool("isDashing", false);

        // Re-enable the NavMeshAgent after charging
        if (!agent.enabled)
            agent.enabled = true;


        // Optional: Reset the NavMeshAgent destination to the current position
        agent.SetDestination(transform.position);
    }
    
   void UltimateAbility()
{
    Debug.Log("ultimate ability");

    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit rayhit;

    if (Physics.Raycast(ray, out rayhit))
    {
        Vector3 direction = (rayhit.point - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);

        GameObject targetHit = rayhit.transform.gameObject;
        Vector3 hitPos = rayhit.point;

        if (targetHit != null)
        {
            // Elevate the spawn position by 20 units
            Vector3 spawnPos = hitPos + Vector3.up * 40;

            // Spawn the arrow shower at the elevated position
            GameObject spawn = Instantiate(showerOfArrow, spawnPos, Quaternion.identity);

            // Make the arrow move toward the hit position
            StartCoroutine(MoveArrowToTarget(spawn, hitPos));

            // Destroy the arrow shower after 5 seconds
            Destroy(spawn, 5);
        }
    }

    isUltimateActive = false;
}

IEnumerator MoveArrowToTarget(GameObject arrowShower, Vector3 targetPosition)
{
    float timeCounter = 0f;

    while (timeCounter < 1.0)
    {
        timeCounter += Time.deltaTime;
        arrowShower.transform.position = Vector3.Lerp(arrowShower.transform.position, targetPosition, timeCounter);
        yield return null;
    }

}

    
}