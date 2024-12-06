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

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        if (currentHealth == 0)
        {
            EnemyDeath();
        }

       
    }

    void EnemyDeath()
    {
        WandererMainManagement.WandererMM.addXP(xpReward);
        Destroy(gameObject);
    }

}
