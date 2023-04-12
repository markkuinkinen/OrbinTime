using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerScript : MonoBehaviour
{
    public Transform[] transformLocations;  //0-11 locations
    public GameObject[] spawnables;     //up, down, left, right, waterUp, waterDown, waterLeft, waterRight
    public PlayerController player;
    private LocationHandler locHandler;

    private int locationNumber;
    private int spawnChance;
    public float timer;
    public float spawnLimiter = 1.5f;
    public float slimeSpeed = 4f;

    void Start()
    {
        locHandler = FindObjectOfType<LocationHandler>();
    }

    void Update()
    {
        if (locHandler.inSlimeGame)
        {
            timer += Time.deltaTime;
        }
    }

    public void spawnEverything()
    {        
        if (timer >= 1.5f)
        {
            spawnThings(timer);
            spawnThings(timer);
            spawnThings(timer);
            spawnThings(timer);
            timer = 0;
        }
    }

    public void spawnThings(float spawntimer)
    {
        if (spawntimer > spawnLimiter)   // make limiter decrease 
        {
            locationNumber = Random.Range(0, 5);
            spawnChance = Random.Range(0, 4);            

            if (locationNumber == 0)    // spawns upper slime
            {
                if (spawnChance > 0)    //end
                {
                    Instantiate(spawnables[0], transformLocations[Random.Range(3, 6)].position, transformLocations[0].rotation);
                    Debug.Log("3/4 chance of slime");
                } 
                else
                {
                    Instantiate(spawnables[5], transformLocations[Random.Range(3, 6)].position, transformLocations[0].rotation);
                    Debug.Log("1/4 chance of water");
                }
            }
            if (locationNumber == 1)    // spawns lower slime
            {
                if (spawnChance > 0)
                {
                    Instantiate(spawnables[1], transformLocations[Random.Range(9, 12)].position, transformLocations[1].rotation);
                    Debug.Log("3/4 chance of slime");
                }
                else
                {
                    Instantiate(spawnables[4], transformLocations[Random.Range(9, 12)].position, transformLocations[1].rotation);
                    Debug.Log("1/4 chance of water");
                }
            }
            if (locationNumber == 2)    // spawns left side slime
            {
                if (spawnChance > 0)
                {
                    Instantiate(spawnables[2], transformLocations[Random.Range(0, 3)].position, transformLocations[2].rotation);
                    Debug.Log("3/4 chance of slime");
                }
                else
                {
                    Instantiate(spawnables[7], transformLocations[Random.Range(0, 3)].position, transformLocations[2].rotation);
                    Debug.Log("1/4 chance of water");
                }
            }
            if (locationNumber == 3)    // spawns right side slime
            {
                if (spawnChance > 0)
                {
                    Instantiate(spawnables[3], transformLocations[Random.Range(6, 9)].position, transformLocations[3].rotation);
                    Debug.Log("3/4 chance of slime");
                }
                else
                {
                    Instantiate(spawnables[6], transformLocations[Random.Range(6, 9)].position, transformLocations[3].rotation);
                    Debug.Log("1/4 chance of water");
                }
            }
        }
    }
}
