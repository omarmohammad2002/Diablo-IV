using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class NormalToBossLevel : MonoBehaviour
{
    // Reference to the player's main management script
    private WandererMainManagement mainManagement;

    // Start is called before the first frame update
    void Start()
    {
        mainManagement = GetComponent<WandererMainManagement>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            if (mainManagement.getRuneFragments() >= 3)
            {
                // Consume the rune fragments
                mainManagement.useRuneFragment();
                mainManagement.useRuneFragment();
                mainManagement.useRuneFragment();
                LoadBossLevel();
               
               
            }}

    }

    void LoadBossLevel()
    {
        GameManager.Instance.SavePlayerState(FindObjectOfType<WandererMainManagement>());
        GameManager.Instance.IsBossLevel = true;
        SceneManager.LoadScene("BossLevel");
    }
}
