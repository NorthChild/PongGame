using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiTurretBulletBehavior : MonoBehaviour
{
    public string shooterTag; // Tag of the turret that shot the bullet
    public GameObject bulletExplosionPrefab;
    public AudioClip destructionSound; // Sound to play when the bullet is destroyed

    void Start()
    {
        // Set the initial tag and layer of the bullet based on the shooter tag
        if (shooterTag == "playerAntiTurret")
        {

            gameObject.tag = "playerAntiTurretBullet";
            gameObject.layer = LayerMask.NameToLayer("playerAntiTurretBullet");
        }
        else if (shooterTag == "antagonistAntiTurret")
        {
            gameObject.tag = "antagonistAntiTurretBullet";
            gameObject.layer = LayerMask.NameToLayer("antagonistAntiTurretBullet");
        }
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    // Check if the bullet was shot by a playerAntiTurret
    //    if (shooterTag == "playerAntiTurret" && collision.gameObject.CompareTag("antagonistBullet"))
    //    {
    //        // If the bullet hits an antagonistBullet, destroy both bullets
    //        HandleCollision();
    //        Destroy(collision.gameObject); // Destroy the enemy bullet
    //    }
    //    // Check if the bullet was shot by an antagonistAntiTurret
    //    else if (shooterTag == "antagonistAntiTurret" && collision.gameObject.CompareTag("playerBullet"))
    //    {
    //        // If the bullet hits a playerBullet, destroy both bullets
    //        HandleCollision();
    //        Destroy(collision.gameObject); // Destroy the enemy bullet
    //    }
    //    else
    //    {
    //        // If the bullet hits anything else, just explode
    //        HandleCollision();
    //    }
    //}
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the bullet was shot by a playerAntiTurret
        if (shooterTag == "playerAntiTurret")
        {
            // If the bullet hits the PlayerBarrier or Player, do nothing
            if (collision.gameObject.CompareTag("PlayerBarrier") || collision.gameObject.CompareTag("Player"))
            {
                return;
            }

            // If the bullet hits an antagonistBullet, destroy both bullets and play effects
            if (collision.gameObject.CompareTag("antagonistBullet"))
            {
                HandleCollision();
                Destroy(collision.gameObject); // Destroy the enemy bullet
                Debug.Log("antagonist bullet destroyed");
            }
            else
            {
                // If it hits anything else, just destroy the bullet
                Destroy(gameObject);
            }
        }
        // Check if the bullet was shot by an antagonistAntiTurret
        else if (shooterTag == "antagonistAntiTurret")
        {
            // If the bullet hits the AntagonistBarrier or Antagonist, do nothing
            if (collision.gameObject.CompareTag("AntagonistBarrier") || collision.gameObject.CompareTag("Antagonist"))
            {
                return;
            }

            // If the bullet hits a playerBullet, destroy both bullets and play effects
            if (collision.gameObject.CompareTag("playerBullet"))
            {
                HandleCollision();
                Destroy(collision.gameObject); // Destroy the enemy bullet
                Debug.Log("player bullet destroyed");
            }
            else
            {
                // If it hits anything else, just destroy the bullet
                Destroy(gameObject);
            }
        }
    }

    private void HandleCollision()
    {
        // Play the destruction sound
        if (destructionSound != null)
        {
            // Create a temporary GameObject to play the sound
            GameObject soundObject = new GameObject("AntiTurretBulletDestructionSound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = destructionSound;
            audioSource.Play();

            // Destroy the soundObject after the sound has finished playing
            Destroy(soundObject, destructionSound.length);
        }

        // Instantiate the explosion at the bullet's position
        if (bulletExplosionPrefab != null)
        {
            Instantiate(bulletExplosionPrefab, transform.position, transform.rotation);
        }

        // Destroy the bullet immediately
        Destroy(gameObject);
    }

}
