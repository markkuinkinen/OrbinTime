using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pacmantest : MonoBehaviour
{
    public int score;
    public bool isAlive;
    public Rigidbody2D rb;
    public Transform trans;
    public Transform boxTrans;
    public pGameManager gameManager;
    Animator anim;
    public LocationHandler locHandler;

    public LayerMask obstacle;

    public float moveSpeed = 3f;

    public Vector2 direction;
    public Vector2 nextDirection;
    public Vector2 initialPosition;
    public Vector3 position;

    public bool isObstructed;
    public bool gameFinished;
    public bool hasMoved;

    private string currentState;
    const string PLAYER_IDLE_UP = "Idle_up";           // 4 Idle states (up down left right)
    const string PLAYER_IDLE_DOWN = "Idle_down";
    const string PLAYER_IDLE_LEFT = "Idle_left";
    const string PLAYER_IDLE_RIGHT = "Idle_right";
    const string PLAYER_WALK_UP = "Walk_up";           // 4 walking direction states (up down left right)
    const string PLAYER_WALK_DOWN = "Walk_down";
    const string PLAYER_WALK_LEFT = "Walk_left";
    const string PLAYER_WALK_RIGHT = "Walk_right";

    void Start()
    {
        locHandler = FindObjectOfType<LocationHandler>();
        hasMoved = false;
        gameManager = FindObjectOfType<pGameManager>();
        anim = GetComponent<Animator>();
        score = 0;
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        isAlive = true;
        gameFinished = false;
        initialPosition = new Vector2(trans.position.x, trans.position.y);
        ChangeAnimationState(PLAYER_IDLE_DOWN);
    }

    void FixedUpdate()
    {
        if (isAlive && !gameManager.gameOver && locHandler.inPacmanGame)
        {
            if (!wallDetected(direction))
            {
                rb.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void Update()
    {
        if (locHandler.inPacmanGame)
        {
            position = new Vector3(trans.position.x, trans.position.y);
            isObstructed = wallDetected(direction);
            userInput();
            animChecker();

            if (nextDirection != Vector2.zero)
            {
                setDirection(nextDirection);
            }
        }

    }

    private bool wallDetected(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxTrans.position, Vector2.one * 0.85f, 0.0f, direction, 0.7f, obstacle);

        if (hit.collider != null) 
        {
            return true;
        } 
        else 
        {
            return false;
        }
    }

    public void ChangeAnimationState(string newState)
    {
        // Stops animation from interrupting itself if it's the same
        if (currentState == newState) return;

        // Plays animation
        anim.Play(newState);

        // Updates the current state
        currentState = newState;
    }

    void animChecker()
    {
        if (direction.y < 0 && rb.velocity.y < 0)
        {
            ChangeAnimationState(PLAYER_WALK_DOWN);
        }
        else if (direction.y < 0 && rb.velocity.y == 0)
        {
            ChangeAnimationState(PLAYER_IDLE_DOWN);
        }

        if (direction.y > 0 && rb.velocity.y > 0)
        {
            ChangeAnimationState(PLAYER_WALK_UP);
        }
        else if (direction.y > 0 && rb.velocity.y == 0)
        {
            ChangeAnimationState(PLAYER_IDLE_UP);
        }

        if (direction.x < 0 && rb.velocity.x < 0)
        {
            ChangeAnimationState(PLAYER_WALK_LEFT);
        }
        else if (direction.x < 0 && rb.velocity.x == 0)
        {
            ChangeAnimationState(PLAYER_IDLE_LEFT);
        }

        if (direction.x > 0 && rb.velocity.x > 0)
        {
            ChangeAnimationState(PLAYER_WALK_RIGHT);
        }
        else if (direction.x > 0 && rb.velocity.x == 0)
        {
            ChangeAnimationState(PLAYER_IDLE_RIGHT);
        }
    }

    private void setDirection(Vector2 direction)
    {
        hasMoved = true;
        if (!wallDetected(direction))
        {
            this.direction = direction;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = direction;
        }
    }


    void userInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || (Input.GetKeyDown(KeyCode.UpArrow)))
        {
            setDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || (Input.GetKeyDown(KeyCode.DownArrow)))
        {
            setDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || (Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            setDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || (Input.GetKeyDown(KeyCode.RightArrow)))
        {
            setDirection(Vector2.right);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ghost")
        {
            isAlive = false;
            gameManager.lives -= 1;
        }

        if (other.tag == "Portal")  // -10 - 
        {
            if (direction.x > 0)
            {
                trans.position = new Vector2(trans.position.x - 20.4f, trans.position.y); //= new Vector2(, this.trans.position.y);
            }
            else if (direction.x < 0)
            {
                trans.position = new Vector2(trans.position.x + 20.4f, trans.position.y);
            }
            
        }

    }

}