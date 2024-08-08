using UnityEngine;

public class DisableAdversaryCollider : MonoBehaviour
{
    public GameObject adversary; // Reference to the adversary
    public GameObject bomb;      // Reference to the Bomb object

    private EdgeCollider2D adversaryCollider;

    void Start()
    {
        if (adversary == null)
        {
            //Debug.LogError("Adversary is not assigned.");
            return;
        }

        if (bomb == null)
        {
            //Debug.LogError("Bomb is not assigned.");
            return;
        }

        adversaryCollider = adversary.GetComponent<EdgeCollider2D>();

        if (adversaryCollider == null)
        {
            //Debug.LogError("Adversary does not have an EdgeCollider2D component.");
        }

        BoxCollider2D triggerCollider = GetComponent<BoxCollider2D>();
        if (triggerCollider == null)
        {
            //Debug.LogError("This game object does not have a BoxCollider2D component.");
            return;
        }

        triggerCollider.isTrigger = true; // Ensure the trigger collider is set as a trigger
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == bomb)
        {
            //Debug.Log("Bomb entered the trigger area.");
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject == bomb)
        {
            //Debug.Log("Bomb is within the trigger area.");
            Rigidbody2D bombRb = bomb.GetComponent<Rigidbody2D>();
            if (bombRb != null)
            {
                //Debug.Log("Bomb Y velocity: " + bombRb.velocity.y);
                if (bombRb.velocity.y < 0)
                {
                    //Debug.LogWarning("Disabling adversary collider.");
                    adversaryCollider.enabled = false; // Disable the collider if the bomb is moving downwards
                }
                else
                {
                    //Debug.LogWarning("Enabling adversary collider.");
                    adversaryCollider.enabled = true; // Enable the collider if the bomb is not moving downwards
                }
            }
            else
            {
                //Debug.LogError("Rigidbody2D component not found on the bomb.");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == bomb)
        {
            //Debug.Log("Bomb exited the trigger area.");
            adversaryCollider.enabled = true; // Enable the collider when the bomb exits the trigger
        }
    }
}
