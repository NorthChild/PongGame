using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierBehaviour : MonoBehaviour
{
    public float respawnTime; // Time in seconds before the barrier reappears

    public SpriteRenderer spriteRenderer;
    public EdgeCollider2D barrierCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        barrierCollider = GetComponent<EdgeCollider2D>();
    }

    private void Update()
    {
        //Debug.Log("respawnTime: " + respawnTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bomb"))
        {
            StartCoroutine(RespawnBarrier());
        }
        else if (collision.gameObject.CompareTag("playerBullet"))
        {
            //Debug.Log("player bullet hit");
            // If the barrier is a playerBarrier, the bullet should pass through without affecting it
            if (gameObject.CompareTag("PlayerBarrier"))
            {
                Debug.Log("player bullet hit on player barrier");
                Physics2D.IgnoreCollision(collision.collider, barrierCollider);
            }
            else
            {
                StartCoroutine(RespawnBarrier());
                Destroy(collision.gameObject); // Destroy the bullet after it hits the barrier
            }
        }
        else if (collision.gameObject.CompareTag("antagonistBullet"))
        {
            // If the barrier is an antagonistBarrier, the bullet should pass through without affecting it
            if (gameObject.CompareTag("AntagonistBarrier"))
            {
                Physics2D.IgnoreCollision(collision.collider, barrierCollider);
            }
            else
            {
                StartCoroutine(RespawnBarrier());
                Destroy(collision.gameObject); // Destroy the bullet after it hits the barrier
            }
        }
    }

    IEnumerator RespawnBarrier()
    {
        // Disable the barrier
        spriteRenderer.enabled = false;
        barrierCollider.enabled = false;

        // Wait for the specified respawn time
        yield return new WaitForSeconds(respawnTime);

        // Enable the barrier
        spriteRenderer.enabled = true;
        barrierCollider.enabled = true;
    }
}
