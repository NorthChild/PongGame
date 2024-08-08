using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehavior : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed at which the paddle moves
    public GameObject leftWall;   // Reference to the left wall
    public GameObject rightWall;  // Reference to the right wall
    private Rigidbody2D rb;
    private BoxCollider2D paddleCollider;
    private float leftBoundary;
    private float rightBoundary;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // Ensure the paddle is kinematic

        paddleCollider = GetComponent<BoxCollider2D>();

        // Calculate boundaries based on wall positions and paddle size
        leftBoundary = leftWall.transform.position.x + leftWall.GetComponent<EdgeCollider2D>().bounds.extents.x + paddleCollider.bounds.extents.x;
        rightBoundary = rightWall.transform.position.x - rightWall.GetComponent<EdgeCollider2D>().bounds.extents.x - paddleCollider.bounds.extents.x;
    }

    void Update()
    {
        // Get input from the horizontal axis (left and right arrow keys or A/D keys)
        float moveInput = Input.GetAxis("Horizontal");

        // Calculate the new position of the paddle
        Vector3 newPosition = transform.position + Vector3.right * moveInput * moveSpeed * Time.deltaTime;

        // Clamp the paddle position within the screen boundaries
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

    // Method to update the paddle's speed
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}
