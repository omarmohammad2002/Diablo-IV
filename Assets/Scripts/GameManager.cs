using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string SelectedCharacterName;
    public bool IsBossLevel;

    // Store player state
    public int SavedHealth=0;
    public int SavedMaxHealth;
    public int SavedCurrentLevel;
    public int SavedMaxLevel;
    public int SavedXP;
    public int SavedMaxXP;
    public int SavedHealingPotions;
    public int SavedAbilityPoints;
    public int SavedRuneFragments;
    public bool Ability1Unlocked;
    public bool Ability2Unlocked;
    public bool Ability3Unlocked;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerState(WandererMainManagement player)
    {
        SavedHealth = player.getCurrentHealth();
        SavedMaxHealth = player.getMaxHealth();
        SavedCurrentLevel= player.getCurrentLevel();
        SavedMaxLevel = player.maxLevel;
        SavedXP = player.getXP();
        SavedMaxXP = player.getMaxXP();
        SavedHealingPotions = player.getHealingPotions();
        SavedAbilityPoints = player.getAbilityPoints();
        SavedRuneFragments = player.getRuneFragments();
        Ability1Unlocked = player.getAbility1Unlock();
        Ability2Unlocked = player.getAbility2Unlock();
        Ability3Unlocked = player.getAbility3Unlock();
    }

    public void LoadPlayerState(WandererMainManagement player)
    {
        player.updatecurrentHealth(SavedHealth);
        player.updateMaxHealth(SavedMaxHealth);
        player.healingPotions = SavedHealingPotions;
        // player.addabilityPoints(); // Update based on SavedAbilityPoints
        player.currentLevel = SavedCurrentLevel;
        player.maxLevel = SavedMaxLevel;
        player.XP = SavedXP;
        player.maxXP = SavedMaxXP;
        player.abilityPoints = SavedAbilityPoints;
        player.runeFragments = SavedRuneFragments;
        if (Ability1Unlocked) player.unlockAbility1();
        if (Ability2Unlocked) player.unlockAbility2();
        if (Ability3Unlocked) player.unlockAbility3();
    }
}
