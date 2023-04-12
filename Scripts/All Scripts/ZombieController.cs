using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    Transform trans;
    SpriteRenderer sRend;
    MonsterController MonController;
    private PlayerController player;
    public SoundController soundController;

    public GameObject hitbox;

    public float moveSpeed = 1f;
    public int health = 1;
    public bool isAlive;
    public bool aggroed;

    string currentState;
    const string ZOMBIE_IDLE = "zombieIdle";
    const string ZOMBIE_MOVELR = "zombieWalkLR";
    const string ZOMBIE_DIE = "zombieDeath";
    const string ZOMBIE_MOVEUP = "zombieWalkUp";
    const string ZOMBIE_MOVEDOWN = "zombieWalkDown";
    const string ZOMBIE_DIEL = "zombieDeathL";

    public bool playerRight;
    public bool playerLeft;
    public bool playerUp;
    public bool playerDown;

    public bool soundPlayed;

    void Start()
    {
        soundController = FindObjectOfType<SoundController>();
        MonController = FindObjectOfType<MonsterController>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        trans = GetComponent<Transform>();
        sRend = GetComponent<SpriteRenderer>();
        isAlive = true;
        aggroed = false;
        ChangeAnimationState(ZOMBIE_IDLE);
    }

    void Update()
    {
        if (isAlive)
        {
            trackPlayer();
        }
    }

    void FixedUpdate()      //for movement
    {
        if (isAlive)
        {
            if (aggroed && player.canTurn)
            {
                followPlayer();
            }
            else
            {
                rb.velocity = new Vector2(0f, 0f);
                ChangeAnimationState(ZOMBIE_IDLE);
            }
        }
        else if (!isAlive)
        {
            if (!soundPlayed)
            {
                soundController.playZombieDeathSound();
                soundPlayed = true;
            }
            rb.velocity = new Vector2(0f, 0f);
            if (playerRight)
            {
                ChangeAnimationState(ZOMBIE_DIEL);
            }
            else
            {
                ChangeAnimationState(ZOMBIE_DIE);
            }
            Invoke("destroyZombie", 1f);
        }
    }

    void followPlayer()
    {
        if (playerRight)     // if player is to the right > move right
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            sRend.flipX = true;
            ChangeAnimationState(ZOMBIE_MOVELR);

            if (playerUp)     // if player is above/below zombie > move up/down
            {
                rb.velocity = new Vector2(rb.velocity.x, moveSpeed);
            }
            else if (playerDown)
            {
                rb.velocity = new Vector2(rb.velocity.x, -moveSpeed);
            }
        }
        else if (playerLeft)    // if player is to the left > move left
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            sRend.flipX = false;
            ChangeAnimationState(ZOMBIE_MOVELR);

            if (playerUp)     // if player is above/below zombie > move up/down
            {
                rb.velocity = new Vector2(rb.velocity.x, moveSpeed);
            }
            else if (playerDown)
            {
                rb.velocity = new Vector2(rb.velocity.x, -moveSpeed);
            }
        }
        else if (playerUp && !playerRight && !playerLeft)
        {
            rb.velocity = new Vector2(rb.velocity.x, moveSpeed);
        }
        else if (playerDown && !playerRight && !playerLeft)
        {
            rb.velocity = new Vector2(rb.velocity.x, -moveSpeed);
        }
    }

    void trackPlayer()  //constantly tracking the players position
    {

        if (trans.position.x < player.transform.position.x)     //left
        {
            playerRight = true;
            playerLeft = false;
            if (Mathf.Abs(trans.position.x - player.transform.position.x) <= 0.5f)
            {
                playerLeft = false;
                playerRight = false;
            }
        }
        else if (trans.position.x > player.transform.position.x)    //right
        {
            playerRight = false;
            playerLeft = true;
        }

        if (trans.position.y < player.transform.position.y)     //up
        {
            playerUp = true;
            playerDown = false;
            if (Mathf.Abs(trans.position.y - player.transform.position.y) <= 0.5f)
            {
                playerDown = false;
                playerUp = false;
            }
        }
        else if (trans.position.y > player.transform.position.y)    //down
        {
            playerUp = false;
            playerDown = true;
        }
    }

    public void increaseZombieCounter()
    {
        MonController.zombieCounter += 1;
    }

    void destroyZombie()
    {
        Destroy(this.gameObject);
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        anim.Play(newState);

        currentState = newState;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Attack" && this.transform.GetChild(0).tag == "enemyDeathCollider" && other.transform.parent.tag == "Player")// && this.gameObject.tag == "Enemy")
        {
            Debug.Log("zombie dies");
            if (Vector3.Distance(this.gameObject.GetComponent<Transform>().position, player.transform.position) <= 1.5f)
            {
                isAlive = false;
                hitbox.SetActive(false);
            }
        }
        if (other.tag == "Player")
        {
            aggroed = true;
        }
    }
}
