using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject gameplayPanel;
    public GameObject levelSelectionPanel;
    public GameObject selectCharacterPanel;

    public ImageHoverEffect[] hoverEffects;

    void Start()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (gameplayPanel != null) gameplayPanel.SetActive(false);
        if (levelSelectionPanel != null) levelSelectionPanel.SetActive(false);
    }

    public void OnPlayButtonPressed()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (gameplayPanel != null) gameplayPanel.SetActive(true);
    }

    public void OnExitButtonPressed()
    {
        Debug.Log("Exiting the game...");
        Application.Quit();
    }

    public void OnNewGameButtonPressed()
    {
        gameplayPanel.SetActive(false);
        selectCharacterPanel.SetActive(true);
    }

    public void OnSelectLevelButtonPressed()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (levelSelectionPanel != null) levelSelectionPanel.SetActive(true);
    }

    public void OnBackToMainMenuPressed()
    {
        if (gameplayPanel != null) gameplayPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);

        // Reset hover states for all buttons
        foreach (var hoverEffect in hoverEffects)
        {
            hoverEffect.ResetHoverState();
        }
    }

    public void OnNormalLevelButtonPressed()
    {
        SceneManager.LoadScene(2);
    }

    public void OnBossLevelButtonPressed()
    {
        SceneManager.LoadScene(3);
    }


}