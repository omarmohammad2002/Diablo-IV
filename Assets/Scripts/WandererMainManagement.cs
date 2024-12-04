using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererMainManagement : MonoBehaviour
{ 
    // Wanderer's Health
    private int currentHealth;
    private int maxHealth ;
    // Wanderer Character class
    private string characterName;
    // Wanderer's Level and XP
    private int currentLevel;
    private int XP;
    // Wanderer's Inventory
    private int healingPotions;
    private int abilityPoints;
    private int runeFragments;
    // Cheats and Gameplay Modifiers
    private bool isInvincible = false;
    private bool isSlowMotion = false;

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 1;
        maxHealth = 100 * currentLevel; 
        currentHealth = maxHealth;
        abilityPoints = 0;
        healingPotions = 0;
        runeFragments = 0;
        XP = 0;
    }
    void Update()
    {
        HandleCheatInputs();
    }
    void HandleCheatInputs()
    {
        // Heal: Increases the Wanderer�s health by 20 health points by pressing �H�
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(20);
        }

        // Decrement Health: Decreases the Wanderer�s health by 20 health points by pressing �D�
        if (Input.GetKeyDown(KeyCode.D))
        {
            DealDamage(20);
        }
        // Toggle Invincibility: Prevents the Wanderer from taking damage by pressing �I�
        if (Input.GetKeyDown(KeyCode.I))
        {
            isInvincible = !isInvincible;
        }

        // Toggle Slow Motion: Makes the gameplay in half speed by pressing �M�
        if (Input.GetKeyDown(KeyCode.M))
        {
            isSlowMotion = !isSlowMotion;
            Time.timeScale = isSlowMotion ? 0.5f : 1f;
        }

        // Toggle Cool Down: Sets the cool down time for all the abilities to 0 by pressing �C�
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Add your ability cooldown logic here and set cooldown to 0, probably in the ability script
        }

        // Unlock Abilities: Unlocks all locked abilities by pressing �U�
        if (Input.GetKeyDown(KeyCode.U))
        {
            // Add logic here to unlock all abilities, probably in the ability script
        }

        // Gain Ability Points: Increments the ability points by 1 point by pressing �A�
        if (Input.GetKeyDown(KeyCode.A))
        {
            addabilityPoints();
        }

        // Gain XP: Increments the XP by 100 points by pressing �X�
        if (Input.GetKeyDown(KeyCode.X))
        {
            addXP(100);
        }
    }
    void DealDamage (int amount)
    {
        // This function deals damage to the player by a specific amount, to be used in enemy attack logic scrip
       if(!isInvincible)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Max(currentHealth, 0);
            //GameOver logic to be added in game over script
        }
    }
    void Heal(int amount)
    {
        // This function just heals the player by a specific amount, to be used in health potions logic script
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }
    void addHealingPotion ()
    {
        healingPotions++;
    }
    void useHealingPotion ()
    {
        // This just deducts the healing potion from the inventory, healing to be added in health potions logic script
        healingPotions--;
      
    }
    void addRuneFragment()
    {
        // This function just adds a rune fragment to the player's inventory, to be used in rune fragments logic script
        runeFragments++;
    }
    void useRuneFragment()
    {
        // This just deducts the rune fragment from the inventory,  to be used in rune fragments logic script
        runeFragments--;
    }
    void addabilityPoints()
    {
        // This function just adds an ability point to the player's inventory, to be used in ability points logic script
        abilityPoints++;
    }
    void useabilityPoints()
    {
        // This just deducts the ability point from the inventory,  to be used in ability points logic script
        abilityPoints--;
    }
    void increaseLevel()
    {
        // This function just increases the level variable of the player, level up logic to be handled in character leveling up script
        currentLevel++;
    }
    void updatecurrentHealth(int amount)
    {
        //This function just updates the current health of the player to a specific amount, to be used in character leveling up script
        currentHealth = amount;
    }
    void updateMaxHealth(int amount)
    {
        //This function just updates the max health of the player to a specific amount, to be used in character leveling up script
        maxHealth = amount;
    }

    public void addXP(int amount)
    {
        // This function just adds XP to the player's XP variable, to be used in The Wanderer gaining XP points script
        XP += amount;
    }

    void updateXP (int amount)
    {
        // This function just updates the XP of the player to a specific amount, to be used in character leveling up script
        XP = amount;
    }
}

