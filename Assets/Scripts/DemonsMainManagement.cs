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


   void Awake()
    {
        maxHealth = 40;
        currentHealth = maxHealth;
        attackPower = 10;
        explosivePower = 15;
        xpReward = 30;
        currentState = DemonState.Patrolling;
        
    }

     public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        if (currentHealth == 0)
        {
            enemyDeath();
        }

       
    }

    public void enemyDeath()
    {
        // add xp to the player, play death animation and destroy the game object
        WandererMainManagement.WandererMM.addXP(xpReward);
        Destroy(gameObject);
    }

}
