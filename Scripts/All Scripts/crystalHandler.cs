using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crystalHandler : MonoBehaviour
{
    public pacmantest Pacman;
    public LocationHandler locHandler;

    void Start()
    {
        Pacman = FindObjectOfType<pacmantest>();
        locHandler = FindObjectOfType<LocationHandler>();
    }

    void Update()
    {
        if (locHandler.inPacmanGame)
        {
            if (!Pacman.isAlive && !Pacman.gameFinished)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    this.gameObject.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.gameObject.SetActive(false);
            Pacman.score += 1;
        }
    }
}
