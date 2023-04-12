using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormScript : MonoBehaviour
{
    public Transform projectileTransform;
    public GameObject projectile;
    public LocationHandler player;

    public bool rightSide;
    float timer;

    void Update()
    {
        if (player.inWaterfall)
        {
            if (rightSide)  // if enabled through inspector, changes projectile value to determine movement direction
            {
                projectile.GetComponent<wormProjectile>().isRightSide = true;
            }
            else
            {
                projectile.GetComponent<wormProjectile>().isRightSide = false;
            }
            
            timer += Time.deltaTime;
            if (timer > 1.2f)
            {
                shoot();
                timer = 0f;
            }
        }
    }

    void shoot()
    {
        Instantiate(projectile, projectileTransform.position, projectileTransform.rotation);
    }
}
