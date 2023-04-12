using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveLeft : MonoBehaviour
{
    public PlayerController player;
    private Rigidbody2D rb;
    spawnerScript spawner;
    private slimeGameManager slimeManager;
    
    public float moveSpeed = 3f;
    public int slimeDirection;

    void Start()
    {
        slimeManager = FindObjectOfType<slimeGameManager>();
        rb = GetComponent<Rigidbody2D>();
        spawner = FindObjectOfType<spawnerScript>();
        player = FindObjectOfType<PlayerController>();
    }

    void FixedUpdate()
    {
        if (player.isAlive && !slimeManager.gameWon)
        {
            chanceOfDirection();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void chanceOfDirection()
    {
        slimeDirection = Random.Range(0, 10);
        if (slimeDirection < 4)    //normal - moving left
        {
            rb.velocity = new Vector2(-spawner.slimeSpeed, 0f);
        }
        else if (slimeDirection > 6)   // left up
        {
            rb.velocity = new Vector2((-spawner.slimeSpeed / 2), (spawner.slimeSpeed / 2));
        }
        else        // left down
        {
            rb.velocity = new Vector2((-spawner.slimeSpeed / 2), (-spawner.slimeSpeed / 2));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "despawner")
        {
            Object.Destroy(this.gameObject);
        }
    }
}
