using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinionsDemonsMainManagement : MonoBehaviour
{
    public string enemyType;
    public int maxHealth;
    private int currentHealth = 10;
    public int attackPower;
    public int xpReward;
    public float moveSpeed;


    public enum EnemyState { Idle, Patrolling, Aggressive }
    public EnemyState currentState;

    public void Start()
    {
        currentHealth = maxHealth;

    }

    public void TakeDamage(int damage)
    {
        print(currentHealth);
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
        //WandererMainManagement.WandererMM.addXP(xpReward);
        Destroy(gameObject);
    }


}
