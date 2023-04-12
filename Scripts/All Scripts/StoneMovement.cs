using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Transform trans;
    public StoneMover stoneMover;
    
    public GameObject hit;  // for player attack
    public GameObject firstProjectileHit;   // for first phase enemy attack
    public GameObject secondProjectileHit;
    Vector2 initialPosition;

    public bool canSpawnStone;

    void Start()
    {
        stoneMover = FindObjectOfType<StoneMover>();
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        canSpawnStone = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Attack")
        {
            Instantiate(hit, new Vector2(this.gameObject.GetComponent<Transform>().position.x + 0.5f, this.gameObject.GetComponent<Transform>().position.y), this.gameObject.GetComponent<Transform>().rotation);
            Destroy(other.gameObject);
        }
        if (other.tag == "spawner" && canSpawnStone)
        {
            stoneMover.spawnStone(Random.Range(0, 4));
            canSpawnStone = false;
        }
        if (other.tag == "despawner")
        {
            Destroy(this.gameObject);
        }
    }
}
