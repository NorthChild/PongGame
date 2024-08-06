using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add this for TextMeshPro

public class UIPointsController : MonoBehaviour
{
    public PlayerUpgradeController playerUpgradeController; // Reference to the PlayerUpgradeController
    public TextMeshProUGUI upgradePointsText; // Reference to the TextMeshProUGUI component

    // Start is called before the first frame update
    void Start()
    {
        upgradePointsText = GetComponent<TextMeshProUGUI>();

        if (playerUpgradeController == null)
        {
            Debug.LogError("PlayerUpgradeController is not assigned.");
        }

        // Initialize the display
        UpdateUpgradePointsDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUpgradePointsDisplay();
    }

    // Method to update the display text
    private void UpdateUpgradePointsDisplay()
    {
        if (playerUpgradeController != null && upgradePointsText != null)
        {
            upgradePointsText.text = playerUpgradeController.TotalUpgradePoints.ToString();
        }
    }
}
