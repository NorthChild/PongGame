using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehaviour : MonoBehaviour
{
    public Sprite damagedSprite; // The sprite to change to when the building is damaged
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private EdgeCollider2D edgeCollider;
    public PlayerUpgradeController playerUpgradeController; // Reference to the UpgradeController
    public AntagonistUpgradeController antagonistUpgradeController; // Reference to the UpgradeController

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        edgeCollider = GetComponent<EdgeCollider2D>();

        if (boxCollider == null)
        {
            // Debug.LogWarning("BoxCollider2D not found on " + gameObject.name);
        }

        if (edgeCollider == null)
        {
            // Debug.LogWarning("EdgeCollider2D not assigned on " + gameObject.name);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bomb") || collision.gameObject.CompareTag("playerBullet") || collision.gameObject.CompareTag("antagonistBullet"))
        {
            
            // Determine the type of bullet that hit the building
            if ((collision.gameObject.CompareTag("playerBullet") && gameObject.CompareTag("antagonistBuilding")) || collision.gameObject.CompareTag("Bomb"))
            {
                //Debug.Log("building collided");
                HandleBuildingDamage();
                // Notify the UpgradeController
                if (playerUpgradeController != null)
                {
                    //Debug.Log("upgrade active");
                    playerUpgradeController.AntagonistBuildingDestroyed();
                }
                // Change the tag of the building
                gameObject.tag = "destroyedBuilding";
            }
            else if ((collision.gameObject.CompareTag("antagonistBullet") && gameObject.CompareTag("playerBuilding")) || collision.gameObject.CompareTag("Bomb"))
            {
                // Notify the UpgradeController
                if (antagonistUpgradeController != null)
                {
                    antagonistUpgradeController.PlayerBuildingDestroyed();
                }
                HandleBuildingDamage();
                // Change the tag of the building
                gameObject.tag = "destroyedBuilding";
            }
        }
    }

    void HandleBuildingDamage()
    {
        // Change the sprite to the damaged sprite
        spriteRenderer.sprite = damagedSprite;

        // Remove the BoxCollider2D component if it exists
        if (boxCollider != null)
        {
            Destroy(boxCollider);
            // Debug.Log("BoxCollider2D destroyed on " + gameObject.name);
        }

        // Remove the EdgeCollider2D component if it exists
        if (edgeCollider != null)
        {
            Destroy(edgeCollider);
            // Debug.Log("EdgeCollider2D destroyed on " + gameObject.name);
        }
    }
}
