using UnityEngine;

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
                    gameOverPanel.SetActive(true);
                }
            }
        }
    }
}