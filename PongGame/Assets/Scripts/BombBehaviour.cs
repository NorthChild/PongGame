using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    public float initialSpeed = 10f; // Initial speed of the ball
    public float maxBounceAngle = 75f; // Maximum angle for the ball to bounce off the paddle
    public float minVerticalSpeed = 2f; // Minimum vertical speed to prevent horizontal bouncing
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
        float randomDirection = Random.Range(0, 2) == 0 ? -1 : 1;
        rb.velocity = new Vector2(initialSpeed * randomDirection, initialSpeed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Bounce the ball off any obstacle or walls
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Obstacle")
        {
            // Reflect the ball's velocity based on the collision normal
            Vector2 reflectDir = Vector2.Reflect(rb.velocity, collision.contacts[0].normal);
            rb.velocity = reflectDir;

            // Ensure the ball doesn't get stuck bouncing horizontally
            EnsureVerticalMovement();
        }

        // Bounce the ball off the paddle
        if (collision.gameObject.tag == "Player")
        {
            // Calculate where on the paddle the ball hit
            float hitPoint = (transform.position.x - collision.transform.position.x) / collision.collider.bounds.size.x;
            float bounceAngle = hitPoint * maxBounceAngle;

            // Ensure the ball doesn't slide horizontally by setting a minimum angle
            bounceAngle = Mathf.Clamp(bounceAngle, -maxBounceAngle, maxBounceAngle);

            // Calculate the new velocity based on the bounce angle
            float newVelocityX = initialSpeed * Mathf.Sin(bounceAngle * Mathf.Deg2Rad);
            float newVelocityY = initialSpeed * Mathf.Cos(bounceAngle * Mathf.Deg2Rad);

            // Ensure the ball moves upwards after hitting the paddle
            rb.velocity = new Vector2(newVelocityX, Mathf.Abs(newVelocityY));
        }
    }

    void Update()
    {
        // Normalize the ball's speed to maintain constant speed after collisions
        rb.velocity = rb.velocity.normalized * initialSpeed;
    }

    void EnsureVerticalMovement()
    {
        if (Mathf.Abs(rb.velocity.y) < minVerticalSpeed)
        {
            float newYVelocity = minVerticalSpeed * Mathf.Sign(rb.velocity.y);
            rb.velocity = new Vector2(rb.velocity.x, newYVelocity).normalized * initialSpeed;
        }
    }

}
