using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererAbilityUnlock : MonoBehaviour
{
    private WandererMainManagement mainManagement; // Reference to the main management script
    // might add UI element here if needed
    // Start is called before the first frame update
    void Start()
    {
        mainManagement = GetComponent<WandererMainManagement>();
    }

    // Update is called once per frame

    void Ability1unlock()
    {
        if (mainManagement.getAbilityPoints() > 0)
        {
            mainManagement.unlockAbility1();
            mainManagement.useabilityPoints();
        }
    }

    void Ability2unlock()
    {
        if (mainManagement.getAbilityPoints() > 0)
        {
            mainManagement.unlockAbility2(); 
            mainManagement.useabilityPoints();
        }
    }
    void Ability3unlock()
    {
        if (mainManagement.getAbilityPoints() > 0)
        {
            mainManagement.unlockAbility3();
            mainManagement.useabilityPoints();
        }
    }


}
