using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add this for TextMeshPro

public class PlayerUpgradeController : MonoBehaviour
{
    private int totalUpgradePoints = 0;

    // Reference to the TextMeshProUGUI object
    public TextMeshProUGUI upgradePointsText;

    // Property to get the total upgrade points
    public int TotalUpgradePoints
    {
        get { return totalUpgradePoints; }
    }

    // Method to be called when an antagonist building is destroyed
    public void AntagonistBuildingDestroyed()
    {
        //Debug.Log("Antagonist building destroyed! Player can select an upgrade. " + totalUpgradePoints.ToString());
        // Implement the logic to show upgrade options to the player
        AddUpgradePoints();
    }

    // Method to add upgrade points
    private void AddUpgradePoints()
    {
        totalUpgradePoints += 1;
        UpdateUpgradePointsUI();
    }

    // Method to update the UI text
    private void UpdateUpgradePointsUI()
    {
        if (upgradePointsText != null)
        {
            upgradePointsText.text = totalUpgradePoints.ToString();
        }
        else
        {
            Debug.LogError("UpgradePointsText is not assigned in the inspector.");
        }
    }

    // Method to spend upgrade points
    public bool SpendUpgradePoints(int points)
    {
        if (totalUpgradePoints >= points)
        {
            totalUpgradePoints -= points;
            UpdateUpgradePointsUI();
            return true;
        }
        return false;
    }
}
