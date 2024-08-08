using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public string shooterTag; // Tag of the turret that shot the bullet
    public GameObject bulletExplosionPrefab;
    public AudioClip destructionSound; // Sound to play when the bullet is destroyed

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
        // Check if the bullet was shot by a playerTurret
        if (shooterTag == "playerTurret")
        {
            // If the bullet hits the PlayerBarrier or Player, do nothing
            if (collision.gameObject.CompareTag("PlayerBarrier") || collision.gameObject.CompareTag("Player"))
            {
                return;
            }
        }
        // Check if the bullet was shot by an adversaryTurret
        else if (shooterTag == "antagonistTurret")
        {
            // If the bullet hits the AntagonistBarrier or Antagonist, do nothing
            if (collision.gameObject.CompareTag("AntagonistBarrier") || collision.gameObject.CompareTag("Antagonist"))
            {
                return;
            }
        }

        // Play the destruction sound
        if (destructionSound != null)
        {
            // Create a temporary GameObject to play the sound
            GameObject soundObject = new GameObject("BulletDestructionSound");
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
