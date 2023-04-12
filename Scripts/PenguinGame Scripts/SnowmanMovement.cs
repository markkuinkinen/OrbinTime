using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public SpriteRenderer head;
    public SpriteRenderer body;
    public float moveSpeed = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        move();
        flip();
    }

    void move()
    {
        rb.velocity = new Vector2(moveSpeed, 0f);
    }

    void flip()
    {
        if (moveSpeed > 0)
        {
            head.flipX = false;
            body.flipX = false;
        }
        else 
        {
            head.flipX = true;
            body.flipX = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Walls")
        {
            moveSpeed *= -1;
        }
    }
}
