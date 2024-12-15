using UnityEngine;
using Cinemachine;

public class SceneSetUp : MonoBehaviour
{
    public GameObject sorcererPrefab;
    public GameObject barbarianPrefab;
    public GameObject roguePrefab;
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
                Debug.Log("Loading player state for boss level");
                GameManager.Instance.LoadPlayerState(player);
            }
        }
    }
}

}
