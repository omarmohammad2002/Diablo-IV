using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsAttacking : MonoBehaviour
{
    private Animator enemyAnimator;
    private MinionsMainManagement managementScript;
    private readonly float attackRange = 4f;
    private GameObject player;
    private readonly float attackCooldown = 3f; // Time between attacks
    private float attackTimer = 0f;
    private BoxCollider punchCollider;

    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        managementScript = GetComponent<MinionsMainManagement>();
        player = GameObject.FindGameObjectWithTag("Player");
        BoxCollider[] colliders = GetComponentsInChildren<BoxCollider>();

        foreach (BoxCollider collider in colliders)
        {
            // Check if the collider's GameObject is NOT the parent
            if (collider.gameObject != this.gameObject)
            {
                punchCollider = collider;
                break; // Exit loop after finding the first match
            }
        }
    }

    void Update()
    {
        AttackingAnimation();
        
    }

//    void OnTriggerEnter(Collider other)
//{
//    if (punchCollider.enabled && other.CompareTag("Player"))
//    {
//            player.GetComponent<WandererMainManagement>().DealDamage(managementScript.attackPower);
//            Debug.Log("Player hit By Minion");
//    }
//}

    void EnablePunchCollider()
    {
        punchCollider.enabled = true;
    }

    void DisablePunchCollider()
    {
        punchCollider.enabled = false;
    }

    void AttackingAnimation(){
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (managementScript.currentState == MinionsMainManagement.MinionState.Aggressive)
        {
            transform.LookAt(player.transform); // Face the player

            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }

            if (distanceToPlayer <= attackRange)
            {
                enemyAnimator.SetLayerWeight(1, 1);
                enemyAnimator.SetLayerWeight(0, 0);
                if (attackTimer <= 0)
                {
                    enemyAnimator.SetBool("isPunching", true); // Trigger punch animation
                    attackTimer = attackCooldown; // Reset the cooldown
                }
                else
                {
                    enemyAnimator.SetBool("isPunching", false); // Stay in attack stance
                }
                }
            else
            {
                enemyAnimator.SetLayerWeight(0, 1);
                enemyAnimator.SetLayerWeight(1, 0);
            }
            
        }
    }

}
