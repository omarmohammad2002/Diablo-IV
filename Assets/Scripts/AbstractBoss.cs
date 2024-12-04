using System.Collections;
using UnityEngine;

public abstract class AbstractBoss : MonoBehaviour
{
    public string bossName;
    public int maxHealth;
    public int currentHealth;
    public int xpReward;
    public float moveSpeed;
    
    public enum BossState { Idle, Attacking, InCombat, Defeated }
    public BossState currentState;

    public virtual void Start()
    {
        currentHealth = maxHealth;
        currentState = BossState.Idle;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        if (currentHealth == 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // Handle XP reward and death logic (e.g., animations)
        WandererMainManagement.WandererMM.addXP(xpReward);
        Destroy(gameObject);
    }

    // Abstract methods to be implemented by specific bosses.
    public abstract void StartCombat();
    public abstract void TransitionToNextPhase();
}
