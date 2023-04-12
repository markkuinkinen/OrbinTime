using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public Transform trans;
    public SpriteRenderer sRend;
    public ParticleSystem psystem;
    public ParticleSystem emission;

    private PlayerController player;
    public MonsterController MonController;
    public SoundController soundController;

    public bool aggroed;
    public bool isAlive;
    public bool soundPlayed;
    public bool playerRight;
    public bool playerLeft;
    public bool playerUp;
    public bool playerDown;

    public float moveSpeed = 0.2f;
    public int health = 1;

    string currentState;
    const string SLIME_IDLE = "gslime_idle";
    const string SLIME_MOVE = "gslime_left";
    const string SLIME_DIE = "gslime_die";

    void Start()
    {
        soundController = FindObjectOfType<SoundController>();
        MonController = FindObjectOfType<MonsterController>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        trans = GetComponent<Transform>();
        sRend = GetComponent<SpriteRenderer>();
        psystem = GetComponentInChildren<ParticleSystem>();
        psystem.Stop();
        isAlive = true;
        aggroed = false;
    }

    void Update()
    {
        trackPlayer();
    }

    void FixedUpdate()
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
                ChangeAnimationState(SLIME_IDLE);
            }
        } 
        else
        {
            if (!soundPlayed)
            {
                soundController.playSlimeDeathSound();
                soundPlayed = true;
            }
            rb.velocity = new Vector2(0f, 0f);
            ChangeAnimationState(SLIME_DIE);
        }
    }

    void destroySlime()
    {
        psystem.Play();

        Destroy(sRend);
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(GetComponent<CapsuleCollider2D>());
        Destroy(GetComponent<CircleCollider2D>());
        Invoke("deleteSlime", 4f);
    }

    void deleteSlime()
    {
        Destroy(this.gameObject);
    }

    public void increaseSlimeCounter()
    {
        MonController.slimeCounter += 1;
    }

    void followPlayer()
    {
        if (playerRight)     // if player is to the right > move right
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            sRend.flipX = true;
            ChangeAnimationState(SLIME_MOVE);
            
            if (playerUp)     // if player is above/below slime > move up/down
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
            ChangeAnimationState(SLIME_MOVE);

            if (playerUp)     // if player is above/below slime > move up/down
            {
                rb.velocity = new Vector2(rb.velocity.x, moveSpeed);
            }
            else if (playerDown)
            {
                rb.velocity = new Vector2(rb.velocity.x, -moveSpeed);
            }
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

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        anim.Play(newState);

        currentState = newState;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Attack" && this.transform.GetChild(0).tag == "enemyDeathCollider")
        {

            if (Vector3.Distance(this.trans.position, player.transform.position) <= 1.5f)
            {
                isAlive = false;
                Invoke("destroySlime", 1f);
            }

            //if the player attacks, the attack collider is activated, if the attack collider hits the slime's "enemydeathcollider" collider, the slime dies
        }

        if (other.tag == "Player")
        {
            Debug.Log("slime in range");
            aggroed = true;

            //if the slime sees that the player is in range, it will follow the player by becoming aggroed
        }
    }
}
