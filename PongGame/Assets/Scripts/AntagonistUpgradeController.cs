using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntagonistUpgradeController : MonoBehaviour
{
    public void PlayerBuildingDestroyed()
    {
        Debug.Log("Player building destroyed! Player can select an upgrade.");
        // Implement the logic to show upgrade options to the player
        ShowUpgradeOptions();
    }

    // Method to show upgrade options to the player
    private void ShowUpgradeOptions()
    {
        // Implement the UI or logic to allow the player to select an upgrade
        // This can involve enabling a UI panel with upgrade buttons, etc.
    }
}
