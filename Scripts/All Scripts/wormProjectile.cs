using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wormProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    public WormScript worm;

    float moveSpeed = 4.5f;
    public bool isRightSide;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameObject.layer = 4;
    }

    void Update()
    {
        if (isRightSide)
        {
            rb.velocity = new Vector3(-moveSpeed, 0f, 0f);
        }
        else
        {
            rb.velocity = new Vector3(moveSpeed, 0f, 0f);
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
