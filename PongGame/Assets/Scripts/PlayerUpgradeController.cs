using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeController : MonoBehaviour
{
    // Method to be called when an antagonist building is destroyed
    public void AntagonistBuildingDestroyed()
    {
        Debug.Log("Antagonist building destroyed! Player can select an upgrade.");
        // Implement the logic to show upgrade options to the player
        ShowUpgradeOptions();
    }

    // Method to show upgrade options to the player
    private void ShowUpgradeOptions()
    {
        

    }
}
