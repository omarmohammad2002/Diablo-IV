using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotions : MonoBehaviour
{
    // Reference to the player's main management script
    private WandererMainManagement mainManagement;

    void Start()
    {
        // Get the WandererMainManagement component from the player
        mainManagement = GetComponent<WandererMainManagement>();

        if (mainManagement == null)
        {
            Debug.LogError("WandererMainManagement script not found on the player!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the "HealthPotion" tag
        if (other.CompareTag("HealthPotion"))
        {
            Debug.Log("Health Potion collected!");  
            // Check if the player has room for another potion
            if (mainManagement.getHealingPotions() < 3)
            {
                Debug.Log("Health Potion collected!");

                // Destroy the health potion object
                Destroy(other.gameObject);

                // Add a health potion to the player's inventory
                mainManagement.addHealingPotion();
                Debug.Log("Health potion added to inventory. Total potions: " + mainManagement.getHealingPotions());
            }
            else
            {
                Debug.Log("Potion inventory full! Cannot collect more health potions.");
            }
        }
    }
}
