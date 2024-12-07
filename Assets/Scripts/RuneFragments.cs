using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneFragments : MonoBehaviour
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
        if (other.CompareTag("RuneFragment"))
        {

                Debug.Log("Rune Fragment collected!");

                // Destroy the health potion object
                Destroy(other.gameObject);

                // Add a health potion to the player's inventory
                mainManagement.addRuneFragment();
                Debug.Log("Rune Fragment added to inventory. Total fragments: " + mainManagement.getRuneFragments());

        }
    }
}
