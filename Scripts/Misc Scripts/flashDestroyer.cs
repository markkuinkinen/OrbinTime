using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashDestroyer : MonoBehaviour
{
    void Start()
    {
        Invoke("destroyObject", 0.1f);
    }

    void destroyObject()
    {
        Destroy(this.gameObject);
    }
}
