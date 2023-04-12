using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDown : MonoBehaviour
{
    private Rigidbody2D rb;
    public PlayerController player;
    private slimeGameManager slimeManager;
    public float moveSpeed = 3f;

    void Start()
    {
        slimeManager = FindObjectOfType<slimeGameManager>();
        player = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player.isAlive && !slimeManager.gameWon)
        {
            move();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void move()
    {
        rb.velocity = new Vector2(0f, -moveSpeed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "despawner")
        {
            Object.Destroy(this.gameObject);
        }
    }
}
