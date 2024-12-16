using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererAbilityUnlock : MonoBehaviour
{
    private WandererMainManagement mainManagement; // Reference to the main management script
    // might add UI element here if needed
    // Start is called before the first frame update
    private GameObject ability1Cover; // Ability 1 UI cover
    private GameObject ability2Cover; // Ability 2 UI cover
    private GameObject ability3Cover; // Ability 3 UI cover


    void Start()
    {
        mainManagement = GetComponent<WandererMainManagement>();

        // Find GameObjects by tag
        ability1Cover = GameObject.FindWithTag("Ability 1 cover");
        ability2Cover = GameObject.FindWithTag("Ability 2 cover");
        ability3Cover = GameObject.FindWithTag("Ability 3 cover");
    }

    // Update is called once per frame
    void Update()
    {
        InitialAbilityUnlock();
    }

 void InitialAbilityUnlock()
    {
        if (mainManagement.getAbilityPoints() > 0)
        {

            if (ability1Cover != null)
        {
            ability1Cover.SetActive(false); // Disable the cover for Ability 1
        }
        else
        {
            Debug.LogWarning("Ability 1 cover not found! Make sure the GameObject is tagged correctly.");
        }

        if (ability2Cover != null)
        {
            ability2Cover.SetActive(false); // Disable the cover for Ability 2
        }
        else
        {
            Debug.LogWarning("Ability 2 cover not found! Make sure the GameObject is tagged correctly.");
        }

        if (ability3Cover != null)
        {
            ability3Cover.SetActive(false); // Disable the cover for Ability 3
        }
        else
        {
            Debug.LogWarning("Ability 3 cover not found! Make sure the GameObject is tagged correctly.");
        }
    
        }
    }

    public void Ability1unlock()
    {
        if (mainManagement.getAbilityPoints() > 0)
        {
            if(!mainManagement.getAbility2Unlock())
            {
                ability2Cover.SetActive(true);
            }

             if(!mainManagement.getAbility3Unlock())
            {
                ability3Cover.SetActive(true);
            }

            Debug.Log("Ability1unlocked");
            mainManagement.unlockAbility1();
            mainManagement.useabilityPoints();
            Debug.Log("used ability poinyts");
        }
    }

    public void Ability2unlock()
    {
        if (mainManagement.getAbilityPoints() > 0)
        {
            Debug.Log("Ability2unlocked");
            mainManagement.unlockAbility2(); 
            mainManagement.useabilityPoints();

            ability2Cover.SetActive(false);

                if(!mainManagement.getAbility1Unlock())
            {
                ability1Cover.SetActive(true);
            }

             if(!mainManagement.getAbility3Unlock())
            {
                ability3Cover.SetActive(true);
            }

        }
        
    }
    public void Ability3unlock()
    {
        if (mainManagement.getAbilityPoints() > 0)
        {
            Debug.Log("Ability3unlocked");
            mainManagement.unlockAbility3();
            mainManagement.useabilityPoints();

             ability3Cover.SetActive(false);

            if(!mainManagement.getAbility1Unlock())
            {
                ability1Cover.SetActive(true);
            }

             if(!mainManagement.getAbility2Unlock())
            {
                ability2Cover.SetActive(true);
            }

    }
    }


}
