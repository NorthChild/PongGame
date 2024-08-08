using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntagonistUpgradeController : MonoBehaviour
{
    private int buildingsDestroyed = 0;
    private Dictionary<string, int> upgradeCounts = new Dictionary<string, int>();

    private List<TurretBehavior> antagonistTurrets = new List<TurretBehavior>(); // List to hold all AntagonistTurret objects
    private List<BarrierBehaviour> antagonistBarriers = new List<BarrierBehaviour>(); // List to hold all AntagonistBarrier objects

    void Start()
    {
        upgradeCounts["AntagonistBarrier"] = 0;
        upgradeCounts["AntagonistTurretFireRate"] = 0;
        upgradeCounts["AntagonistTurretBulletSpeed"] = 0;

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
    }

    public void PlayerBuildingDestroyed()
    {
        Debug.Log("Player building destroyed! Player can select an upgrade.");
        buildingsDestroyed++;

        if (buildingsDestroyed % 4 == 0)
        {
            AddAdversaryUpgradePoints();
        }
    }

    // Method to show upgrade options to the player
    private void AddAdversaryUpgradePoints()
    {
        Debug.Log("Adding Points");
        List<GameObject> possibleTargets = new List<GameObject>(2);
        possibleTargets.AddRange(GameObject.FindGameObjectsWithTag("AntagonistBarrier"));
        possibleTargets.AddRange(GameObject.FindGameObjectsWithTag("antagonistTurret"));

        if (possibleTargets.Count == 0)
        {
            Debug.LogWarning("No AntagonistBarrier or AntagonistTurret objects found.");
            return;
        }

        GameObject selectedObject = possibleTargets[Random.Range(0, possibleTargets.Count)];

        if (selectedObject.CompareTag("AntagonistBarrier"))
        {
            //Debug.Log("Adding Points to barrier");
            if (upgradeCounts["AntagonistBarrier"] < 3)
            {
                ApplyBarrierUpgrade();
                upgradeCounts["AntagonistBarrier"]++;
                //Debug.Log("AntagonistBarrier upgrade applied: Decrease respawn time by 3f.");
            }
            else
            {
                //Debug.Log("AntagonistBarrier upgrades maxed out.");
                ApplyAvailableTurretUpgrade();
            }
        }
        else if (selectedObject.CompareTag("antagonistTurret"))
        {
            //Debug.Log("Adding Points to turret");
            if (upgradeCounts["AntagonistTurretFireRate"] < 3 || upgradeCounts["AntagonistTurretBulletSpeed"] < 3)
            {
                bool upgraded = false;
                int attempts = 0; // Add attempts counter

                while (!upgraded && attempts < 6) // Try a maximum of 6 times
                {
                    int upgradeType = Random.Range(0, 2); // 0 for fireRate, 1 for bullet speed

                    if (upgradeType == 0 && upgradeCounts["AntagonistTurretFireRate"] < 3)
                    {
                        ApplyTurretFireRateUpgrade();
                        upgradeCounts["AntagonistTurretFireRate"]++;
                        upgraded = true;
                        //Debug.Log("AntagonistTurret upgrade applied: Decrease fire rate by 3f.");
                    }
                    else if (upgradeType == 1 && upgradeCounts["AntagonistTurretBulletSpeed"] < 3)
                    {
                        ApplyTurretBulletSpeedUpgrade();
                        upgradeCounts["AntagonistTurretBulletSpeed"]++;
                        upgraded = true;
                        //Debug.Log("AntagonistTurret upgrade applied: Increase bullet speed by 2.");
                    }

                    attempts++;
                }

                if (!upgraded)
                {
                    //Debug.Log("AntagonistTurret upgrades maxed out.");
                    ApplyRemainingTurretUpgrade();
                }
            }
            else
            {
                //Debug.Log("AntagonistTurret upgrades maxed out.");
                ApplyAvailableBarrierUpgrade();
            }
        }

        if (AllUpgradesMaxed())
        {
            Debug.Log("No more available upgrades left.");
        }
    }

    private bool AllUpgradesMaxed()
    {
        return upgradeCounts["AntagonistBarrier"] >= 3 &&
               upgradeCounts["AntagonistTurretFireRate"] >= 3 &&
               upgradeCounts["AntagonistTurretBulletSpeed"] >= 3;
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
            barrierComponent.respawnTime = Mathf.Max(1, barrierComponent.respawnTime - 3f);
            //Debug.Log($"Barrier upgraded: Respawn time decreased by 3f for barrier {barrierComponent.name}.");
        }
    }

    private void ApplyAvailableBarrierUpgrade()
    {
        if (upgradeCounts["AntagonistBarrier"] < 3)
        {
            ApplyBarrierUpgrade();
            upgradeCounts["AntagonistBarrier"]++;
        }
    }

    private void ApplyAvailableTurretUpgrade()
    {
        if (upgradeCounts["AntagonistTurretFireRate"] < 3)
        {
            ApplyTurretFireRateUpgrade();
            upgradeCounts["AntagonistTurretFireRate"]++;
        }
        else if (upgradeCounts["AntagonistTurretBulletSpeed"] < 3)
        {
            ApplyTurretBulletSpeedUpgrade();
            upgradeCounts["AntagonistTurretBulletSpeed"]++;
        }
    }

    private void ApplyRemainingTurretUpgrade()
    {
        if (upgradeCounts["AntagonistTurretFireRate"] < 3)
        {
            ApplyTurretFireRateUpgrade();
            upgradeCounts["AntagonistTurretFireRate"]++;
        }
        else if (upgradeCounts["AntagonistTurretBulletSpeed"] < 3)
        {
            ApplyTurretBulletSpeedUpgrade();
            upgradeCounts["AntagonistTurretBulletSpeed"]++;
        }
    }
}
