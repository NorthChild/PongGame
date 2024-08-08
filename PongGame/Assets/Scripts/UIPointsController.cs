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
        Debug.Log("test 1, total destruction: " + playerUpgradeController.TotalUpgradePoints);
        Debug.Log("test 2 points to spend: " + Mathf.FloorToInt(playerUpgradeController.TotalUpgradePoints / 4f));
        if (playerUpgradeController != null && upgradePointsText != null)
        {
            // Divide the total upgrade points by 4 and floor the value
            int displayPoints = Mathf.FloorToInt(playerUpgradeController.TotalUpgradePoints / 4f);

            // Assign the calculated value to the upgradePointsText
            upgradePointsText.text = displayPoints.ToString();
        }
    }
}
