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

    private bool isBasicActive = false;
    private bool isWildcardActive = false;
    private bool isUltimateActive = false;
    private bool isCharging = false;

    // Cooldown timers
    private float basicCooldown = 1f;
    private float defensiveCooldown = 10f;
    private float wildcardCooldown = 5f;
    private float ultimateCooldown = 10f;

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
        else if (isUltimateActive && Input.GetMouseButtonDown(1)) // Ultimate Ability
        {
            animator.SetBool("isCharging", true);
            Debug.Log(animator.GetBool("isCharging"));
            isCharging = true;
            lastUsedTime["Ultimate"] = currentTime;
            UltimateAbility();
          
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

        if (Input.GetKeyDown(KeyCode.E) && mainManagement.getAbility3Unlock()) // Activate Ultimate Targeting
        {
            if (currentTime >= lastUsedTime["Ultimate"] + ultimateCooldown)
            {
                Debug.Log("Ultimate ability activated. Select a target location.");
                isUltimateActive = true;
            }
        }

        if (isCharging && agent.remainingDistance < agent.stoppingDistance + 1f) // Stop Charging Animation
        {

            animator.SetBool("isCharging", false);
            Debug.Log(animator.GetBool("isCharging"));  
            isCharging = false;
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
        // Add defensive ability logic here (e.g., temporary invincibility or damage reduction)
        if (mainManagement != null)
        {
            StartCoroutine(ActivateShieldForDuration(3f)); // Shield lasts for 3 seconds
        }
    }
    private IEnumerator ActivateShieldForDuration(float duration)
    {
        Debug.Log("Shield activated. Barbarian is invincible.");

        // Set isInvincible to true
        mainManagement.setisInvincible(true);
        Debug.Log("mainManagement.isInvincible: " + mainManagement.getisInvincible());
        // Wait for the shield's duration
        yield return new WaitForSeconds(duration);

        // Reset isInvincible to false
        mainManagement.setisInvincible(false);
        Debug.Log("mainManagement.isInvincible: " + mainManagement.getisInvincible());
        Debug.Log("Shield deactivated. Barbarian is no longer invincible.");
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

    private void UltimateAbility()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            agent.SetDestination(hit.point);
            Vector3 targetPosition = hit.point;
            Vector3 startPosition = transform.position;
            Debug.Log("Ultimate ability triggered, charging towards: " + targetPosition);
            StartCoroutine(DamageEnemiesAlongPath(startPosition, targetPosition, 1f));
            isUltimateActive = false;
        }
    }

    IEnumerator DamageEnemiesAlongPath(Vector3 startPosition, Vector3 targetPosition, float radius)
    {
        float stepDistance = 1f;
        Vector3 direction = (targetPosition - startPosition).normalized;
        float distance = Vector3.Distance(startPosition, targetPosition);

        for (float step = 0; step <= distance; step += stepDistance)
        {
            // Calculate the current position along the path
            Vector3 currentPosition = startPosition + direction * step;

            // Check for enemies within the radius at the current position
            Collider[] hitColliders = Physics.OverlapSphere(currentPosition, radius);

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    MinionsDemonsMainManagement enemyScript = hitCollider.GetComponent<MinionsDemonsMainManagement>();

                    if (enemyScript != null)
                    {
                        // Wait until the player is close to the enemy
                        Debug.Log("player position" + transform.position);
                        Debug.Log("enemy position" + hitCollider.transform.position);
                        while (Vector3.Distance(transform.position, hitCollider.transform.position) > radius)
                        {
                            yield return null; // Wait for the next frame
                        }

                        // Destroy the enemy after reaching it
                        Destroy(hitCollider.gameObject);
                        Debug.Log("Enemy destroyed after player reached: " + hitCollider.name);
                    }
                }
            }
        }
    }
}