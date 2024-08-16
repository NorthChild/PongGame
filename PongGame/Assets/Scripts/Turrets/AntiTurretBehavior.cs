using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiTurretBehavior : MonoBehaviour
{
    public float fireRate; // Time in seconds between shots
    public GameObject bulletPrefab; // Bullet prefab
    public Transform firePoint; // Point from where the bullet will be fired
    public Transform turretBase; // Base of the turret that will rotate
    public float bulletSpeed; // Speed of the bullet

    private float fireTimer = 0f;

    void Update()
    {
        // Find the closest enemy bullet
        GameObject closestBullet = FindClosestBullet();

        if (closestBullet != null)
        {
            //Debug.Log("bullet found");
            // Aim at the closest enemy bullet from the turret base
            Vector3 direction = closestBullet.transform.position - turretBase.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            turretBase.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // Adjust angle to aim the top part

            // Increment the fire timer
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                //Debug.Log("shooting bullet");
                // Shoot at the target bullet
                Shoot(closestBullet);
                fireTimer = 0f; // Reset the fire timer
            }
        }
    }

    GameObject FindClosestBullet()
    {
        string targetTag;

        // Determine the tag of bullets to look for based on the tag of the current turret
        if (gameObject.tag == "playerAntiTurret")
        {
            targetTag = "antagonistBullet";
        }
        else if (gameObject.tag == "antagonistAntiTurret")
        {
            targetTag = "playerBullet";
        }
        else
        {
            // If the turret's tag is neither playerAntiTurret nor antagonistAntiTurret, return null
            Debug.Log("nothing found");
            return null;
        }

        // Find all objects with the target tag
        GameObject[] bullets = GameObject.FindGameObjectsWithTag(targetTag);
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        // Loop through each bullet to find the closest one
        foreach (GameObject bullet in bullets)
        {
            float distance = Vector3.Distance(transform.position, bullet.transform.position);
            if (distance < minDistance)
            {
                closest = bullet;
                minDistance = distance;
            }
        }

        return closest;
    }

    void Shoot(GameObject target)
    {
        //Debug.Log("shooting");
        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Set the shooterTag of the bullet
        AntiTurretBulletBehavior antiTurretBulletBehavior = bullet.GetComponent<AntiTurretBulletBehavior>();
        if (antiTurretBulletBehavior != null)
        {
            antiTurretBulletBehavior.shooterTag = gameObject.tag; // Set the tag of the turret that shot the bullet
        }

        // Calculate the direction to the target bullet
        Vector3 direction = (target.transform.position - firePoint.position).normalized;

        // Set the bullet's velocity
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;
    }
}
