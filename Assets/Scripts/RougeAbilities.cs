using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RougeAbilities : MonoBehaviour
{

    [SerializeField] GameObject fireball;
    [SerializeField] Transform fireballPosition;
    [SerializeField] float fireballSpeed = 10f;
    Rigidbody rb;
    [SerializeField] GameObject inferno;

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
        else if (isDashing && Input.GetMouseButtonDown(1)) // Ultimate Ability
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
    void BasicAbility()
    {
        StartCoroutine(ThrowFireballWithDelay());
    }

    IEnumerator ThrowFireballWithDelay()
    {
        // Wait for 3 seconds before throwing the fireball
        yield return new WaitForSeconds(3f);

        // Perform raycast to determine where the fireball should go
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayhit;

        if (Physics.Raycast(ray, out rayhit))
        {
            // Calculate the direction to the point the ray hit
            Vector3 direction = (rayhit.point - transform.position).normalized;
            direction.y = 0.2f; // Adjust this value for more or less upward angle
            
            // Rotate the character to face the target direction
            // transform.rotation = Quaternion.LookRotation(direction); 

            // Instantiate the fireball at the specified position and rotation
            GameObject spawn = Instantiate(fireball, fireballPosition.position, fireballPosition.rotation);
            
            // Calculate the target position for the fireball to move towards
            Vector3 hitPos = rayhit.point;
            Vector3 targetPos = (hitPos - transform.position).normalized;

            // Adjust the fireball's trajectory by adding an upward component
            targetPos.y = 0.2f; // This ensures the fireball has an upward trajectory as well.

            // Set the fireball velocity
            Rigidbody rb = spawn.GetComponent<Rigidbody>();
            rb.velocity = targetPos * fireballSpeed;

            // Destroy the fireball after 2 seconds
            Destroy(spawn, 2f);
        }

        // Deactivate ability so it cannot be triggered again immediately
        isBasicActive = false;
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
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 20 * Time.deltaTime); // Doubled the rotation speed from 10 to 20
        }

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 10 * Time.deltaTime); // Doubled the move speed from 5 to 10

        // Stop dashing if reached the target
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
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
                hitPos = hitPos + (Vector3.up * inferno.transform.localScale.y / 2) + (Vector3.right * 15);
                GameObject spawn = Instantiate(inferno, hitPos, Quaternion.identity);

                Destroy(spawn, 5);
            }
        }

        isUltimateActive = false;
    }
}
