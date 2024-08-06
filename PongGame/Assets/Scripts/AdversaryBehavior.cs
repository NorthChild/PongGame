using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdversaryBehavior : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed at which the paddle moves
    public GameObject leftWall;   // Reference to the left wall
    public GameObject rightWall;  // Reference to the right wall
    public GameObject ball;       // Reference to the ball
    public float moveDistance = 20f; // Distance to move if paddle is stationary
    public float stationaryTime = 2f; // Time threshold to consider paddle stationary

    private Rigidbody2D rb;
    private BoxCollider2D paddleCollider;
    private float leftBoundary;
    private float rightBoundary;
    private Vector3 lastPosition;
    private float stationaryTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // Ensure the paddle is kinematic

        paddleCollider = GetComponent<BoxCollider2D>();

        // Calculate boundaries based on wall positions and paddle size
        leftBoundary = leftWall.transform.position.x + leftWall.GetComponent<BoxCollider2D>().bounds.extents.x + paddleCollider.bounds.extents.x;
        rightBoundary = rightWall.transform.position.x - rightWall.GetComponent<BoxCollider2D>().bounds.extents.x - paddleCollider.bounds.extents.x;

        // Initialize last position
        lastPosition = transform.position;
    }

    void Update()
    {
        // Get the x position of the ball
        float targetX = ball.transform.position.x;

        // Calculate the new position of the paddle
        Vector3 newPosition = Vector3.MoveTowards(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);

        // Clamp the paddle position within the screen boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, leftBoundary, rightBoundary);

        // Update the paddle position
        transform.position = newPosition;

        // Check if the paddle is stationary
        if (transform.position == lastPosition)
        {
            stationaryTimer += Time.deltaTime;
            if (stationaryTimer >= stationaryTime)
            {
                MovePaddle();
                stationaryTimer = 0f; // Reset the timer after moving
            }
        }
        else
        {
            stationaryTimer = 0f; // Reset the timer if the paddle moved
        }

        lastPosition = transform.position; // Update last position
    }

    void MovePaddle()
    {
        //Debug.Log("moved paddle");
        // Move the paddle left or right by moveDistance
        float direction = (Random.value > 0.5f) ? 2f : -2f;
        Vector3 newPosition = transform.position + new Vector3(direction * moveDistance, 0, 0);

        // Clamp the new position within the screen boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, leftBoundary, rightBoundary);

        // Update the paddle position
        transform.position = newPosition;
    }

    void FixedUpdate()
    {
        // Ensure the paddle stays within the boundaries during physics updates
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, leftBoundary, rightBoundary);
        transform.position = clampedPosition;
    }
}
