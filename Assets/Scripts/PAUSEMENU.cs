using UnityEngine;
using UnityEngine.SceneManagement;  // For loading scenes (like the Main Menu or Restart)
using UnityEngine.UI;

public class PAUSEMENU : MonoBehaviour
{
    public GameObject pauseMenuUI;  // The pause menu panel (UI element)
    private bool isPaused = false;  // Flag to check if the game is paused

    void Update()
    {
        // Check if the Escape key is pressed to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();  // Resume the game
            }
            else
            {
                Pause();  // Pause the game
            }
        }
    }

    // Function to resume the game
    public void Resume()
    {
        pauseMenuUI.SetActive(false);  // Hide the pause menu
        Time.timeScale = 1f;  // Resume the game time
        isPaused = false;  // Update the pause state
    }

    // Function to pause the game
    public void Pause()
    {
        pauseMenuUI.SetActive(true);  // Show the pause menu
        Time.timeScale = 0f;  // Pause the game time
        isPaused = true;  // Update the pause state
    }

    // Function to restart the current level
    public void RestartLevel()
    {
        Time.timeScale = 1f;  // Ensure the game time is running normally
        Scene currentScene = SceneManager.GetActiveScene();  // Get the current scene
        SceneManager.LoadScene(currentScene.name);  // Reload the current scene
    }

    // Function to quit the game and load the main menu
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;  // Ensure the game time is running normally
        SceneManager.LoadScene("mainmenu");  // Replace with your actual Main Menu scene name
        Debug.Log(1);
    }
}

