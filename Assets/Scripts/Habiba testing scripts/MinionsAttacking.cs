using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsAttacking : MonoBehaviour
{
    private Animator enemyAnimator;
    private MinionsMainManagement managementScript;
    private readonly float attackRange = 2f;
    private GameObject player;

    private readonly float attackCooldown = 1.5f; // Time between attacks
    private float attackTimer = 0f;

    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        managementScript = GetComponent<MinionsMainManagement>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (managementScript.currentState == MinionsMainManagement.MinionState.Aggressive)
        {
            transform.LookAt(player.transform); // Face the player

            // Reduce the attack timer if it's greater than zero
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
