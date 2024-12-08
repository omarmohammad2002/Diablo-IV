using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsMainManagement : MonoBehaviour
{

    public int maxHealth;     
    public int currentHealth; 
    public int attackPower;   
    public int xpReward;      
    public enum MinionState { Idle, Aggressive }
    public MinionState currentState;

    void Awake()
    {
        maxHealth = 20;
        currentHealth = maxHealth;
        attackPower = 5;
        xpReward = 10;
        currentState = MinionState.Idle;
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        if (currentHealth == 0)
        {
            EnemyDeath();
        }

       
    }

    public void EnemyDeath()
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
        //might add death animation before destroying the game object
        // Play death animation and destroy the game object
        Destroy(gameObject);
}


}
