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
    // private CapsuleCollider bombCollider;

   void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        managementScript = GetComponent<DemonsMainManagement>();
        player = GameObject.FindGameObjectWithTag("Player");
        swordCollider = GetComponentInChildren<BoxCollider>();
        // bombCollider = GetComponentInChildren<CapsuleCollider>();
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

     void OnTriggerEnter(Collider other)
{
    if (swordCollider.enabled && other.CompareTag("Player"))
    {
        Debug.Log("Player hit By Demon");
    }
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
