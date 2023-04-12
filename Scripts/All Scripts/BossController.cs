using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer sRend;
    public Transform trans;

    public Transform spawnPosition;
    public BossPlayer player;
    public AudioSource bossSoundPlayer;
    public AudioClip hitSound;

    public Transform projectileSpawnLocation;
    public GameObject firstProjectile;
    public GameObject firstHit;
    public GameObject secondProjectile;
    public GameObject secondHit;
    public float projectileSpeed = 7f;
    public float slowerProjectileSpeed = 3.5f;

    public bool isAttacking;
    public bool isAlive;
    public bool isMovingLeft;
    public bool isMovingRight;

    public float moveSpeed = 4f;
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    public string currentState;
    const string BOSS_IDLE = "Boss_idle";
    const string BOSS_MOVEL = "Boss_walkL";  //need to flip when moving >
    const string BOSS_ATTACK = "Boss_attack";
    const string BOSS_ATTACK2 = "Boss_attack2";
    const string BOSS_DEATH = "Boss_death";

    public GameObject wholeHP;
    public Transform healthBar;
    public GameObject firstBar;
    public Transform secondHealthBar;
    public GameObject secondBar;
    public GameObject hit;

    void Start()
    {
        trans = GetComponent<Transform>();
        sRend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        isAlive = true;
        isMovingLeft = true;
    }

    void Update()
    {
        if (isAlive && player.isAlive)
        {
            bossFightMovement();
            healthController();
        }
        else if (!isAlive)
        {
            rb.velocity = Vector2.zero;
            ChangeAnimationState(BOSS_DEATH);
        }
        else
        {
            rb.velocity = Vector2.zero;
            ChangeAnimationState(BOSS_IDLE);
        }
    }

    public void resetBoss()
    {
        isAttacking = false;
        this.trans.position = new Vector2(spawnPosition.position.x, spawnPosition.position.y);
        isMovingLeft = true;
        isAlive = true;
        resetHealth();
    }

    void resetHealth()
    {
        currentHealth = 100;
        healthBar.localScale = new Vector2(1, 1);
        secondHealthBar.localScale = new Vector2(1, 1);
        firstBar.SetActive(true);
        secondBar.SetActive(false);
    }

    void healthController()
    {
        if (currentHealth >= 0)
        {
            healthBar.localScale = new Vector3(currentHealth / 100f, 1f);
            if (currentHealth == 0)
            {
                secondBar.SetActive(true);
                firstBar.SetActive(false);
            }
        }
        else if (currentHealth <= 0 && currentHealth > -100)
        {
            secondHealthBar.localScale = new Vector3((1 - (currentHealth * -1) / 100f), 1f);
        }
        else if (currentHealth <= -100)
        {
            isAlive = false;
            wholeHP.SetActive(false);
        }
    }

    void isCurrentlyAttacking()
    {
        isAttacking = true;
    }

    void attackFinished()
    {
        isAttacking = false;
        ChangeAnimationState(BOSS_IDLE);
    }

    void Attack()
    {
        int attackDecider = Random.Range(1, 3);


        if (attackDecider == 1)
        {
            sRend.flipX = false;
            ChangeAnimationState(BOSS_ATTACK);
            isAttacking = true;
            attackDecider = 0;
        }
        else if (attackDecider == 2)
        {
            sRend.flipX = false;
            ChangeAnimationState(BOSS_ATTACK2);
            isAttacking = true;
            attackDecider = 0;
        }
    }

    void tripleAttack()
    {
        if (currentHealth > 0)      // for first phase attacks
        {
            GameObject projectileLeft = Instantiate(firstProjectile, projectileSpawnLocation.position, Quaternion.identity);
            Rigidbody2D rbLeft = projectileLeft.GetComponent<Rigidbody2D>();
            rbLeft.velocity = new Vector2(-projectileSpeed, -projectileSpeed);

            GameObject projectileDown = Instantiate(firstProjectile, projectileSpawnLocation.position, Quaternion.identity);
            Rigidbody2D rbDown = projectileDown.GetComponent<Rigidbody2D>();
            rbDown.velocity = new Vector2(0, -projectileSpeed);

            GameObject projectileRight = Instantiate(firstProjectile, projectileSpawnLocation.position, Quaternion.identity);
            Rigidbody2D rbRight = projectileRight.GetComponent<Rigidbody2D>();
            rbRight.velocity = new Vector2(projectileSpeed, -projectileSpeed);
        }
        else                    // for second phase attacks
        {
            GameObject projectileLeft = Instantiate(secondProjectile, projectileSpawnLocation.position, Quaternion.identity);
            Rigidbody2D rbLeft = projectileLeft.GetComponent<Rigidbody2D>();
            rbLeft.velocity = new Vector2(-projectileSpeed, -projectileSpeed);

            GameObject projectileDown = Instantiate(secondProjectile, projectileSpawnLocation.position, Quaternion.identity);
            Rigidbody2D rbDown = projectileDown.GetComponent<Rigidbody2D>();
            rbDown.velocity = new Vector2(0, -projectileSpeed);

            GameObject projectileRight = Instantiate(secondProjectile, projectileSpawnLocation.position, Quaternion.identity);
            Rigidbody2D rbRight = projectileRight.GetComponent<Rigidbody2D>();
            rbRight.velocity = new Vector2(projectileSpeed, -projectileSpeed);
        }

    }

    void singleAttack()     // single attack for first phase, penta attack for second  
    {
        if (currentHealth > 0)
        {
            GameObject singleProjectile = Instantiate(firstProjectile, projectileSpawnLocation.position, Quaternion.identity);
            Rigidbody2D rb = singleProjectile.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(0, -projectileSpeed);
        }
        else
        {
            GameObject projectileLeftEXTRA = Instantiate(secondProjectile, projectileSpawnLocation.position, Quaternion.identity);
            Rigidbody2D rbLeftEXTRA = projectileLeftEXTRA.GetComponent<Rigidbody2D>();
            rbLeftEXTRA.velocity = new Vector2(-slowerProjectileSpeed * 2f, -slowerProjectileSpeed);

            GameObject projectileLeft = Instantiate(secondProjectile, projectileSpawnLocation.position, Quaternion.identity);
            Rigidbody2D rbLeft = projectileLeft.GetComponent<Rigidbody2D>();
            rbLeft.velocity = new Vector2(-slowerProjectileSpeed, -slowerProjectileSpeed);

            GameObject projectileDown = Instantiate(secondProjectile, projectileSpawnLocation.position, Quaternion.identity);
            Rigidbody2D rbDown = projectileDown.GetComponent<Rigidbody2D>();
            rbDown.velocity = new Vector2(0, -slowerProjectileSpeed);

            GameObject projectileRight = Instantiate(secondProjectile, projectileSpawnLocation.position, Quaternion.identity);
            Rigidbody2D rbRight = projectileRight.GetComponent<Rigidbody2D>();
            rbRight.velocity = new Vector2(slowerProjectileSpeed, -slowerProjectileSpeed);

            GameObject projectileRightEXTRA = Instantiate(secondProjectile, projectileSpawnLocation.position, Quaternion.identity);
            Rigidbody2D rbRightEXTRA = projectileRightEXTRA.GetComponent<Rigidbody2D>();
            rbRightEXTRA.velocity = new Vector2(slowerProjectileSpeed * 2f, -slowerProjectileSpeed);
        }
    }


    void bossFightMovement()
    {
        if (isMovingLeft && !isAttacking)
        {
            rb.velocity = new Vector2(-moveSpeed, 0f);
            sRend.flipX = false;
            ChangeAnimationState(BOSS_MOVEL);
        }
        else if (isMovingRight && !isAttacking)
        {
            rb.velocity = new Vector2(moveSpeed, 0f);
            sRend.flipX = true;
            ChangeAnimationState(BOSS_MOVEL);
        }
        else //if not moving OR is attacking
        {
            if (isAttacking)
            {
                rb.velocity = Vector2.zero;
            }
            else
            {
                isAttacking = false;
                rb.velocity = Vector2.zero;
                ChangeAnimationState(BOSS_IDLE);
            }
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
        if (other.tag == "Attack")
        {
            currentHealth -= 20f;
            bossSoundPlayer.PlayOneShot(hitSound, 0.3f);
            Instantiate(hit, new Vector2(this.gameObject.GetComponent<Transform>().position.x, this.gameObject.GetComponent<Transform>().position.y - 1f), this.gameObject.GetComponent<Transform>().rotation);
            Destroy(other.gameObject);
        }

        if (other.tag == "Node")
        {
            if (other.GetComponent<NodeScript>().canMoveLeft == true && (other.GetComponent<NodeScript>().canMoveRight == true))
            {
                int decider = Random.Range(1, 4);
                if (decider == 0)
                {
                    isMovingLeft = true;
                    isMovingRight = false;
                }
                else if (decider == 1)
                {
                    Attack();
                }
                else if (decider == 2)
                {
                    isMovingRight = true;
                    isMovingLeft = false;
                }
                decider = 0;
            }
            else if (other.GetComponent<NodeScript>().canMoveLeft == true)
            {
                int decider = Random.Range(1, 3);
                if (decider == 1)
                {
                    isMovingRight = false;
                    isMovingLeft = true;
                }
                else if (decider == 2)
                {
                    Attack();
                    isMovingRight = false;
                    isMovingLeft = true;
                }
            }
            else if (other.GetComponent<NodeScript>().canMoveRight == true)
            {
                int decider = Random.Range(1, 3);
                if (decider == 1)
                {
                    isMovingRight = true;
                    isMovingLeft = false;
                }
                else if (decider == 2)
                {
                    Attack();
                    isMovingRight = true;
                    isMovingLeft = false;
                }
            }
            else
            {
                Attack();
            }
        } 
    }
}
