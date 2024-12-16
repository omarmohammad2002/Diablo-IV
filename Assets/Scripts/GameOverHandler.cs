using UnityEngine;
using System.Collections;

public class GameOverHandler : MonoBehaviour
{
    public GameObject gameOverPanel;
    private GameObject player;
    private WandererMainManagement mainManagement;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player != null)
        {
            mainManagement = player.GetComponent<WandererMainManagement>();
            if (mainManagement != null)
            {
                if (mainManagement.getCurrentHealth() <= 0)
                {
                    StartCoroutine(ShowGameOverWithDelay(5f));
                }
            }
        }
    }

    private IEnumerator ShowGameOverWithDelay(float delay)
    {
        // Wait for the specified time
        yield return new WaitForSeconds(delay);

        // Show the Game Over panel
        gameOverPanel.SetActive(true);
    }
}