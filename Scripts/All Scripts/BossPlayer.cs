using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlayer : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public Transform trans;
    public Transform spawnPosition;
    public AudioSource bossAudio;
    public AudioClip hitSound;

    public float moveSpeed = 4f;

    public bool isAttacking;
    public bool isAlive;

    public int maxHealth = 2;
    public int currentHealth = 2;
    public GameObject[] healthObjects;

    public GameObject projectile;
    public Transform projectileSpawnLocation;
    public GameObject attack1;
    public GameObject attack2;

    private string currentState;
    const string PLAYER_IDLE = "Idle_up";
    const string PLAYER_MOVELEFT = "Walk_left";
    const string PLAYER_MOVERIGHT = "Walk_right";
    const string PLAYER_ATTACK = "Attack";
    const string PLAYER_DEATH = "Death";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isAlive = true;
    }

    void FixedUpdate()
    {
        if (isAlive)
        {
            Attack();
            bossFightMovement();
            healthController();
        }
        else
        {
            ChangeAnimationState(PLAYER_DEATH);
            rb.velocity = Vector2.zero;
        }
    }

    public void resetPlayer()
    {
        trans.position = new Vector2(spawnPosition.position.x, spawnPosition.position.y);
        isAlive = true;
        currentHealth = 2;
        isAttacking = false;
        ChangeAnimationState(PLAYER_IDLE);
    }

    void healthController()
    {
        if (currentHealth == maxHealth)
        {
            healthObjects[0].SetActive(true);
            healthObjects[1].SetActive(true);
        }
        else if (currentHealth == 1)
        {
            healthObjects[0].SetActive(false);
        }
        else
        {
            healthObjects[1].SetActive(false);
            isAlive = false;
        }
    }

    void isCurrentlyAttacking()
    {
        isAttacking = true;
    }

    void attackFinished()
    {
        isAttacking = false;
        //ChangeAnimationState(PLAYER_IDLE);
    }

    void Attack()
    {
        if (Input.GetKey(KeyCode.Space) && !isAttacking && currentState != PLAYER_ATTACK)
        {
            isAttacking = true;
            ChangeAnimationState(PLAYER_ATTACK);
        }
    }

    public void spawnProjectile()
    {
        GameObject projectileUp = Instantiate(projectile, projectileSpawnLocation.position, Quaternion.identity);
        Rigidbody2D rbUp = projectileUp.GetComponent<Rigidbody2D>();
        rbUp.velocity = new Vector2(0, moveSpeed * 2f);
    }

    void bossFightMovement()
    {
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !isAttacking && trans.position.x > -11f)
        {
            rb.velocity = new Vector2(-moveSpeed, 0f);
            ChangeAnimationState(PLAYER_MOVELEFT);
        }
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !isAttacking)
        {
            rb.velocity = new Vector2(moveSpeed, 0f);
            ChangeAnimationState(PLAYER_MOVERIGHT);
        }
        else //if not moving OR is attacking
        {
            if (isAttacking)
            {
                rb.velocity = Vector2.zero;
                //ChangeAnimationState(PLAYER_ATTACK);
            }
            else
            {
                isAttacking = false;
                rb.velocity = Vector2.zero;
                ChangeAnimationState(PLAYER_IDLE);
            }

        }
    }

    void ChangeAnimationState(string newState)
    {
        // Stops animation from interrupting itself if it's the same
        if (currentState == newState) return;

        // Plays animation
        anim.Play(newState);

        // Updates the current state
        currentState = newState;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "firstAttack")
        {
            Instantiate(attack1, new Vector2(trans.position.x, trans.position.y + 0.6f), trans.rotation);
            currentHealth -= 1;
            bossAudio.PlayOneShot(hitSound, 0.3f);
            Debug.Log("dejj to 1st");
        }
        if (other.tag == "secondAttack")
        {
            currentHealth -= 1;
            bossAudio.PlayOneShot(hitSound, 0.3f);
            Instantiate(attack2, new Vector2(trans.position.x, trans.position.y + 0.6f), trans.rotation);
            Debug.Log("dejj to 2nd");
        }
        //bossAudio.PlayOneShot(hitSound, 0.4f);
    }
}
