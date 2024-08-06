using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeAssignment : MonoBehaviour
{
    public UpgradeHandler upgradeHandler; // Reference to the UpgradeHandler
    private List<TurretBehavior> playerTurrets = new List<TurretBehavior>(); // List to hold all playerTurret objects
    private List<BarrierBehaviour> playerBarrier = new List<BarrierBehaviour>(); // List to hold all playerTurret objects

    void Start()
    {
        if (upgradeHandler == null)
        {
            Debug.LogError("UpgradeHandler is not assigned.");
            return;
        }

        // Subscribe to the OnUpgradeApplied event
        upgradeHandler.OnUpgradeApplied += HandleUpgradeApplied;

        // Find all objects with the tag playerTurret and add them to the list
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("playerTurret");
        foreach (GameObject turret in turrets)
        {
            TurretBehavior turretComponent = turret.GetComponent<TurretBehavior>();
            if (turretComponent != null)
            {
                playerTurrets.Add(turretComponent);
            }
        }

        GameObject[] barriers = GameObject.FindGameObjectsWithTag("PlayerBarrier");
        foreach (GameObject barrier in barriers)
        {
            BarrierBehaviour barrierComponent = barrier.GetComponent<BarrierBehaviour>();
            if (barrierComponent != null)
            {
                playerBarrier.Add(barrierComponent);
            }
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        upgradeHandler.OnUpgradeApplied -= HandleUpgradeApplied;
    }

    // Method to handle upgrades when notified
    private void HandleUpgradeApplied(GameObject parent, int upgradeCount)
    {
        Debug.Log(parent.name);
        switch (parent.name)
        {
            case "turretSpeed":
                ApplyTurretUpgrade(upgradeCount);
                break;
            case "barrierRegen":
                ApplyBarrierUpgrade(upgradeCount);
                break;
            // Add more cases for other upgrade types
            default:
                Debug.LogWarning($"No upgrade logic defined for {parent.name}");
                break;
        }
    }

    private void ApplyTurretUpgrade(int upgradeCount)
    {
        foreach (TurretBehavior turretComponent in playerTurrets)
        {
            turretComponent.fireRate = Mathf.Max(1, turretComponent.fireRate - (3)); // Ensure fireRate doesn't go below 1
            Debug.Log($"Turret upgraded: Fire rate decreased by {2} seconds for turret {turretComponent.name}.");
        }
    }

    private void ApplyBarrierUpgrade(int upgradeCount)
    {
        foreach (BarrierBehaviour barrierComponent in playerBarrier)
        {
            barrierComponent.respawnTime = Mathf.Max(1, barrierComponent.respawnTime - (3f)); // Ensure fireRate doesn't go below 1
            Debug.Log($"Turret upgraded: Fire rate decreased by {2} seconds for turret {barrierComponent.name}.");
        }
    }

}
