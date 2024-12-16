using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverHandler : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;
    private GameObject player;
    private GameObject boss;
    private WandererMainManagement mainManagement;
    private BossMainManagement bossMainManagement;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boss = GameObject.FindGameObjectWithTag("Boss");
        bossMainManagement = boss.GetComponent<BossMainManagement>();
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

            if (bossMainManagement != null)
            {
                Debug.Log(bossMainManagement.Done);
                if (bossMainManagement.Done)
                {
                    StartCoroutine(Win());
                }
            }
        }
    }

    private IEnumerator Win()
    {
        yield return new WaitForSeconds(6f);
        gameWinPanel.SetActive(true);
    }

}