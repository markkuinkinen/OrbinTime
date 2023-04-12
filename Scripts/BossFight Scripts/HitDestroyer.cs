using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDestroyer : MonoBehaviour
{
    void Start()
    {
        Invoke("DestroySelf", 1f);
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
