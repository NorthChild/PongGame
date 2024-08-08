using UnityEngine;

public class AdversaryBehavior : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed at which the paddle moves
    public GameObject leftWall;   // Reference to the left wall
    public GameObject rightWall;  // Reference to the right wall
    public GameObject bomb;       // Reference to the Bomb object

    private Rigidbody2D rb;
    private EdgeCollider2D paddleCollider;
    private float leftBoundary;
    private float rightBoundary;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // Ensure the paddle is kinematic

        paddleCollider = GetComponent<EdgeCollider2D>();

        // Calculate boundaries based on wall positions and paddle size
        leftBoundary = leftWall.transform.position.x + leftWall.GetComponent<EdgeCollider2D>().bounds.extents.x + paddleCollider.bounds.extents.x;
        rightBoundary = rightWall.transform.position.x - rightWall.GetComponent<EdgeCollider2D>().bounds.extents.x - paddleCollider.bounds.extents.x;
    }

    void Update()
    {
        // Get the x position of the bomb
        float targetX = bomb.transform.position.x;

        // Calculate the new position of the paddle
        Vector3 newPosition = Vector3.MoveTowards(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);

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
}
