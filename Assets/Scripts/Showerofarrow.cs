using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerofArrow : MonoBehaviour
{
    public Transform center; // The center object of the prefab
    public float regionLength = 5f; // Half the length of the square region
    public float damageInterval = 1f; // Interval for applying damage
    public int damageAmount = 10; // Initial damage amount
    public float slowEffectDuration = 3f; // Duration of the slow effect
    public float slowMultiplier = 0.25f; // Slow effect multiplier

    private float timeCounter = 0;
    private HashSet<Collider> affectedEnemies = new HashSet<Collider>(); // Track affected enemies

    private void Start()
    {
        ApplyEffects();
        print("Start");
    }

    private void Update()
    {
        timeCounter += Time.deltaTime;

        if (timeCounter >= damageInterval)
        {
            timeCounter = 0f;
            ApplyEffects();
        }
    }

    private void ApplyEffects()
    {
        // Get all colliders in the square region
        Collider[] hitColliders = Physics.OverlapBox(
            center.position,
            new Vector3(regionLength, 1f, regionLength), // Define the square region
            Quaternion.identity // No rotation for the box
        );


        foreach (Collider hitCollider in hitColliders)
        {
            // Check if the enemy has already been affected
            if (affectedEnemies.Contains(hitCollider)) continue;

            if (hitCollider.CompareTag("Minion"))
            {
                MinionsMainManagement minionScript = hitCollider.GetComponent<MinionsMainManagement>();
                if (minionScript != null)
                {
                    affectedEnemies.Add(hitCollider); // Mark as affected
                    minionScript.TakeDamage(damageAmount);
                    ApplySlowEffect(minionScript);
                }
            }

            if (hitCollider.CompareTag("Demon"))
            {
                DemonsMainManagement demonScript = hitCollider.GetComponent<DemonsMainManagement>();
                if (demonScript != null)
                {
                    affectedEnemies.Add(hitCollider); // Mark as affected
                    demonScript.TakeDamage(damageAmount);
                    ApplySlowEffect(demonScript);
                }
            }

            if (hitCollider.CompareTag("Boss"))
            {
                print("Boss");
                BossMainManagement bossScript = hitCollider.GetComponent<BossMainManagement>();
                if (bossScript != null)
                {
                    affectedEnemies.Add(hitCollider); // Mark as affected
                    bossScript.TakeDamage(damageAmount);
                    bossScript.SlowDown();  
                }
            }
        }
    }

    private void ApplySlowEffect(MonoBehaviour target)
    {
        if (target is MinionsMainManagement minion)
        {
            minion.StunMinion();
        }
        else if (target is DemonsMainManagement demon)
        {
            demon.StunDemon();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the square region in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center.position, new Vector3(regionLength * 2, 1f, regionLength * 2));
    }
}
