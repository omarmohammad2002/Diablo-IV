using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WandererMainManagement : MonoBehaviour
{
    // Wanderer's Health
    public int currentHealth;
    public int maxHealth;
    // Wanderer Character class
    public string characterName;
    // Wanderer's Level and XP
    public int currentLevel;
    public int maxLevel = 4;
    public int XP;
    public int maxXP = 100;
    // Wanderer's Inventory
    public int healingPotions;
    public int abilityPoints;
    public int runeFragments;

    [SerializeField] private Slider healthSlider; // Reference to the slider

    [SerializeField] private Slider xpSlider; // Reference to the XP slider

    // Wanderer's Health
    // Wanderer's Inventory
    // Cheats and Gameplay Modifiers
    public bool isInvincible = false;
    private bool isSlowMotion = false;

    private bool ability1Unlock = false;
    private bool ability2Unlock = false;
    private bool ability3Unlock = false;
    // Pause Game
    private bool isGamePaused = false;
    private GameObject pauseScreen;

    private bool isDead = false;

    // Enemies Following
    public int enemiesFollowing = 0;

    private Animator Animator;
    public bool animationPlayed;

    //audio
    private AudioSource AudioSource;
    public AudioClip drinkingSound;
    public AudioClip dyingSound;
    public AudioClip damagedSound;
    public AudioClip pickUpSound;


    public TextMeshProUGUI healthText;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI healingPotionsText;
    public TextMeshProUGUI abilityPointsText;
    public TextMeshProUGUI RuneFragmentsText;

    public GameObject bloodPrefab; // Prefab to instantiate


    // Start is called before the first frame update
    void Awake()
    {
        currentLevel = 1;
        maxHealth = 100 * currentLevel;
        currentHealth = maxHealth;
        abilityPoints = 0;
        healingPotions = 0;
        runeFragments = 0;
        XP = 0;
        Animator = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();
        InitializeSlider(); // Set up the slider at the start
        
    }

    private void InitializeSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        else
        {
            Debug.LogWarning("Health slider is not assigned!");
        }

        if (xpSlider != null)
        {
            xpSlider.maxValue = maxXP;
            xpSlider.value = XP;
        }
        else
        {
            Debug.LogWarning("XP slider is not assigned!");
        }
    }
    public void PlaySound(string soundName)
    {
        switch (soundName)
        {
            case "Drinking":
                AudioSource.PlayOneShot(drinkingSound);
                break;
            case "Dying":
                AudioSource.PlayOneShot(dyingSound);
                break;
            case "Damaged":
                AudioSource.PlayOneShot(damagedSound);
                break;
            case "PickUp":
                AudioSource.PlayOneShot(pickUpSound);
                break;
            default:
                break;
        }
    }
    void Update()
    {
        HandleCheatInputs();
        if (Input.GetKeyDown(KeyCode.F))
        {
            ConsumeHealingPotion();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }
        UpdateUI(); // Update UI every frame
    }

    public void UpdateUI()
    {
        if (healthText != null)
           healthText.text = $"HP:                    {currentHealth}/{maxHealth}";
        
        if (xpText != null)
            xpText.text = $"XP:                   {XP}/{maxXP}";
        
        if (levelText != null)
            levelText.text = $"{currentLevel}";
        

        if (healingPotionsText != null)
            healingPotionsText.text = $"Potions:                {healingPotions}";

        if (abilityPointsText != null)
            abilityPointsText.text = $"Ability Points:        {abilityPoints}";

        if (RuneFragmentsText != null)
            RuneFragmentsText.text = $"Fragments:           {runeFragments}";
    }
    void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Time.timeScale = 0; // Pause the game
            // Open the pause screen
            if (pauseScreen != null)
            {
                pauseScreen.SetActive(true);
                Debug.Log("Game Paused");
            }
        }
        else
        {
            Time.timeScale = 1; // Resume the game
            // Close the pause screen
            if (pauseScreen != null)
            {
                pauseScreen.SetActive(false);
                Debug.Log("Game Resumed");
            }
        }
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
            string name = GameManager.Instance.SelectedCharacterName;
            if (name.Equals("Sorcerer"))
            {
                sorcererAbilities script = GetComponent<sorcererAbilities>();
                script.basicCooldown = 0;
                script.wildcardCooldown = 0;
                script.defensiveCooldown = 0;
                script.ultimateCooldown = 0;
            }
            else if (name.Equals("Rogue"))
            {
                RougeAbilities script = GetComponent<RougeAbilities>();
                script.basicCooldown = 0;
                script.wildcardCooldown = 0;
                script.defensiveCooldown = 0;
                script.ultimateCooldown = 0;
            }
            else if (name.Equals("Barbarian"))
            {
                BarbarianAbilities script = GetComponent<BarbarianAbilities>();
                script.basicCooldown = 0;
                script.wildcardCooldown = 0;
                script.defensiveCooldown = 0;
                script.ultimateCooldown = 0;

            }
        }

        // Unlock Abilities: Unlocks all locked abilities by pressing �U�
        if (Input.GetKeyDown(KeyCode.U))
        {
            // Add logic here to unlock all abilities, probably in the ability script
            ability1Unlock = true;
            ability2Unlock = true;
            ability3Unlock = true;
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
        if (Input.GetKeyDown(KeyCode.O))
        {
            addRuneFragment();
        }
    }
    public void DealDamage(int amount)
    {
        // This function deals damage to the player by a specific amount, to be used in enemy attack logic scripts
        if (currentHealth > 0)
        {


            if (!isInvincible)
            {

                if (healthSlider != null)
                    healthSlider.value = currentHealth; // Update the slider

                currentHealth -= amount;
                TriggerDamageAnimation(); // Updated to use the new method

                // Activate blood effect
                Transform blood = transform.Find("Blood"); // Finds the child named 'blood'
                if (blood != null)
                {
                    StartCoroutine(ActivateBloodEffect(blood.gameObject, 1f)); // Activate for 2 seconds
                }

                if (currentHealth <= 0)
                {
                    // Trigger death animation and game over logic
                    Animator.SetTrigger("Dead");
                    isDead = true;
                    animationPlayed = true;
                    NavMeshAgent agent = GetComponent<NavMeshAgent>();
                    if (agent != null)
                    {
                        agent.enabled = false;
                        Debug.Log("NavMeshAgent disabled");
                    }
                }
            }
        }
    }

    // Coroutine to handle activating and deactivating the blood effect
    private IEnumerator ActivateBloodEffect(GameObject bloodObject, float duration)
    {
        bloodObject.SetActive(true); // Activate the blood effect
        yield return new WaitForSeconds(duration); // Wait for the specified duration
        Vector3 spawnPosition = transform.position + transform.forward * 2f;
        spawnPosition.y = 0; // Ensure it's on the ground (adjust if necessary)
        Instantiate(bloodPrefab, spawnPosition, Quaternion.identity);
        bloodObject.SetActive(false); // Deactivate the blood effect
    }


    // New method to handle damage animation trigger
    private void TriggerDamageAnimation()
    {
        Animator.SetTrigger("Damaged");
        StartCoroutine(ResetTriggerWithDelay("Damaged", 0.02f)); // Reset the trigger after 2 seconds
    }

    // Coroutine to reset the trigger after a delay
    private IEnumerator ResetTriggerWithDelay(string triggerName, float delay)
    {
        yield return new WaitForSeconds(delay);
        Animator.ResetTrigger(triggerName);
        Debug.Log($"Trigger '{triggerName}' has been reset after {delay} seconds.");
    }

    public void Heal(int amount)
    {
        // This function just heals the player by a specific amount, to be used in health potions logic script
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        if (healthSlider != null)
            healthSlider.value = currentHealth; // Update the slider
    }
    public void ConsumeHealingPotion()
    {
        if ((healingPotions > 0) && (currentHealth < maxHealth))
        {
            Animator.SetTrigger("Drinking");
            Debug.Log("drinking");

            Heal((int)((50f / 100f) * maxHealth)); // Heal by 50 health points (adjust as needed)
            useHealingPotion(); // Reduce potion count
            Debug.Log("Used a healing potion. Remaining potions: " + healingPotions);
        }
        else
        {
            Debug.Log("No healing potions available!");
        }
    }
    public void addHealingPotion()
    {
        healingPotions++;
        Debug.Log("Health potion added to inventory. Total potions: " + healingPotions);
        PlaySound("PickUp");
    }
    public void useHealingPotion()
    {
        // This just deducts the healing potion from the inventory, healing to be added in health potions logic script
        healingPotions--;

    }
    public void addRuneFragment()
    {
        // This function just adds a rune fragment to the player's inventory, to be used in rune fragments logic script
        runeFragments++;
        PlaySound("PickUp");
    }
    public void useRuneFragment()
    {
        // This just deducts the rune fragment from the inventory,  to be used in rune fragments logic script
        runeFragments--;
    }
    public void addabilityPoints()
    {
        // This function just adds an ability point to the player's inventory, to be used in ability points logic script
        abilityPoints++;
    }
    public void useabilityPoints()
    {
        // This just deducts the ability point from the inventory,  to be used in ability points logic script
        abilityPoints--;
    }
    public void increaseLevel()
    {
        // This function just increases the level variable of the player, level up logic to be handled in character leveling up script
        currentLevel++;
    }
    public void updatecurrentHealth(int amount)
    {
        //This function just updates the current health of the player to a specific amount, to be used in character leveling up script
        currentHealth = amount;

        if (healthSlider != null)
            healthSlider.value = currentHealth; // Update the slider
    }
    public void updateMaxHealth(int amount)
    {
        //This function just updates the max health of the player to a specific amount, to be used in character leveling up script
        maxHealth = amount;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth; // Update the slider max value
            healthSlider.value = currentHealth; // Update the slider value
        }
    }

    public void addXP(int amount)
    {
        if (currentLevel < maxLevel)
        {
            XP += amount;
            if (xpSlider != null)
                xpSlider.value = XP; // Update the XP slider

            if (XP >= maxXP)
            {

                if (XP > maxXP)
                    XP = XP - maxXP;
                else if (XP == maxXP)
                    XP = 0;

                if (xpSlider != null)
                    xpSlider.value = XP; // Reset XP slider after leveling up

                increaseLevel();
                updateMaxHealth(100 * currentLevel);
                updatecurrentHealth(maxHealth);
                updatemaxXP(100 * currentLevel);
                addabilityPoints();
            }
        }
    }
    public void updatemaxXP(int amount)
    {
        //This function just updates the max XP of the player to a specific amount, to be used in character leveling up script
        maxXP = amount;
        if (xpSlider != null)
        {
            xpSlider.maxValue = maxXP; // Update the slider max value
        }
    }


    public void unlockAbility1()
    {
        // This function just unlocks the first ability variable of the player, to be used in ability logic script
        ability1Unlock = true;
    }
    public void unlockAbility2()
    {
        // This function just unlocks the second ability variable of the player, to be used in ability logic script
        ability2Unlock = true;
    }
    public void unlockAbility3()
    {
        // This function just unlocks the third ability variable of the player, to be used in ability logic script
        ability3Unlock = true;
    }
    public int getCurrentHealth()
    {
        return currentHealth;
    }
    public int getMaxHealth()
    {
        return maxHealth;
    }
    public int getCurrentLevel()
    {
        return currentLevel;
    }
    public int getXP()
    {
        return XP;
    }
    public int getMaxXP()
    {
        return maxXP;
    }
    public int getHealingPotions()
    {
        return healingPotions;
    }
    public int getAbilityPoints()
    {
        return abilityPoints;
    }
    public int getRuneFragments()
    {
        return runeFragments;
    }
    public bool getIsInvincible()
    {
        return isInvincible;
    }
    public bool getIsSlowMotion()
    {
        return isSlowMotion;
    }
    public bool getAbility1Unlock()
    {
        return ability1Unlock;
    }
    public bool getAbility2Unlock()
    {
        return ability2Unlock;
    }

    public bool getAbility3Unlock()
    {
        return ability3Unlock;
    }
    public void setisInvincible(bool X)
    {
        isInvincible = X;
    }
    public bool getisInvincible()
    {
        return isInvincible;
    }

    public void addEnemyFollowing()
    {
        enemiesFollowing++;
    }
    public void removeEnemyFollowing()
    {
        enemiesFollowing--;
    }
    public int getEnemiesFollowing()
    {
        return enemiesFollowing;
    }
}