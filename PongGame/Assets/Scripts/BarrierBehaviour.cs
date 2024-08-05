using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierBehaviour : MonoBehaviour
{
    public float respawnTime = 15f; // Time in seconds before the barrier reappears

    public SpriteRenderer spriteRenderer;
    public Collider2D barrierCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        barrierCollider = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bomb"))
        {
            StartCoroutine(RespawnBarrier());
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
