using System.Collections;
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
        float randomAngle = Random.Range(-45f, 45f);
        Vector2 direction = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad)).normalized;
        rb.velocity = direction * initialSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            HandleWallCollision(collision);
        }
        else if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Antagonist") || collision.gameObject.CompareTag("PlayerBarrier") || collision.gameObject.CompareTag("AntagonistBarrier"))
        {
            HandlePaddleCollision(collision);
        }
        else if (collision.gameObject.CompareTag("playerBuilding") || collision.gameObject.CompareTag("antagonistBuilding"))
        {
            HandleBuildingCollision(collision);
        }

        // Debugging information to monitor the velocity after collisions
        //Debug.Log("Collision Detected with: " + collision.gameObject.name);
        //Debug.Log("Velocity after collision: " + rb.velocity);
    }

    void HandleWallCollision(Collision2D collision)
    {
        if (bounceSound != null)
        {
            PlaySound(bounceSound);
        }

        // Reflect the ball's velocity based on the collision normal
        Vector2 reflectDir = Vector2.Reflect(rb.velocity, collision.contacts[0].normal);
        rb.velocity = reflectDir;

        // Apply wall bounce force if the ball is traveling in a straight line
        if (IsTravelingStraightLine())
        {
            Vector2 bounceDirection = collision.contacts[0].normal;
            rb.AddForce(bounceDirection * wallBounceForce, ForceMode2D.Force);
        }

        // Ensure the ball doesn't get stuck bouncing horizontally
        EnsureMovement();
    }

    void HandlePaddleCollision(Collision2D collision)
    {
        if (bounceSound != null)
        {
            PlaySound(bounceSound);
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

    void HandleBuildingCollision(Collision2D collision)
    {
        if (bounceSound != null)
        {
            PlaySound(bounceSound);
        }

        // Reflect the ball's velocity based on the collision normal
        Vector2 reflectDir = Vector2.Reflect(rb.velocity, collision.contacts[0].normal);
        rb.velocity = reflectDir;

        // Ensure the ball doesn't get stuck bouncing horizontally
        EnsureMovement();
    }

    void Update()
    {
        // Normalize the ball's speed to maintain constant speed after collisions
        if (rb.velocity.magnitude < initialSpeed)
        {
            rb.velocity = rb.velocity.normalized * initialSpeed;
            //Debug.Log("Velocity normalized to maintain speed.");
        }

        // Prevent the ball from stopping or having very low velocities
        PreventStuckBall();
    }

    void EnsureMovement()
    {
        Vector2 newVelocity = rb.velocity;

        // Ensure minimum vertical speed
        if (Mathf.Abs(newVelocity.y) < minVerticalSpeed)
        {
            newVelocity.y = minVerticalSpeed * Mathf.Sign(newVelocity.y);
        }

        // Ensure minimum horizontal speed
        if (Mathf.Abs(newVelocity.x) < minHorizontalSpeed)
        {
            newVelocity.x = minHorizontalSpeed * Mathf.Sign(newVelocity.x);
        }

        // Add slight random perturbation to prevent getting stuck
        newVelocity.x += Random.Range(-0.1f, 0.1f);
        newVelocity.y += Random.Range(-0.1f, 0.1f);

        rb.velocity = newVelocity.normalized * initialSpeed;
    }

    void PreventStuckBall()
    {
        // If the velocity is below a very low threshold, re-launch the ball
        if (rb.velocity.magnitude < 0.1f)
        {
            Debug.LogWarning("Ball velocity is very low, re-launching...");
            LaunchBall();
        }
    }

    bool IsTravelingStraightLine()
    {
        // Check if the ball is traveling in a straight horizontal or vertical line
        return Mathf.Abs(rb.velocity.x) < minHorizontalSpeed || Mathf.Abs(rb.velocity.y) < minVerticalSpeed;
    }

    void PlaySound(AudioClip clip)
    {
        // Create a temporary GameObject to play the sound
        GameObject soundObject = new GameObject("BounceSound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        // Destroy the soundObject after the sound has finished playing
        Destroy(soundObject, clip.length);
    }
}
