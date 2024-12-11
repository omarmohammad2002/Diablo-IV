using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonsMainManagement : MonoBehaviour
{
    
    public int maxHealth;     
    public int currentHealth; 
    public int attackPower;   
    public int xpReward;      
    public int explosivePower;
    public enum DemonState { Idle, Patrolling, Aggressive }
    public DemonState currentState;
    private Animator demonAnimator;



   void Awake()
    {
        maxHealth = 40;
        currentHealth = maxHealth;
        attackPower = 10;
        explosivePower = 15;
        xpReward = 30;
        currentState = DemonState.Patrolling;
        
    }

    void Start()
    {
        demonAnimator = GetComponent<Animator>();
    }

     public void TakeDamage(int damage)
    {
        demonAnimator.SetBool("isDamaged", true);
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        demonAnimator.SetLayerWeight(2, 0.5f);
        if (currentHealth == 0)
        {
            enemyDeath();
        }

       
    }

    

    public void enemyDeath()
    {
        // Search for the player object using the Player tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Get the WandererMainManagement component
            WandererMainManagement wandererMM = player.GetComponent<WandererMainManagement>();

            if (wandererMM != null)
            {
                // Add XP to the player
                wandererMM.addXP(xpReward);
            }
            else
            {
                Debug.LogError("WandererMainManagement component not found on the player!");
            }
        }
        else
        {
            Debug.LogError("Player object with tag 'Player' not found!");
        }
        //might add death animation before destruction

        // Play death animation and destroy the game object
        Destroy(gameObject);
    }


}
