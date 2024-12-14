using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalLevelInstantiation : MonoBehaviour
{
    public GameObject minionPrefab;
    public GameObject demonPrefab;
    public Transform demonPatrolPointsPrefab;
    public Transform minionIdlePointPrefab;
    public GameObject healingPotionPrefab;
    public GameObject runeFragmentPrefab;
    public GameObject[] enemyCampsPrefab;

    public int totalMinions = 50;
    public int totalDemons = 10;
    public int totalHealingPotions = 25; // Adjust the total as needed

    public GameObject terrainObject; // Reference to the GameObject representing the terrain
    public float randomizationRangeScale = 0.5f; // Reduce range to 50% of the camp bounds

    void Awake()
    {
        GenerateEnemyCamps();
    }

    void Update()
    {
        foreach (GameObject enemyCamp in enemyCampsPrefab)
        {
            // Check if all minions and demons are dead
            bool allEnemiesDefeated = true;

            foreach (Transform child in enemyCamp.transform)
            {
                if (child.CompareTag("Minion") || child.CompareTag("Demon"))
                {
                    allEnemiesDefeated = false;
                    break; // No need to check further if an enemy is still alive
                }
            }

            // Activate the rune fragment if all enemies are defeated
            if (allEnemiesDefeated)
            {
                foreach (Transform child in enemyCamp.transform)
                {
                    if (child.CompareTag("RuneFragment"))
                    {
                        child.gameObject.SetActive(true);
                        break; // Activate the first rune fragment found
                    }
                }
            }
        }
    }

    void GenerateEnemyCamps()
    {
        int campsCount = enemyCampsPrefab.Length;
        int minionsPerCamp = totalMinions / campsCount;
        int demonsPerCamp = totalDemons / campsCount;
        int potionsPerCamp = totalHealingPotions / campsCount;

        for (int i = 0; i < campsCount; i++)
        {
            GameObject enemyCamp = enemyCampsPrefab[i];

            // Create enemies and potions as children of the camp
            CreateEnemies(enemyCamp.transform, minionsPerCamp, demonsPerCamp);
            CreateHealingPotions(enemyCamp.transform, potionsPerCamp);

            // Create Rune Fragment as a child of the camp
            CreateRuneFragment(enemyCamp.transform);
        }

        // Distribute any remaining minions, demons, or potions among the camps
        DistributeRemainingEntities(minionsPerCamp * campsCount, totalMinions, 
                                     demonsPerCamp * campsCount, totalDemons, 
                                     potionsPerCamp * campsCount, totalHealingPotions);
    }

    void CreateEnemies(Transform parent, int minions, int demons)
    {
        Collider campCollider = parent.GetComponent<Collider>(); // Get the camp collider
        for (int i = 0; i < minions; i++)
        {
            Vector3 spawnPosition = GetRandomPositionWithinScaledBounds(campCollider);
            spawnPosition.y = 0; // Fix Y position to 0 for minions

            // Instantiate minion
            GameObject minion = Instantiate(minionPrefab, spawnPosition, Quaternion.identity);
            minion.transform.SetParent(parent);

            // Instantiate idle point and assign to MinionsChasingPlayer script
            Transform idlePoint = Instantiate(minionIdlePointPrefab, spawnPosition, Quaternion.identity);
            MinionsChasingPlayer minionScript = minion.GetComponent<MinionsChasingPlayer>();
            if (minionScript != null)
            {
                minionScript.idlePoint = idlePoint;
            }
        }

        for (int i = 0; i < demons; i++)
        {
            Vector3 spawnPosition = GetRandomPositionWithinScaledBounds(campCollider);
            spawnPosition.y = 0; // Fix Y position to 0 for demons

            // Instantiate demon
            GameObject demon = Instantiate(demonPrefab, spawnPosition, Quaternion.identity);
            demon.transform.SetParent(parent);

            // Instantiate patrol point and assign to DemonsChasingPlayer script
            Transform patrolPoint = Instantiate(demonPatrolPointsPrefab, spawnPosition, Quaternion.identity);
            DemonsChasingPlayer demonScript = demon.GetComponent<DemonsChasingPlayer>();
            if (demonScript != null)
            {
                demonScript.patrolPoints = patrolPoint;
            }
        }
    }

    void CreateHealingPotions(Transform parent, int potionCount)
    {
        Collider campCollider = parent.GetComponent<Collider>(); // Get the camp collider
        for (int i = 0; i < potionCount; i++)
        {
            Vector3 potionPosition = GetRandomPositionWithinScaledBounds(campCollider);
            potionPosition.y = 1; // Fix Y position to 1 for healing potions
            GameObject potion = Instantiate(healingPotionPrefab, potionPosition, Quaternion.identity);
            potion.transform.SetParent(parent);
        }
    }

    void CreateRuneFragment(Transform parent)
    {
        Collider campCollider = parent.GetComponent<Collider>(); // Get the camp collider
        Vector3 fragmentPosition = GetRandomPositionWithinScaledBounds(campCollider);
        fragmentPosition.y = 1; // Fix Y position to 1 for rune fragments
        GameObject runeFragment = Instantiate(runeFragmentPrefab, fragmentPosition, Quaternion.identity);
        runeFragment.transform.SetParent(parent);
        runeFragment.SetActive(false); // Hide until enemies are defeated
    }

    void DistributeRemainingEntities(int usedMinions, int totalMinions, int usedDemons, int totalDemons, int usedPotions, int totalPotions)
    {
        int remainingMinions = totalMinions - usedMinions;
        int remainingDemons = totalDemons - usedDemons;
        int remainingPotions = totalPotions - usedPotions;

        for (int i = 0; i < enemyCampsPrefab.Length; i++)
        {
            if (remainingMinions > 0)
            {
                CreateEnemies(enemyCampsPrefab[i].transform, 1, 0);
                remainingMinions--;
            }

            if (remainingDemons > 0)
            {
                CreateEnemies(enemyCampsPrefab[i].transform, 0, 1);
                remainingDemons--;
            }

            if (remainingPotions > 0)
            {
                CreateHealingPotions(enemyCampsPrefab[i].transform, 1);
                remainingPotions--;
            }
        }
    }

    Vector3 GetRandomPositionWithinScaledBounds(Collider collider)
    {
        Bounds bounds = collider.bounds;
        float xMin = Mathf.Lerp(bounds.min.x, bounds.center.x, randomizationRangeScale);
        float xMax = Mathf.Lerp(bounds.max.x, bounds.center.x, randomizationRangeScale);
        float zMin = Mathf.Lerp(bounds.min.z, bounds.center.z, randomizationRangeScale);
        float zMax = Mathf.Lerp(bounds.max.z, bounds.center.z, randomizationRangeScale);

        float randomX = Random.Range(xMin, xMax);
        float randomZ = Random.Range(zMin, zMax);
        return new Vector3(randomX, bounds.min.y, randomZ);
    }
}
