using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class SceneManagerScript : MonoBehaviour
{
    // References to the panels
    public GameObject mainMenuPanel;
    public GameObject gameplayPanel;
    public GameObject levelSelectionPanel;

    void Start()
    {
        // Ensure only the main menu panel is active at the start
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (gameplayPanel != null) gameplayPanel.SetActive(false);
        if (levelSelectionPanel != null) levelSelectionPanel.SetActive(false);
    }

    // Method to switch to the gameplay panel
    public void OnPlayButtonPressed()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (gameplayPanel != null) gameplayPanel.SetActive(true);
    }

    // Method to start a new game (load the first scene)
    public void OnNewGameButtonPressed()
    {
        SceneManager.LoadScene(0); // Assumes the first scene is at index 0
    }

    // Method to open the level selection panel
    public void OnSelectLevelButtonPressed()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (levelSelectionPanel != null) levelSelectionPanel.SetActive(true);
    }

    // Method to return to the main menu panel
    public void OnBackToMainMenuPressed()
    {
        if (levelSelectionPanel != null) levelSelectionPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    // Method to load the Normal Level (Scene 2)
    public void OnNormalLevelButtonPressed()
    {
        SceneManager.LoadScene(2); // Assumes Scene 2 is the Normal Level
    }

    // Method to load the Boss Level (Scene 3)
    public void OnBossLevelButtonPressed()
    {
        SceneManager.LoadScene(3); // Assumes Scene 3 is the Boss Level
    }

    // Method to quit the application
    public void OnExitButtonPressed()
    {
        Debug.Log("Exiting the game..."); // Debug message for testing in the editor
        Application.Quit();
    }
}