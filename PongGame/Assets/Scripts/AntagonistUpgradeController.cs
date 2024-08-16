using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntagonistUpgradeController : MonoBehaviour
{
    private int buildingsDestroyed = 0;
    private Dictionary<string, int> upgradeCounts = new Dictionary<string, int>();

    private List<TurretBehavior> antagonistTurrets = new List<TurretBehavior>(); // List to hold all AntagonistTurret objects
    private List<AntiTurretBehavior> antagonistAntiTurrets = new List<AntiTurretBehavior>(); // List to hold all AntagonistTurret objects
    private List<BarrierBehaviour> antagonistBarriers = new List<BarrierBehaviour>(); // List to hold all AntagonistBarrier objects
    private List<AdversaryBehavior> antagonistPlatform = new List<AdversaryBehavior>(); // List to hold all AntagonistBarrier objects


    void Start()
    {
        upgradeCounts["AntagonistBarrier"] = 0;
        upgradeCounts["AntagonistTurretFireRate"] = 0;
        upgradeCounts["AntagonistAntiTurretFireRate"] = 0;
        upgradeCounts["AntagonistTurretBulletSpeed"] = 0;
        upgradeCounts["AntagonistPlatformSpeed"] = 0;

        // Find all objects with the tag AntagonistTurret and add them to the list
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("antagonistTurret");
        foreach (GameObject turret in turrets)
        {
            TurretBehavior turretComponent = turret.GetComponent<TurretBehavior>();
            if (turretComponent != null)
            {
                antagonistTurrets.Add(turretComponent);
            }
        }

        GameObject[] antiTurrets = GameObject.FindGameObjectsWithTag("antagonistAntiTurret");
        foreach (GameObject antiTurret in antiTurrets)
        {
            AntiTurretBehavior antiTurretComponent = antiTurret.GetComponent<AntiTurretBehavior>();
            if (antiTurretComponent != null)
            {
                antagonistAntiTurrets.Add(antiTurretComponent);
            }
        }

        // Find all objects with the tag AntagonistBarrier and add them to the list
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("AntagonistBarrier");
        foreach (GameObject barrier in barriers)
        {
            BarrierBehaviour barrierComponent = barrier.GetComponent<BarrierBehaviour>();
            if (barrierComponent != null)
            {
                antagonistBarriers.Add(barrierComponent);
            }
        }

        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Antagonist");
        foreach (GameObject platform in platforms)
        {
            AdversaryBehavior platformComponent = platform.GetComponent<AdversaryBehavior>();
            if (platformComponent != null)
            {
                antagonistPlatform.Add(platformComponent);
            }
        }
    }

    public void PlayerBuildingDestroyed()
    {
        //Debug.Log("Player building destroyed! Player can select an upgrade.");
        buildingsDestroyed++;

        if (buildingsDestroyed % 4 == 0)
        {
            AddAdversaryUpgradePoints();
        }
    }

    private void AddAdversaryUpgradePoints()
    {
        //Debug.Log("Adding Points");

        // Create a list of available upgrades that are not maxed out
        List<string> availableUpgrades = new List<string>();

        if (upgradeCounts["AntagonistBarrier"] < 3)
        {
            availableUpgrades.Add("AntagonistBarrier");
        }
        if (upgradeCounts["AntagonistTurretFireRate"] < 3)
        {
            availableUpgrades.Add("AntagonistTurretFireRate");
        }
        if (upgradeCounts["AntagonistAntiTurretFireRate"] < 3)
        {
            availableUpgrades.Add("AntagonistAntiTurretFireRate");
        }
        if (upgradeCounts["AntagonistTurretBulletSpeed"] < 3)
        {
            availableUpgrades.Add("AntagonistTurretBulletSpeed");
        }
        if (upgradeCounts["AntagonistPlatformSpeed"] < 3)
        {
            availableUpgrades.Add("AntagonistPlatformSpeed");
        }

        // If there are no upgrades available, all are maxed out
        if (availableUpgrades.Count == 0)
        {
            Debug.Log("No more available upgrades left.");
            return;
        }

        // Randomly select one of the available upgrades
        string selectedUpgrade = availableUpgrades[Random.Range(0, availableUpgrades.Count)];

        // Apply the selected upgrade
        switch (selectedUpgrade)
        {
            case "AntagonistBarrier":
                ApplyBarrierUpgrade();
                upgradeCounts["AntagonistBarrier"]++;
                break;

            case "AntagonistTurretFireRate":
                ApplyTurretFireRateUpgrade();
                upgradeCounts["AntagonistTurretFireRate"]++;
                break;

            case "AntagonistAntiTurretFireRate":
                ApplyAntiTurretFireRateUpgrade();
                upgradeCounts["AntagonistAntiTurretFireRate"]++;
                break;

            case "AntagonistTurretBulletSpeed":
                ApplyTurretBulletSpeedUpgrade();
                upgradeCounts["AntagonistTurretBulletSpeed"]++;
                break;

            case "AntagonistPlatformSpeed":
                ApplyPlatformUpgrade();
                upgradeCounts["AntagonistPlatformSpeed"]++;
                break;
        }
    }

    private void ApplyTurretFireRateUpgrade()
    {
        Debug.Log("Adding Points to turret fire rate");
        foreach (TurretBehavior turretComponent in antagonistTurrets)
        {
            turretComponent.fireRate = Mathf.Max(1, turretComponent.fireRate - 3); // Ensure fireRate doesn't go below 1
            //Debug.Log($"Turret upgraded: Fire rate decreased by 3 for turret {turretComponent.name}.");
        }
    }

    private void ApplyAntiTurretFireRateUpgrade()
    {
        Debug.Log("Adding Points to anti turret fire rate");
        foreach (AntiTurretBehavior antiTurretComponent in antagonistAntiTurrets)
        {
            antiTurretComponent.fireRate = Mathf.Max(0.1f, antiTurretComponent.fireRate - 0.1f); 
            //Debug.Log($"Turret upgraded: Fire rate decreased by 3 for turret {turretComponent.name}.");
        }
    }

    private void ApplyTurretBulletSpeedUpgrade()
    {
        Debug.Log("Adding Points to turret bullet speed");
        foreach (TurretBehavior turretComponent in antagonistTurrets)
        {
            turretComponent.bulletSpeed = Mathf.Max(1, turretComponent.bulletSpeed + 2);
            //Debug.Log($"Bullet upgraded: Bullet speed increased by 2 for turret {turretComponent.name}.");
        }
    }

    private void ApplyBarrierUpgrade()
    {
        Debug.Log("Adding Points to barrier recharge rate");
        foreach (BarrierBehaviour barrierComponent in antagonistBarriers)
        {
            barrierComponent.respawnTime = Mathf.Max(1, barrierComponent.respawnTime - 4f);
            //Debug.Log($"Barrier upgraded: Respawn time decreased by 3f for barrier {barrierComponent.name}.");
        }
    }

    private void ApplyPlatformUpgrade()
    {
        Debug.Log("Adding Points to platform speed");
        foreach (AdversaryBehavior platformComponent in antagonistPlatform)
        {
            platformComponent.moveSpeed = Mathf.Max(1, platformComponent.moveSpeed + 4f);
        }
    }
}
