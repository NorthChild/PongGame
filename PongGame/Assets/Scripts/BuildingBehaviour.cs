using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehaviour : MonoBehaviour
{
    public Sprite damagedSprite; // The sprite to change to when the building is damaged
    public GameObject explosionPrefab; // Reference to the explosion prefab
    public AudioClip destructionSound; 
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private EdgeCollider2D edgeCollider;
    public PlayerUpgradeController playerUpgradeController; // Reference to the UpgradeController
    public AntagonistUpgradeController antagonistUpgradeController; // Reference to the UpgradeController
    public ScoreController scoreController;

    private bool isDamaged = false; // Flag to ensure the explosion plays only once

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        edgeCollider = GetComponent<EdgeCollider2D>();

        if (boxCollider == null)
        {
            //Debug.LogWarning("BoxCollider2D not found on " + gameObject.name);
        }

        if (edgeCollider == null)
        {
            //Debug.LogWarning("EdgeCollider2D not assigned on " + gameObject.name);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDamaged) return; // If the building is already damaged, do nothing

        if (collision.gameObject.CompareTag("Bomb") || collision.gameObject.CompareTag("playerBullet") || collision.gameObject.CompareTag("antagonistBullet"))
        {
            // Determine if the building hit is an antagonist building hit by a player bullet or a bomb
            if (gameObject.CompareTag("antagonistBuilding") && (collision.gameObject.CompareTag("playerBullet") || collision.gameObject.CompareTag("Bomb")))
            {
                HandleBuildingDamage();
                // Notify the ScoreController
                if (scoreController != null)
                {
                    scoreController.AntagonistBuildingDestroyed();
                }
                // Notify the PlayerUpgradeController
                if (playerUpgradeController != null)
                {
                    playerUpgradeController.AntagonistBuildingDestroyed();
                }
                // Change the tag of the building
                gameObject.tag = "destroyedBuilding";
            }
            // Determine if the building hit is a player building hit by an antagonist bullet or a bomb
            else if (gameObject.CompareTag("playerBuilding") && (collision.gameObject.CompareTag("antagonistBullet") || collision.gameObject.CompareTag("Bomb")))
            {
                HandleBuildingDamage();
                // Notify the ScoreController
                if (scoreController != null)
                {
                    scoreController.PlayerBuildingDestroyed();
                }
                // Notify the AntagonistUpgradeController
                if (antagonistUpgradeController != null)
                {
                    antagonistUpgradeController.PlayerBuildingDestroyed();
                }
                // Change the tag of the building
                gameObject.tag = "destroyedBuilding";
            }
        }
    }

    void HandleBuildingDamage()
    {
        // Set the building as damaged
        isDamaged = true;

        // Change the sprite to the damaged sprite
        spriteRenderer.sprite = damagedSprite;

        // Remove the BoxCollider2D component if it exists
        if (boxCollider != null)
        {
            Destroy(boxCollider);
            //Debug.Log("BoxCollider2D destroyed on " + gameObject.name);
        }

        // Remove the EdgeCollider2D component if it exists
        if (edgeCollider != null)
        {
            Destroy(edgeCollider);
            //Debug.Log("EdgeCollider2D destroyed on " + gameObject.name);
        }

        // Play the destruction sound
        if (destructionSound != null)
        {
            // Create a temporary GameObject to play the sound
            GameObject soundObject = new GameObject("BuildingDestructionSound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = destructionSound;
            audioSource.Play();

            // Destroy the soundObject after the sound has finished playing
            Destroy(soundObject, destructionSound.length);
        }

        // Instantiate the explosion at the building's position
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }
    }
}
