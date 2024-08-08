using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    public float initialSpeed = 10f; // Initial speed of the ball
    public float maxBounceAngle = 75f; // Maximum angle for the ball to bounce off the paddle
    public float minVerticalSpeed = 2f; // Minimum vertical speed to prevent horizontal bouncing
    public float minHorizontalSpeed = 2f; // Minimum horizontal speed to prevent vertical bouncing
    public float wallBounceForce = 55.5f; // Force applied by the wall to bounce the ball away
    public AudioClip bounceSound;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Ensure continuous collision detection
        LaunchBall();
    }

    void LaunchBall()
    {
        // Launch the ball in a random direction
        float randomAngle = UnityEngine.Random.Range(-45f, 45f);
        Vector2 direction = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad)).normalized;
        rb.velocity = direction * initialSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Reflect the ball's velocity based on the collision normal
            Vector2 reflectDir = Vector2.Reflect(rb.velocity, collision.contacts[0].normal);
            rb.velocity = reflectDir;

            // Ensure the ball doesn't get stuck bouncing horizontally
            EnsureMovement();

            // Apply wall bounce force if the ball is traveling in a straight line
            if (IsTravelingStraightLine())
            {
                Vector2 bounceDirection = collision.contacts[0].normal;
                rb.AddForce(bounceDirection * wallBounceForce, ForceMode2D.Force);
            }
        }

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Antagonist") || collision.gameObject.CompareTag("PlayerBarrier") || collision.gameObject.CompareTag("AntagonistBarrier"))
        {
            if (bounceSound != null)
            {
                // Create a temporary GameObject to play the sound
                GameObject soundObject = new GameObject("BuildingDestructionSound");
                AudioSource audioSource = soundObject.AddComponent<AudioSource>();
                audioSource.clip = bounceSound;
                audioSource.Play();

                // Destroy the soundObject after the sound has finished playing
                Destroy(soundObject, bounceSound.length);
            }

            // Calculate where on the paddle the ball hit
            float hitPoint = (transform.position.x - collision.transform.position.x) / collision.collider.bounds.size.x;
            float bounceAngle = hitPoint * maxBounceAngle;

            // Ensure the ball doesn't slide horizontally by setting a minimum angle
            bounceAngle = Mathf.Clamp(bounceAngle, -maxBounceAngle, maxBounceAngle);

            // Calculate the new velocity based on the bounce angle
            float newVelocityX = initialSpeed * Mathf.Sin(bounceAngle * Mathf.Deg2Rad);
            float newVelocityY = initialSpeed * Mathf.Cos(bounceAngle * Mathf.Deg2Rad);

            // Determine if the ball should move upwards or downwards based on the paddle
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("PlayerBarrier"))
            {
                rb.velocity = new Vector2(newVelocityX, Mathf.Abs(newVelocityY)); // Ensure upward movement
            }
            else if (collision.gameObject.CompareTag("Antagonist") || collision.gameObject.CompareTag("AntagonistBarrier"))
            {
                rb.velocity = new Vector2(newVelocityX, -Mathf.Abs(newVelocityY)); // Ensure downward movement
            }

            // Ensure the ball doesn't get stuck bouncing horizontally
            EnsureMovement();
        }
    }

    void Update()
    {
        // Normalize the ball's speed to maintain constant speed after collisions
        rb.velocity = rb.velocity.normalized * initialSpeed;
    }

    void EnsureMovement()
    {
        Vector2 newVelocity = rb.velocity;
        

        // Ensure minimum vertical speed
        if (Mathf.Abs(newVelocity.y) < minVerticalSpeed)
        {
            float newYVelocity = minVerticalSpeed * Mathf.Sign(newVelocity.y);
            newVelocity.y = newYVelocity;
        }

        // Ensure minimum horizontal speed
        if (Mathf.Abs(newVelocity.x) < minHorizontalSpeed)
        {
            float newXVelocity = minHorizontalSpeed * Mathf.Sign(newVelocity.x);
            newVelocity.x = newXVelocity;
        }

        // Add slight random perturbation to prevent getting stuck
        newVelocity.x += UnityEngine.Random.Range(-0.1f, 0.1f);
        newVelocity.y += UnityEngine.Random.Range(-0.1f, 0.1f);

        rb.velocity = newVelocity.normalized * initialSpeed;
    }

    bool IsTravelingStraightLine()
    {
        // Check if the ball is traveling in a straight horizontal or vertical line
        return Mathf.Abs(rb.velocity.x) < minHorizontalSpeed || Mathf.Abs(rb.velocity.y) < minVerticalSpeed;
    }
}
