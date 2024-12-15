using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public void OnBossLevelButtonPressed()
    {
        Debug.Log("Boss Level Button Pressed");
        GameManager.Instance.IsBossLevel = true;
    }

    public void OnSorcererSelected()
    {
        GameManager.Instance.SelectedCharacterName = "Sorcerer";
        LoadLevel();
    }

    public void OnBarbarianSelected()
    {
        GameManager.Instance.SelectedCharacterName = "Barbarian";
        LoadLevel();
    }

    public void OnRogueSelected()
    {
        GameManager.Instance.SelectedCharacterName = "Rogue";
        LoadLevel();
    }

    public void LoadLevel()
    {
        string sceneName = GameManager.Instance.IsBossLevel ? "BossLevel" : "NormalLevel";
        SceneManager.LoadScene(sceneName);
    }
}
