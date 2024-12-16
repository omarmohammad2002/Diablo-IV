using UnityEngine;
using Cinemachine;

public class SceneSetUp : MonoBehaviour
{
    public GameObject sorcererPrefab;
    public GameObject barbarianPrefab;
    public GameObject roguePrefab;
    public GameObject sorcererHUDPrefab;
    public GameObject barbarianHUDPrefab;
    public GameObject rogueHUDPrefab;
    public Transform spawnPoint; // Set this in the Inspector

    private void Awake()
    {
        SpawnCharacter();
    }

private void SpawnCharacter()
{
    string characterName = GameManager.Instance.SelectedCharacterName;
    GameObject characterPrefab = null;

    switch (characterName)
    {
        case "Sorcerer":
            characterPrefab = sorcererPrefab;
            break;
        case "Barbarian":
            characterPrefab = barbarianPrefab;
            break;
        case "Rogue":
            characterPrefab = roguePrefab;
            break;
    }

    if (characterPrefab != null)
    {
        GameObject character = Instantiate(characterPrefab, spawnPoint.position, spawnPoint.rotation);
        // switch (characterName)
        // {
        //     case "Sorcerer":
        //         GameObject sorcererHUD = Instantiate(sorcererHUDPrefab, Vector3.zero, Quaternion.identity);
        //         break;
        //     case "Barbarian":
        //         GameObject barbarianHUD = Instantiate(barbarianHUDPrefab, Vector3.zero, Quaternion.identity);
        //         break;
        //     case "Rogue":
        //         GameObject rogueHUD = Instantiate(rogueHUDPrefab, Vector3.zero, Quaternion.identity);
        //         break;
        // }

        // Configure Cinemachine camera
        CinemachineVirtualCamera vCam = FindObjectOfType<CinemachineVirtualCamera>();
        vCam.Follow = character.transform;
        vCam.LookAt = character.transform;

        WandererMainManagement player = character.GetComponent<WandererMainManagement>();

        if (GameManager.Instance.IsBossLevel)
        {

            if (GameManager.Instance.SavedMaxHealth == 0) // Check if no previous state
            {
                    // Fresh start for the boss level
                player.updateMaxHealth(400);
                player.updatecurrentHealth(400);
                player.unlockAbility1();
                player.unlockAbility2();
                player.unlockAbility3();
                player.healingPotions = 0;
                // player.addabilityPoints(); // Reset to 0
                player.runeFragments = 0;
            }
            else
            {
                // Load saved state
                GameManager.Instance.LoadPlayerState(player);
            }
        }
    }
}

}
