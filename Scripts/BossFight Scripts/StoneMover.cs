using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMover : MonoBehaviour
{
    public Transform spawnPosition;
    public GameObject[] stones; //0-3

    public void spawnStone(int x)
    {
        Instantiate(stones[x], spawnPosition.position, spawnPosition.rotation);
    }
}
