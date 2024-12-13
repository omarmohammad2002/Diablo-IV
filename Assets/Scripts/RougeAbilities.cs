using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RougeAbilities : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowPosition;
    [SerializeField] float arrowSpeed = 10f;
    Rigidbody rb;
    [SerializeField] GameObject showerOfArrow;
    [SerializeField] GameObject smokeBombPrefab;
    [SerializeField] float smokeBombRadius = 5f;
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
    private float basicCooldown = 1f;
    private float defensiveCooldown = 10f;
    private float wildcardCooldown = 5f;
    private float ultimateCooldown = 10f;
    // for cooldown
    private Dictionary<string, float> lastUsedTime = new Dictionary<string, float>();

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
                rotateToAttack();
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
        else if (isDashing && Input.GetMouseButtonDown(1))
        {
            print("SetDashTarget");
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
 void BasicAbility()
{
    if (!isBasicActive) return;

    // Start the coroutine for handling the arrow
    StartCoroutine(ThrowArrowWithDelay());
}
    //    IEnumerator ThrowArrowWithDelay()
    //{
    //        Dynamically find the arrow position
    //       Transform currentArrowPosition = FindArrowPosition();

    //        if (currentArrowPosition == null)
    //        {
    //            Debug.LogError("Arrow position not found. Aborting arrow spawn.");
    //            yield break;
    //        }

    //        yield return new WaitForSeconds(0.9f); // Delay before spawning the arrow

    //        // Cast a ray to determine the target point
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit rayhit;

    //        if (Physics.Raycast(ray, out rayhit))
    //        {
    //            Vector3 targetPoint = rayhit.point; // Point where the ray hit
    //            Vector3 direction = (targetPoint - currentArrowPosition.position).normalized; // Calculate direction

    //            // Spawn the arrow
    //            GameObject spawn = Instantiate(arrow, currentArrowPosition.position, Quaternion.LookRotation(direction));

    //            // Adjust player rotation to face the target point
    //            Vector3 playerDirection = (targetPoint - transform.position).normalized;
    //            playerDirection.y = 0; // Keep the player upright
    //            transform.rotation = Quaternion.LookRotation(playerDirection);

    //            // Apply velocity to the arrow
    //            Rigidbody rb = spawn.GetComponent<Rigidbody>();
    //            if (rb != null)
    //            {
    //                rb.velocity = direction * arrowSpeed;
    //            }

    //            // Destroy the arrow after 4 seconds
    //            Destroy(spawn, 4f);
    //        }
    //        else
    //        {
    //            Debug.LogError("No valid target found for the arrow.");
    //        }


    //        isBasicActive = false;
    //}



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
            rb.AddForce(transform.forward * 20, ForceMode.Impulse); // Adjust force as needed
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



    void rotateToAttack()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.point;
            Vector3 direction = targetPosition - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 600 * Time.deltaTime);
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
                   }

    }
    private void WildcardAbility()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Minion"))
            {
                MinionsMainManagement minionScript = hitCollider.GetComponent<MinionsMainManagement>();
                if (minionScript != null)
                {
                    minionScript.TakeDamage(10);
                    Debug.Log("Enemy hit by Iron Maelstorm: " + hitCollider.name);
                }
            }

            if (hitCollider.CompareTag("Demon"))
            {
                DemonsMainManagement demonScript = hitCollider.GetComponent<DemonsMainManagement>();
                if (demonScript != null)
                {
                    demonScript.TakeDamage(10);
                    Debug.Log("Enemy hit by Iron Maelstorm: " + hitCollider.name);
                }
            }

            if (hitCollider.CompareTag("Boss"))
            {
                BossMainManagement bossScript = hitCollider.GetComponent<BossMainManagement>();
                if (bossScript != null)
                {
                    bossScript.TakeDamage(10);
                    Debug.Log("Enemy hit by Iron Maelstorm: " + hitCollider.name);
                }
            }
        }
        isWildcardActive = false;
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
       // Calculate the direction to the target
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0; // Ensure the player stays upright

        // Rotate the player to face the target direction
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 30 * Time.deltaTime); // Doubled the rotation speed from 10 to 20
        }

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 20 * Time.deltaTime); // Doubled the move speed from 5 to 10

        // Stop dashing if reached the target
        if (Vector3.Distance(transform.position, targetPosition) < 1.2f)
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
        // Enable other actions here if needed
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
