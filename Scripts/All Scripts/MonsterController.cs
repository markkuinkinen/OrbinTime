using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    PlayerController player;
    LocationHandler locHandler;
    NPCandQuestHandler questHandler;

    // Zombie variables
    public GameObject zombieBox;
    public GameObject[] zombies;
    public int zombieCounter;
    public bool zombiesKilled;
    public bool zombieSoundPlayed;
    public GameObject ZombiePrefab;
    public Transform[] zombieSpawnLocations;
    private List<GameObject> spawnedZombies = new List<GameObject>();

    // Slime variables
    public GameObject slimeBox;
    public GameObject[] slimes;
    public int slimeCounter;
    public bool slimesKilled;
    public bool slimeSoundPlayed;
    public GameObject slimePrefab;
    public Transform[] slimeSpawnLocations;
    private List<GameObject> spawnedSlimes = new List<GameObject>();

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        locHandler = FindObjectOfType<LocationHandler>();
        questHandler = FindObjectOfType<NPCandQuestHandler>();
        spawnZombies();
        spawnSlimes();
    }

    void Update()
    {
        if (zombieCounter == 10)
        {
            zombiesKilled = true;
            if (!zombieSoundPlayed)
            {
                questHandler.playCompletedSound();
                zombieSoundPlayed = true;
            }
        }
        if (slimeCounter == 6)
        {
            slimesKilled = true;
            if (!slimeSoundPlayed)
            {
                questHandler.playCompletedSound();
                slimeSoundPlayed = true;
            }
        }
    }

    public void spawnSlimes()
    {
        if (!slimesKilled)
        {
            slimeCounter = 0;
            for (int x = 0; x < slimeSpawnLocations.Length; x++)
            {
                GameObject slm = Instantiate(slimePrefab, slimeSpawnLocations[x].position, slimeSpawnLocations[x].rotation, slimeBox.GetComponent<Transform>());
                spawnedSlimes.Add(slm);
            }
        }
    }

    public void spawnZombies()
    {
        if (!zombiesKilled)
        {
            zombieCounter = 0;
            for (int j = 0; j < zombieSpawnLocations.Length; j++)
            {
                GameObject zmb = Instantiate(ZombiePrefab, zombieSpawnLocations[j].position, zombieSpawnLocations[j].rotation, zombieBox.GetComponent<Transform>());
                spawnedZombies.Add(zmb);
            }
        }
    }

    public void despawnZombies()
    {
        foreach (GameObject zomble in spawnedZombies)
        {
            Destroy(zomble);
        }
        spawnedZombies.Clear();
    }

    public void despawnSlimes()
    {
        foreach (GameObject sloime in spawnedSlimes)
        {
            Destroy(sloime);
        }
        spawnedSlimes.Clear();
    }
}
