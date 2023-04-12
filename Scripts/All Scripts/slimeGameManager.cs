using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeGameManager : MonoBehaviour
{
    public PlayerController player;
    public spawnerScript spawner;
    public LocationHandler locHandler;
    public NPCandQuestHandler questHandler;

    public GameObject slimeCanvas;
    public GameObject deathText;
    public GameObject winText;
    public bool gameWon;

    public int slimeWater;

    void Start()
    {
        slimeWater = 1;
    }

    void Update()
    {
        if (locHandler.inSlimeGame)
        {
            player.currentHealth = 2;
            slimeCanvas.SetActive(true);
            if (player.isAlive && slimeWater <= 30)
            {
                spawner.spawnEverything();
            }
            else if (!player.isAlive)
            {
                player.canMove = false;
                player.deathState();
                player.canTurn = false;
                deathText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                {
                    slimeWater = 1;
                    locHandler.spawn(-117.86f, 26.04f);
                    player.isAlive = true;
                    player.spawnState();
                    player.canTurn = true;
                    deathText.SetActive(false);
                }
            }
            else
            {
                gameWon = true;
                player.canMove = false;
                winText.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    slimeCanvas.SetActive(false);
                    player.canMove = true;
                    locHandler.inSlimeGame = false;
                    questHandler.slimeGameCompleted();
                    locHandler.spawn(0, 38.22f);
                    player.slimeCamera.SetActive(false);
                    player.well.SetActive(true);
                }
            }
        }
    }
}
