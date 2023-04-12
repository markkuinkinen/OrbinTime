using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    public BossPlayer player;
    public BossController boss;
    public NPCandQuestHandler questHandler;
    public LocationHandler locHandler;

    public GameObject deathText;
    public GameObject victoryText;

    public BoxCollider2D playerCollider;
    public BoxCollider2D bossCollider;

    void Start()
    {
        questHandler = FindObjectOfType<NPCandQuestHandler>();
        locHandler = FindObjectOfType<LocationHandler>();
    }

    void Update()
    {
        if (locHandler.inBossFight)
        {
            if (!player.isAlive)
            {
                deathText.SetActive(true);
                bossCollider.enabled = false;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    deathText.SetActive(false);
                    bossCollider.enabled = true;
                    playerCollider.enabled = true;
                    boss.resetBoss();
                    player.resetPlayer();
                }
            }
            else if (!boss.isAlive)
            {
                victoryText.SetActive(true);
                playerCollider.enabled = false;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    victoryText.SetActive(false);
                    locHandler.exitBossFight();
                    Debug.Log("boss fight exited");
                }
            }
        }
    }
}
