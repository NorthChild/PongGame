using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public string shooterTag; // Tag of the turret that shot the bullet

    void Start()
    {
        // Set the initial tag of the bullet based on the shooter tag
        if (shooterTag == "playerTurret")
        {
            gameObject.tag = "playerBullet";
            gameObject.layer = LayerMask.NameToLayer("playerBullet");
        }
        else if (shooterTag == "antagonistTurret")
        {
            gameObject.tag = "antagonistBullet";
            gameObject.layer = LayerMask.NameToLayer("antagonistBullet");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collided");
        // Check if the bullet was shot by a playerTurret
        if (shooterTag == "playerTurret")
        {
            //Debug.Log("player turret");
            // If the bullet hits the PlayerBarrier, do nothing
            if (collision.gameObject.CompareTag("PlayerBarrier") || collision.gameObject.CompareTag("Player"))
            {
                return;
            }
        }
        // Check if the bullet was shot by an adversaryTurret
        else if (shooterTag == "antagonistTurret")
        {
            // If the bullet hits the AntagonistBarrier, do nothing
            if (collision.gameObject.CompareTag("AntagonistBarrier") || collision.gameObject.CompareTag("Antagonist"))
            {
                return;
            }
        }

        // Destroy the bullet on collision with any other object
        Destroy(gameObject);
    }
}
