using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableWorms : MonoBehaviour
{
    public GameObject wormEnemies;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            wormEnemies.SetActive(false);
        }
    }
}
