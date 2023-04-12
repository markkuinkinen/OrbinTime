using UnityEngine;

public class ghost : MonoBehaviour
{
    public LocationHandler locHandler;
    public pGameManager gameManager;
    public pacmantest Player;
    public Rigidbody2D rb;
    public Animator anim;

    public Transform trans { get; private set; }
    public Transform boxTrans;

    public float moveSpeed = 3f;
    public float xStartingDirection;

    public Vector2 initialPosition { get; private set; }
    public Vector2 direction;
    public Vector2 nextDirection;

    public LayerMask obstacle;

    string currentState;
    const string GHOST_UP = "ghost_up";           // 4 Idle states (up down left right)
    const string GHOST_DOWN = "ghost_down";
    const string GHOST_LEFT = "ghost_left";
    const string GHOST_RIGHT = "ghost_right";

    void Start()
    {
        locHandler = FindObjectOfType<LocationHandler>();
        gameManager = FindObjectOfType<pGameManager>();
        Player = FindObjectOfType<pacmantest>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        this.initialPosition = new Vector2(this.trans.position.x, this.trans.position.y);
        direction.x = xStartingDirection;
        ChangeAnimationState(GHOST_DOWN);
    }

    private void animChecker()
    {
        if (direction.x > 0)
        {
            ChangeAnimationState(GHOST_RIGHT);
        }
        else if (direction.x < 0)
        {
            ChangeAnimationState(GHOST_LEFT);
        }
        else if (direction.y > 0)
        {
            ChangeAnimationState(GHOST_UP);
        }
        else if (direction.y < 0)
        {
            ChangeAnimationState(GHOST_DOWN);
        }
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        anim.Play(newState);

        currentState = newState;
    }


    public void resetPosition()
    {
        if (this.trans != null && this.initialPosition != null)
        {
            this.trans.position = this.initialPosition;
            ChangeAnimationState(GHOST_DOWN);
            setDirection(new Vector2(xStartingDirection, 0f));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null)
        {
            int index = Random.Range(0, node.availableDirections.Count);
            if (node.availableDirections[index] == -this.direction && node.availableDirections.Count > 1)
            {
                index++;
                if (index >= node.availableDirections.Count)
                {
                    index = 0;
                }
            }
            setDirection(node.availableDirections[index]);
        }
        if (other.tag == "Portal")
        {
            if (this.direction.x > 0)
            {
                this.trans.position = new Vector2(this.trans.position.x - 20.4f, this.trans.position.y);
            }
            else if (this.direction.x < 0)
            {
                this.trans.position = new Vector2(this.trans.position.x + 20.4f, this.trans.position.y);
            }
        }
    }

    private bool wallDetected(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxTrans.position, Vector2.one * 0.7f, 0.0f, direction, 0.5f, obstacle);

        Debug.DrawRay(boxTrans.position, direction * 1.1f, Color.green);
        Debug.DrawRay(hit.point, hit.normal, Color.red);

        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void setDirection(Vector2 direction)
    {
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

    void Update()
    {
        if (locHandler.inPacmanGame)
        {
            if (Player.isAlive && !gameManager.gameOver && Player.hasMoved)
            {
                rb.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
                anim.SetFloat("xSpeed", direction.x);
                anim.SetFloat("ySpeed", direction.y);
                animChecker();

                if (nextDirection != Vector2.zero)
                {
                    setDirection(nextDirection);
                }
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
        
    }
}
