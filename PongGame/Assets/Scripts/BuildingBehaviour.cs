using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehaviour : MonoBehaviour
{
    public Sprite damagedSprite; // The sprite to change to when the building is damaged
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private EdgeCollider2D edgeCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

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
        if (collision.gameObject.CompareTag("Bomb"))
        {
            //Debug.Log("Bomb collided with building: " + gameObject.name);
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
        }
    }
}
