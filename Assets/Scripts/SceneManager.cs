using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject gameplayPanel;
    public GameObject optionsPanel;
    public GameObject audioPanel;
    public GameObject developersPanel;
    public GameObject creditsPanel;
    public GameObject selectLevelPanel;
    public GameObject selectCharacterPanel;
    public GameObject selectBarbarianPanel;
    public GameObject selectSorcererPanel;
    public GameObject selectErikaPanel;

    private static bool comingFromLevelSelection = false;

    public ImageHoverEffect[] hoverEffects;

    void Start()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (gameplayPanel != null) gameplayPanel.SetActive(false);
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
        comingFromLevelSelection = false;
        gameplayPanel.SetActive(false);
        selectCharacterPanel.SetActive(true);
    }

    public void OnSelectLevelButtonPressed()
    {
        comingFromLevelSelection = true;
        if (gameplayPanel != null) gameplayPanel.SetActive(false);
        if (selectLevelPanel != null) selectLevelPanel.SetActive(true);
    }

    public void OnLevelButtonPressed()
    {
        if (selectLevelPanel != null) selectLevelPanel.SetActive(false);
        if (selectCharacterPanel != null) selectCharacterPanel.SetActive(true);
    }

    public void OnBackToChooseLevelOrGamePressed()
    {
        if (selectLevelPanel != null) selectLevelPanel.SetActive(false);
        if (gameplayPanel != null) gameplayPanel.SetActive(true);
    }

    public void OnAudioButtonPressed()
    {
        optionsPanel.SetActive(false);
        audioPanel.SetActive(true);
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
        if (selectLevelPanel != null) selectLevelPanel.SetActive(false);
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
        if (audioPanel != null) audioPanel.SetActive(false);
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
        if (selectSorcererPanel != null) selectSorcererPanel.SetActive(false);
        if (selectErikaPanel != null) selectErikaPanel.SetActive(false);
        if (selectCharacterPanel != null) selectCharacterPanel.SetActive(true);

        // Reset hover states for all buttons
        foreach (var hoverEffect in hoverEffects)
        {
            hoverEffect.ResetHoverState();
        }
    }

    public void OnBackToChooseLevelPressed()
    {
        if (comingFromLevelSelection)
        {
            if (selectCharacterPanel != null) selectCharacterPanel.SetActive(false);
            if (selectLevelPanel != null) selectLevelPanel.SetActive(true);
        }
        else
        {
            if (selectCharacterPanel != null) selectCharacterPanel.SetActive(false);
            if (gameplayPanel != null) gameplayPanel.SetActive(true);
        }

        foreach (var hoverEffect in hoverEffects)
        {
            hoverEffect.ResetHoverState();
        }
    }

    public void onBarbarianClicked()
    {
        if (selectCharacterPanel != null) selectCharacterPanel.SetActive(false);
        if (selectBarbarianPanel != null) selectBarbarianPanel.SetActive(true);
    }

    public void onSorcererClicked()
    {
        if (selectCharacterPanel != null) selectCharacterPanel.SetActive(false);
        if (selectSorcererPanel != null) selectSorcererPanel.SetActive(true);
    }

    public void onErikaClicked()
    {
        if (selectCharacterPanel != null) selectCharacterPanel.SetActive(false);
        if (selectErikaPanel != null) selectErikaPanel.SetActive(true);
    }
}