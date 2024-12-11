using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DemonsAttacking : MonoBehaviour
{

    private Animator enemyAnimator;
    private DemonsMainManagement managementScript;
    private readonly float attackRange = 3f;
    private GameObject player;
    private BoxCollider swordCollider;
    private CapsuleCollider bombCollider;

   void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        managementScript = GetComponent<DemonsMainManagement>();
        player = GameObject.FindGameObjectWithTag("Player");
        bombCollider = GetComponentInChildren<CapsuleCollider>();
        if (bombCollider == null)
        {
            Debug.LogError("Bomb Collider not found");
        }
        BoxCollider[] colliders = GetComponentsInChildren<BoxCollider>();

        foreach (BoxCollider collider in colliders)
        {
            // Check if the collider's GameObject is NOT the parent
            if (collider.gameObject != this.gameObject)
            {
                swordCollider = collider;
                break; // Exit loop after finding the first match
            }
        }

    }


    // Update is called once per frame
    void Update()
    {

       AttackingAnimation();
    }

    void EnableSwordCollider()
    {
        swordCollider.enabled = true;
    }

    void DisableSwordCollider()
    {
        swordCollider.enabled = false;
    }

    void EnableBombCollider()
    {
        bombCollider.enabled = true;
    }

    void DisableBombCollider()
    {
        bombCollider.enabled = false;
    }


    void AttackingAnimation()
    {
         float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (managementScript.currentState == DemonsMainManagement.DemonState.Aggressive)
        {
            transform.LookAt(player.transform); 


            if (distanceToPlayer <= attackRange)
            {
                enemyAnimator.SetLayerWeight(1, 1);
                enemyAnimator.SetLayerWeight(0, 0);
                enemyAnimator.SetBool("isAttacking", true);
               
            }
            else
            {
                enemyAnimator.SetBool("isAttacking", false);
                enemyAnimator.SetLayerWeight(0, 1);
                enemyAnimator.SetLayerWeight(1, 0);
            }
        
    }
    }
}
