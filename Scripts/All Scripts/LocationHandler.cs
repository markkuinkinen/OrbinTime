using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocationHandler : MonoBehaviour
{
    public PlayerController player;
    public GameObject playerCamera;
    public Camera mainCamera;

    public Transform starterSpawn;
    public NPCandQuestHandler quest;
    public textHandler textHandler;
    public GameObject chief;
    public GameObject boss_Skin;

    //public GameObject well;
    public GameObject wellDoor;
    public GameObject pilotObject;

    // location bools to help returning location to town
    bool inFirstHouse;
    public bool inOverworld;
    public bool inFarmHouse;
    bool inChiefHouse;
    public bool inPilotHouse;
    public bool inGraveHouse;
    public bool inWaterfall;
    public bool inSnowHouse;
    bool inTownHouse;
    bool inHeroHouse;
    public bool inSnow;
    public bool inSlimeGame;
    public bool inPenguinGame;
    public bool inPacmanGame;
    public bool inCrypt;
    public bool inBossFight;
    public bool inGraveyard;
    public bool postBossFight;

    public StoneMover stones;
    public GameObject stone;

    public GameObject bossFightObjects;
    public GameObject pacmanCamera;
    public GameObject pacmanCanvas;
    public GameObject slimeCanvas;
    public GameObject questCanvas;
    public GameObject pacmanGame;
    public GameObject playerHealth;
    
    bool townSpawn;

    Vector2 currentSpawnLocation;
    Vector2 StartingSpawnLocation;
    Vector2 postWellLocation;
    Vector2 postPenguinLocation;
    Vector2 fromPenguinHouseLocation;

    void Start()
    {
        inOverworld = true;
    }

    public void enterBossFight()
    {
        bossFightObjects.SetActive(true);
        playerCamera.SetActive(false);
        questCanvas.SetActive(false);
        player.canMove = false;
        player.canTurn = false;
        stones.spawnStone(Random.Range(0, 4));
        playerHealth.SetActive(false);
        chief.SetActive(false);
        boss_Skin.SetActive(true);
    }

    public void exitBossFight()
    {
        playerCamera.SetActive(true);
        bossFightObjects.SetActive(false);
        inBossFight = false;
        postBossFight = true;
    }

    public void exitPacman()
    {
        spawn(-15.46f, 28.18766f);
        pacmanGame.SetActive(false);
        questCanvas.SetActive(true);
        playerCamera.SetActive(true);
        inPacmanGame = false;
        inCrypt = false;
        playerHealth.SetActive(true);
        player.currentHealth = 2;
        inGraveyard = false;
        inOverworld = true;
    }

    public void flyToSnow()
    {
        spawn(76.46f, -46.9f);
        pilotObject.GetComponent<Transform>().position = new Vector2(76.45f, -45.66f);
        textHandler.textBox.SetActive(false);
    }

    public void flyBack()
    {
        spawn(46.08f, 32.1f);
        pilotObject.GetComponent<Transform>().position = new Vector2(46.08f, 33.18f);
        textHandler.textBox.SetActive(false);
    }

    public void spawn(float x, float y)
    {
        player.trans.position = new Vector2(x, y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "overworld":
                inOverworld = true;
                inGraveyard = false;
                break;
            case "graveyard":
                inOverworld = false;
                inGraveyard = true;
                break;
            case "pacman":
                playerCamera.SetActive(false);
                Debug.Log("playercam should be disabled");
                pacmanCamera.SetActive(true);
                pacmanCanvas.SetActive(true);
                questCanvas.SetActive(false);
                inPacmanGame = true;
                playerHealth.SetActive(false);
                break;
            case "townHouse1":
                player.trans.position = new Vector2(32.35f, 116.31f);
                inTownHouse = true;
                break;
            case "snowHouse":
                player.trans.position = new Vector2(96.81f, -98.44f);
                inSnowHouse = true;
                break;
            case "crypt":
                spawn(190.5f, -32.7f);
                inCrypt = true;
                break;
            case "firstHouse":
                player.trans.position = new Vector2(-62.07f, 9.65f);
                inFirstHouse = true;
                break;

            case "slimeHouse":
                player.trans.position = new Vector2(-9.81f, 75.96f);
                inFarmHouse = true;
                break;

            case "slimeGame":
                inSlimeGame = true;
                player.isAlive = true;
                player.ChangeAnimationState("Idle_down");         
                spawn(-117.87f, 25.9f);
                break;

            case "chiefHouse":
                player.trans.position = new Vector2(64.57f, 71.5f);
                inChiefHouse = true;
                break;

            case "pilotHouse":
                player.trans.position = new Vector2(109.24f, 41.65f);
                inPilotHouse = true;
                break;

            case "graveHouse":
                player.trans.position = new Vector2(115.97f, 12.74f);
                inGraveHouse = true;
                break;

            case "waterFall":
                player.trans.position = new Vector2(-73.61f, 65.38f);
                inWaterfall = true;
                break;

            case "town":
                if (inFirstHouse)
                {
                    player.trans.position = new Vector2(-3.48f, 7.47f);
                    inFirstHouse = false;
                }
                else if (inTownHouse)
                {
                    player.trans.position = new Vector2(35.48f, 18.32f);
                    inTownHouse = false;
                }
                else if (inSnowHouse)
                {
                    player.trans.position = new Vector2(98.54f, -49.83f);
                    inSnowHouse = false;
                }
                else if (inFarmHouse)
                {
                    player.trans.position = new Vector2(4.52f, 42.53f);
                    inFarmHouse = false;
                }
                else if (inChiefHouse)
                {
                    player.trans.position = new Vector2(42.46f, 47.01f);
                    inChiefHouse = false;
                }
                else if (inPilotHouse)
                {
                    player.trans.position = new Vector2(42.45f, 33.36f);
                    inPilotHouse = false;
                }
                else if (inGraveHouse)
                {
                    player.trans.position = new Vector2(49.45f, 10.97f);
                    inGraveHouse = false;
                }
                else if (inWaterfall)
                {
                    player.trans.position = new Vector2(12.53f, 34.186f);
                    inWaterfall = false;
                }
                else if (inCrypt)
                {
                    player.trans.position = new Vector2(63.5f, 6.1f);
                    inCrypt = false;
                }
                break;     
        }
    }
}
