using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pGameManager : MonoBehaviour
{
    public NPCandQuestHandler questHandler;
    public LocationHandler locHandler;
    public pacmantest pacman;
    public Transform crystals;

    public ghost ghostLeft;
    public ghost ghostRight;

    public ghostChase ghostChase;
    public GameObject deathText;
    public GameObject winText;
    public GameObject[] hearts;
    public GameObject respawnText;

    public bool gameOver;
    public bool gameFinished;

    public int crystalsCollected;
    public int scoreNeeded = 142;     //142
    public int lives = 2;

    void Start()
    {
        locHandler = FindObjectOfType<LocationHandler>();
        questHandler = FindObjectOfType<NPCandQuestHandler>();
        ghostChase = FindObjectOfType<ghostChase>();
        pacman = FindObjectOfType<pacmantest>();
        gameOver = false;
        gameFinished = false;
    }

    void Update()
    {
        if (!gameOver)     
        {
            lifeUpdater();
            winCondition(); 
            if (!pacman.isAlive) 
            {
                if (lives > 0)
                {
                    respawnText.SetActive(true);
                }
                else
                {
                    deathText.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    resetGame();
                }
            }
        }
        else
        {
            winText.SetActive(true);
            questHandler.orbCollected("black");

            if (Input.GetKeyDown(KeyCode.E))
            {
                winText.SetActive(false);
                locHandler.exitPacman();
                Debug.Log("should be down pgamemanager 69");
            }
        }  
    }

    void lifeUpdater()
    {
        if (lives == 2)
        {
            hearts[0].SetActive(true);
            hearts[1].SetActive(true);
        }
        else if (lives == 1)
        {
            hearts[1].SetActive(false);
        }
        else if (lives == 0)
        {
            hearts[0].SetActive(false);
        }
    }

    void winCondition()
    {
        if (pacman.score >= scoreNeeded)
        {
            gameOver = true;
        }
    }

    public void resetGame()
    {
        pacman.GetComponent<Transform>().position = pacman.initialPosition;
        pacman.isAlive = true;
        pacman.hasMoved = false;
        pacman.direction = new Vector2(0f, 0f);
        pacman.ChangeAnimationState("Idle_down");
        ghostLeft.resetPosition();
        ghostRight.resetPosition();
        ghostChase.resetPosition();
        ghostChase.ChangeAnimationState("ghost_down");

        if (lives > 0)
        {
            respawnText.SetActive(false);
        }
        else
        {
            deathText.SetActive(false);
            respawnCrystals();
            pacman.score = 0;
            lives = 2;
        }
    }

    void respawnCrystals()
    {
        foreach (Transform crystal in crystals)
        {
            crystal.gameObject.SetActive(true);
        }
    }
}
