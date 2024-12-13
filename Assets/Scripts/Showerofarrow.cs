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

    private void Start()
    {
        ApplyEffects();
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
            if (hitCollider.CompareTag("Minion"))
            {
                MinionsMainManagement minionScript = hitCollider.GetComponent<MinionsMainManagement>();
                if (minionScript != null)
                {
                    minionScript.TakeDamage(damageAmount);
                    StartCoroutine(ApplySlowEffect(minionScript));
                }
            }

            if (hitCollider.CompareTag("Demon"))
            {
                DemonsMainManagement demonScript = hitCollider.GetComponent<DemonsMainManagement>();
                if (demonScript != null)
                {
                    demonScript.TakeDamage(damageAmount);
                    StartCoroutine(ApplySlowEffect(demonScript));
                }
            }

            if (hitCollider.CompareTag("Boss"))
            {
                BossMainManagement bossScript = hitCollider.GetComponent<BossMainManagement>();
                if (bossScript != null)
                {
                    bossScript.TakeDamage(damageAmount);
                    StartCoroutine(ApplySlowEffect(bossScript));
                }
            }
        }
    }

    private IEnumerator ApplySlowEffect(MonoBehaviour target)
    {
        // if (target is MinionsMainManagement minion)
        // {
        //     minion.ModifySpeed(slowMultiplier);
        //     yield return new WaitForSeconds(slowEffectDuration);
        //     minion.ModifySpeed(1f / slowMultiplier); // Restore original speed
        // }
        // else if (target is DemonsMainManagement demon)
        // {
        //     demon.ModifySpeed(slowMultiplier);
        //     yield return new WaitForSeconds(slowEffectDuration);
        //     demon.ModifySpeed(1f / slowMultiplier);
        // }
        // else if (target is BossMainManagement boss)
        // {
        //     boss.ModifySpeed(slowMultiplier);
        //     yield return new WaitForSeconds(slowEffectDuration);
        //     boss.ModifySpeed(1f / slowMultiplier);
        // }
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the square region in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center.position, new Vector3(regionLength * 2, 1f, regionLength * 2));
    }
}
