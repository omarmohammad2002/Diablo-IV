using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject gameplayPanel;
    public GameObject optionsPanel;
    public GameObject developersPanel;
    public GameObject creditsPanel;
    public GameObject levelSelectionPanel;
    public GameObject selectCharacterPanel;
    public GameObject selectBarbarianPanel;

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

    public void OnOptionButtonPressed()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(true);
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

    public void OnDevelopersButtonPressed()
    {
        optionsPanel.SetActive(false);
        developersPanel.SetActive(true);
    }

    public void OnCreditsButtonPressed()
    {
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void OnBackToMainMenuPressed()
    {
        if (gameplayPanel != null) gameplayPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);

        // Reset hover states for all buttons
        foreach (var hoverEffect in hoverEffects)
        {
            hoverEffect.ResetHoverState();
        }
    }

    public void OnBackToOptionsPressed()
    {
        if (developersPanel != null) developersPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(true);

        // Reset hover states for all buttons
        foreach (var hoverEffect in hoverEffects)
        {
            hoverEffect.ResetHoverState();
        }
    }

    public void OnBackToSelectCharacterPressed()
    {
        if (selectBarbarianPanel != null) selectBarbarianPanel.SetActive(false);
        if (selectCharacterPanel != null) selectCharacterPanel.SetActive(true);

        // Reset hover states for all buttons
        foreach (var hoverEffect in hoverEffects)
        {
            hoverEffect.ResetHoverState();
        }
    }

    public void OnBackToChooseLevelPressed()
    {
        if (selectCharacterPanel != null) selectCharacterPanel.SetActive(false);
        if (gameplayPanel != null) gameplayPanel.SetActive(true);

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