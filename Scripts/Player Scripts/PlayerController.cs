using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // components
    Rigidbody2D rb;
    public Transform trans;
    public LocationHandler locHandler;
    public NPCandQuestHandler questHandler;
    public SpriteRenderer sRend;
    MonsterController MonController;
    private AudioSource playerAudio;
    public GameObject flash;
    public Transform flashPosition;

    // Player
    public GameObject playerCamera;
    public bool canMove;
    float walkSpeed = 6f;
    float speedLimiter = 0.7f;
    float inputHorizontal;
    float inputVertical;

    // Animations and states
    public Animator anim;
    string currentState;
    const string PLAYER_IDLE_UP = "Idle_up";           // 4 Idle states (up down left right)
    const string PLAYER_IDLE_DOWN = "Idle_down";
    const string PLAYER_IDLE_LEFT = "Idle_left";
    const string PLAYER_IDLE_RIGHT = "Idle_right";
    const string PLAYER_WALK_UP = "Walk_up";           // 4 walking direction states (up down left right)
    const string PLAYER_WALK_DOWN = "Walk_down";
    const string PLAYER_WALK_LEFT = "Walk_left";
    const string PLAYER_WALK_RIGHT = "Walk_right";
    const string PLAYER_ATTACK_UP = "Attack_up";
    const string PLAYER_ATTACK_DOWN = "Attack_down";
    const string PLAYER_ATTACK_LEFT = "Attack_left";
    const string PLAYER_ATTACK_RIGHT = "Attack_right";
    public string PLAYER_DEATH = "Die_left";
    const string PLAYER_UPDATE = "sprite_update";

    public bool isAttacking;
    public bool attackBoxActive;
    public bool Attackable;
    public BoxCollider2D hitBox;
    public GameObject[] attackBoxes; // up, down, left, right
    public GameObject[] hearts; //0, 1, 2
    public int maxHealth = 2;
    public int currentHealth;
    public GameObject heartBox;

    public GameObject allOrbs;
    public bool invulnerable;
    public AudioClip attackSound;
    public AudioClip bossAttackSound;
    public AudioClip hitSound;
    public AudioClip waterSound;
    public AudioClip slimeGameDone;
    public GameObject deathText;
    public bool killedByZombie;
    public bool killedBySlime;
    public bool killedByGhost;
    public bool graveSpark;

    public bool facingUp;
    public bool facingDown;
    public bool facingLeft;
    public bool facingRight;

    //for slime game
    private SlimeController slime;
    public slimeGameManager slimeManager;
    spawnerScript spawner;
    public GameObject slimeCamera;
    public int slimeWater;
    public bool isAlive;
    public GameObject well;
    public GameObject wellCollider;

    //for penguin game
    float sideMovement = 5f;
    float slideSpeed = -4f;
    float playerCamY;
    float timer;
    public bool canTurn;
    public GameObject everythingPenguin;
    public GameObject penguinDeathText;
    public GameObject penguinSprite;
    public GameObject penguinCamera;
    public bool canTalkToPenguin;

    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        slimeManager = FindObjectOfType<slimeGameManager>();
        MonController = FindObjectOfType<MonsterController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        trans = gameObject.GetComponent<Transform>();
        spawner = FindObjectOfType<spawnerScript>();
        sRend = GetComponent<SpriteRenderer>();
        penguinCamera.SetActive(false);
        slime = FindObjectOfType<SlimeController>();
        canMove = true;
        isAlive = false;
        canTurn = true;
        Attackable = true;
        currentHealth = 2;

        if (facingUp)
        {
            ChangeAnimationState(PLAYER_IDLE_UP);
        }
        else if (facingDown)
        {
            ChangeAnimationState(PLAYER_IDLE_DOWN);
        }
        else if (facingLeft)
        {
            ChangeAnimationState(PLAYER_IDLE_LEFT);
        }
        else if (facingRight)
        {
            ChangeAnimationState(PLAYER_IDLE_RIGHT);
        }
    }

    void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        sparkChecker();

        if (!locHandler.inPenguinGame && !locHandler.inSlimeGame && !locHandler.inPacmanGame && canMove)
        {
            Attack();
            attackBoxController();
        }

        if (invulnerable)
        {
            sRend.color = new Color(sRend.color.r, sRend.color.b, sRend.color.g, 0.4f);
            Invoke("resetInvulnerability", 1.5f);
        }
        else
        {
            sRend.color = new Color(sRend.color.r, sRend.color.b, sRend.color.g, 1f);
        }

        if (isAttacking)
        {
            canMove = false;
            Debug.Log("can move should be false");
        }
        else
        {
            canMove = true;
        }

        if (locHandler.inPenguinGame)
        {
            if (isAlive && Input.GetKeyDown(KeyCode.R))
            {
                locHandler.spawn(-61.7f, -41.2f);
                sideMovement = 5f;
                slideSpeed = -4f;
            }
            playerCamera.SetActive(false);
            penguinCamera.SetActive(true);
            heartBox.SetActive(false);
            allOrbs.SetActive(false);
            if (!isAlive)
            {
                rb.velocity = new Vector2(0f, 0f);
                penguinDeathText.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                {
                    isAlive = true;
                    penguinDeathText.SetActive(false);
                    locHandler.spawn(-61.7f, -41.2f);
                    sideMovement = 5f;
                    slideSpeed = -4f;
                }
            }
        } 
        if (!locHandler.inPenguinGame && !locHandler.inSlimeGame && !locHandler.inPacmanGame && !locHandler.inBossFight)
        {
            allOrbs.SetActive(true);
            heartBox.SetActive(true);
            penguinCamera.SetActive(false);
            playerCamera.SetActive(true);
            lifeController();
        }
    }

    void FixedUpdate()
    {
        if (canMove)      
        {
            if (locHandler.inSlimeGame)
            {
                playerCamera.SetActive(false);
                heartBox.SetActive(false);
                allOrbs.SetActive(false);
                slimeCamera.SetActive(true); 
            }

            if (canTurn && canMove && !locHandler.inPenguinGame && !locHandler.inPacmanGame)
            {
                move();
            }
            else
            {
                rb.velocity = new Vector2(0f, 0f);
                if (!locHandler.inPenguinGame && isAlive)
                {
                    ChangeAnimationState(PLAYER_IDLE_DOWN);
                    facingUp = false;
                }
            }

            if (locHandler.inPenguinGame && isAlive)
            {
                everythingPenguin.SetActive(true);
                penguinSprite.SetActive(true);
                slideMovement();
                Debug.Log(playerCamera.transform.position.y);
                increaseSpeed();
            } 
        } 
        else if (!canMove)
        {
            rb.velocity = new Vector2(0f, 0f);
            idle();
        }
    }

    void sparkChecker()
    {
        if (graveSpark)
        {
            Instantiate(flash, flashPosition.position, flashPosition.rotation);
        }
    }

    void slideMovement()        // for penguin game
    {
        rb.velocity = new Vector2(rb.velocity.x, slideSpeed);
        canTurn = false;
        ChangeAnimationState(PLAYER_IDLE_DOWN);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(-sideMovement, rb.velocity.y);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(sideMovement, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    void lifeController()
    {
        if (currentHealth >= 2)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i].SetActive(true);
            }
        }
        else if (currentHealth == 1)
        {
            hearts[2].SetActive(false);
        }
        else if (currentHealth == 0)
        {
            hearts[1].SetActive(false);
        }
        else
        {
            hearts[0].SetActive(false);
            deathText.SetActive(true);
            canTurn = false;
            isAlive = false;
            rb.velocity = new Vector2(0f, 0f);
            ChangeAnimationState(PLAYER_DEATH);
            anim.SetBool("isDead", true);
            isAttacking = false;
            facingUp = false;
            facingDown = false;
            facingLeft = false;
            facingRight = false;
            
            if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)) && !canTurn && !isAlive && !locHandler.inSlimeGame)
            {
                if (killedByZombie)
                {
                    MonController.despawnZombies();
                    locHandler.inGraveHouse = true;
                    locHandler.spawn(116f, 14.6f);
                    MonController.spawnZombies();
                }
                else if (killedBySlime)
                {
                    MonController.despawnSlimes();
                    locHandler.inFarmHouse = true;
                    locHandler.spawn(-3.71f, 80.4f);
                    MonController.spawnSlimes();
                }

                canMove = true;
                isAlive = true;
                anim.SetBool("isDead", false);
                facingDown = true;
                ChangeAnimationState(PLAYER_IDLE_DOWN);
                Debug.Log("respawned");
                deathText.SetActive(false);
                canTurn = true;
                currentHealth = 2;
            }
        }
    }

    public void deathState()
    {
        ChangeAnimationState(PLAYER_DEATH);
        rb.velocity = new Vector2(0f, 0f);
        facingUp = false;
        facingDown = false;
        facingLeft = false;
        facingRight = false;
        isAttacking = false;
    }

    public void spawnState()
    {
        ChangeAnimationState(PLAYER_IDLE_DOWN);
        facingDown = true;
    }

    void increaseSpeed()        // for penguin game
    {
        timer += Time.deltaTime;
        if (timer > 2f)
        {
            slideSpeed -= 1f;
            sideMovement += 0.25f;
            timer = 0f;
        }
    }

    void move()
    {
        if (inputHorizontal != 0 || inputVertical != 0)         // True if going horizontal OR vertical
        {
            if (inputHorizontal != 0 && inputVertical != 0)     // True if both are pressed = going diagonal
            {
                inputHorizontal *= speedLimiter;                // Limits speed if both directions are pressed
                inputVertical *= speedLimiter;
            }

            //rb.velocity = new Vector2(inputHorizontal * walkSpeed, inputVertical * walkSpeed);        // 2 methods to make diagonal speed normal
            rb.velocity = new Vector2(inputHorizontal, inputVertical).normalized * walkSpeed;

            if (inputVertical > 0)      // Walking up 
            {
                facingUp = true;            // These variables keep track of facing direction, used in IDLE animation
                facingDown = false;
                facingLeft = false;
                facingRight = false;
                anim.SetBool("facingUp", true);
                anim.SetBool("facingDown", false);
                anim.SetBool("facingLeft", false);
                anim.SetBool("facingRight", false);

                ChangeAnimationState(PLAYER_WALK_UP);
            }
            else if (inputVertical < 0)     // Walking down
            {
                facingDown = true;
                facingUp = false;
                facingLeft = false;
                facingRight = false;
                anim.SetBool("facingUp", false);
                anim.SetBool("facingDown", true);
                anim.SetBool("facingLeft", false);
                anim.SetBool("facingRight", false);

                ChangeAnimationState(PLAYER_WALK_DOWN);
            }
            else if (inputHorizontal > 0)       //Walking right
            {
                facingRight = true;
                facingUp = false;
                facingDown = false;
                facingLeft = false;
                anim.SetBool("facingUp", false);
                anim.SetBool("facingDown", false);
                anim.SetBool("facingLeft", false);
                anim.SetBool("facingRight", true);

                ChangeAnimationState(PLAYER_WALK_RIGHT);
            }
            else if (inputHorizontal < 0)       // Walking left 
            {
                facingLeft = true;
                facingUp = false;
                facingDown = false;
                facingRight = false;
                anim.SetBool("facingUp", false);
                anim.SetBool("facingDown", false);
                anim.SetBool("facingLeft", true);
                anim.SetBool("facingRight", false);

                ChangeAnimationState(PLAYER_WALK_LEFT);
            }
        }
        else
        {
            rb.velocity = new Vector2(0f, 0f);                  // Stops player if no inputs are pressed

            // Changes Idle animation depending on most recent direction
            if (facingUp)
            {
                ChangeAnimationState(PLAYER_IDLE_UP);
            }
            else if (facingDown)
            {
                ChangeAnimationState(PLAYER_IDLE_DOWN);
            }
            else if (facingLeft)
            {
                ChangeAnimationState(PLAYER_IDLE_LEFT);
            }
            else if (facingRight)
            {
                ChangeAnimationState(PLAYER_IDLE_RIGHT);
            }
        }
    }

    void idle()
    {
        if (facingUp)
        {
            ChangeAnimationState(PLAYER_IDLE_UP);
        }
        else if (facingDown)
        {
            ChangeAnimationState(PLAYER_IDLE_DOWN);
        }
        else if (facingLeft)
        {
            ChangeAnimationState(PLAYER_IDLE_LEFT);
        }
        else if (facingRight)
        {
            ChangeAnimationState(PLAYER_IDLE_RIGHT);
        }
    }

    void resetAttack()
    {
        isAttacking = false;
        
    }

    public void makeAttackBoxActive()
    {
        attackBoxActive = true;
        hitBox.enabled = false;
    }
    public void resetAttackBox()
    {
        attackBoxActive = false;
        hitBox.enabled = true;
    }

    public void playAttackSound()
    {
        if (!locHandler.inBossFight)
        {
            playerAudio.PlayOneShot(attackSound);
        }
        else
        {
            playerAudio.PlayOneShot(bossAttackSound);
        }
    }

    void attackBoxController()
    {
        if (attackBoxActive)
        {
            if (facingUp)
            {
                attackBoxes[0].SetActive(true);
            }
            else if (facingDown)
            {
                attackBoxes[1].SetActive(true);
            }
            else if (facingLeft)
            {
                attackBoxes[2].SetActive(true);
            }
            else if (facingRight)
            {
                attackBoxes[3].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < attackBoxes.Length; i++)
            {
                attackBoxes[i].SetActive(false);
            }
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isAttacking = true;
            anim.SetTrigger("Attacking");
        }
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        anim.Play(newState);

        currentState = newState;
    }

    public void idleDown()
    {
        anim.Play(PLAYER_IDLE_DOWN);
    }

    public void idleUp()
    {
        anim.Play(PLAYER_IDLE_UP);
    }

    public bool talking()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void changeSprite()
    {
        anim.SetBool("UpdatedSprite", true);
        facingDown = false;
        facingUp = false;
        facingLeft = false;
        facingRight = false;
        ChangeAnimationState(PLAYER_UPDATE);
    }

    public bool isFacingUp()
    {
        if (facingUp)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void stopMoving()
    {
        canMove = false;
    }

    public void startMoving()
    {
        canMove = true;
    }

    public void spawnLocation(float x, float y, float z)
    {
        gameObject.transform.position = new Vector3(x, y, z);
    }

    public void exitFirstHouse()
    {
        transform.position = new Vector3(-3.5f, 8.33f, 0f);
    }

    void resetInvulnerability()
    {
        invulnerable = false;
    }

    public void makeUnAttackable()
    {
        Attackable = false;
    }

    public void makeAttackable()
    {
        Attackable = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")   // for waterfall level
        {
            if (locHandler.inWaterfall)
            {
                locHandler.spawn(-73.71f, 66.7f);
            }
            if (locHandler.inPenguinGame)
            {
                isAlive = false;
            }
            if (locHandler.inSlimeGame)
            {
                isAlive = false;
            }
        }

        if (other.tag == "slime" && this.tag == "Player")     //works just terribly
        {
            if (!invulnerable && Attackable)
            {
                currentHealth -= 1;
                playerAudio.PlayOneShot(hitSound, 0.3f);
                invulnerable = true;
                killedByZombie = false;
                killedBySlime = true;
            }
        }

        if (other.tag == "flash")
        {
            graveSpark = true;
        }

        if (other.tag == "graveyardZombie" && this.tag == "Player")
        {
            if (!invulnerable && Attackable)
            {
                playerAudio.PlayOneShot(hitSound, 0.3f);
                invulnerable = true;
                currentHealth -= 1;
                killedBySlime = false;
                killedByZombie = true;                
            }
        }

        if (other.tag == "Finish")  //penguinHouse or something
        {
            penguinSprite.SetActive(false);
            everythingPenguin.SetActive(false);
            locHandler.spawn(14.9f, -55.9f);
            facingDown = true;
            ChangeAnimationState(PLAYER_IDLE_DOWN);
            locHandler.inPenguinGame = false;
            isAlive = false;
            canTurn = true;
        }

        if (other.tag == "Penguin")
        {
            canTalkToPenguin = true;
        }

        if (other.tag == "PenguinHouseDoor")
        {
            spawnLocation(79.49f, -41.48f, 0f);
        }

        if (other.tag == "water" && slimeManager.slimeWater <= 30)
        {
            slimeManager.slimeWater += 1;
            Destroy(other.gameObject);
            playerAudio.PlayOneShot(waterSound, 0.6f);
            if (slimeManager.slimeWater >= 30)
            {
                playerAudio.PlayOneShot(slimeGameDone);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Penguin")
        {
            canTalkToPenguin = false;
        }
    }
}