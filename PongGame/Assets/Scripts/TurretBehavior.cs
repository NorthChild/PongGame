using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehavior : MonoBehaviour
{
    public float fireRate; // Time in seconds between shots
    public GameObject bulletPrefab; // Bullet prefab
    public Transform firePoint; // Point from where the bullet will be fired
    public Transform turretBase; // Base of the turret that will rotate
    public float bulletSpeed; // Speed of the bullet

    private float fireTimer = 0f;

    void Update()
    {
        // Find the closest building
        GameObject closestBuilding = FindClosestBuilding();

        Debug.Log("fire rate: " + fireRate);

        if (closestBuilding != null)
        {
            // Aim at the closest building from the turret base
            Vector3 direction = closestBuilding.transform.position - turretBase.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            turretBase.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // Adjust angle to aim the top part

            // Increment the fire timer
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                // Shoot at the target
                Shoot(closestBuilding);
                fireTimer = 0f; // Reset the fire timer
            }
        }
    }

    GameObject FindClosestBuilding()
    {
        string targetTag;

        // Determine the tag to look for based on the tag of the current turret
        if (gameObject.tag == "playerTurret")
        {
            targetTag = "antagonistBuilding";
        }
        else if (gameObject.tag == "antagonistTurret")
        {
            targetTag = "playerBuilding";
        }
        else
        {
            // If the turret's tag is neither playerTurret nor adversaryTurret, return null
            return null;
        }

        // Find all objects with the target tag
        GameObject[] buildings = GameObject.FindGameObjectsWithTag(targetTag);
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        // Loop through each building to find the closest one
        foreach (GameObject building in buildings)
        {
            float distance = Vector3.Distance(transform.position, building.transform.position);
            if (distance < minDistance)
            {
                closest = building;
                minDistance = distance;
            }
        }

        return closest;
    }

    void Shoot(GameObject target)
    {
        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Set the shooterTag of the bullet
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        if (bulletBehavior != null)
        {
            bulletBehavior.shooterTag = gameObject.tag; // Set the tag of the turret that shot the bullet
        }

        // Calculate the direction to the target

        Vector3 direction = (target.transform.position - firePoint.position).normalized;

        // Set the bullet's velocity
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;
    }
}
