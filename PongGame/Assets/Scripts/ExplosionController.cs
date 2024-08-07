using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public float lifetime = 0.30f; // Duration of the explosion animation

    void Start()
    {
        // Destroy the explosion game object after the specified lifetime
        Destroy(gameObject, lifetime);
    }
}
