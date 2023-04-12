using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject hit;
    public float moveSpeed;
    public bool movingUp;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "despawner")
        {
            Destroy(this.gameObject);
            Debug.Log("despawned?");
        }
    }
}
