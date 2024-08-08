using UnityEngine;

public class IconController : MonoBehaviour
{
    public float idleTime = 5f; // Time in seconds before the cursor hides

    private float lastMouseMovementTime;
    private Vector3 lastMousePosition;

    void Start()
    {
        lastMouseMovementTime = Time.time;
        lastMousePosition = Input.mousePosition;
    }

    void Update()
    {
        // Check if the mouse has moved
        if (Input.mousePosition != lastMousePosition)
        {
            lastMouseMovementTime = Time.time;
            lastMousePosition = Input.mousePosition;

            // Ensure the cursor is visible when the mouse moves
            if (!Cursor.visible)
            {
                Cursor.visible = true;
            }
        }

        // Check if the idle time has been exceeded
        if (Time.time - lastMouseMovementTime >= idleTime)
        {
            // Hide the cursor
            Cursor.visible = false;
        }
    }
}
