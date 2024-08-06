using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeHandler : MonoBehaviour
{
    public PlayerUpgradeController playerUpgradeController; // Reference to the PlayerUpgradeController

    // List of parent objects to cycle through
    public List<GameObject> upgradeParents;

    private int currentIndex = -1; // Index of the current parent object
    private SpriteRenderer currentSpriteRenderer; // Current sprite renderer
    private Color originalColor; // To store the original color

    // Dictionary to keep track of the next upgrade index for each parent
    private Dictionary<GameObject, int> upgradeIndices = new Dictionary<GameObject, int>();

    // Dictionary to keep track of the number of upgrades for each parent
    private Dictionary<GameObject, int> upgradeCounts = new Dictionary<GameObject, int>();

    // Event to notify when an upgrade is applied
    public delegate void UpgradeApplied(GameObject parent, int upgradeCount);
    public event UpgradeApplied OnUpgradeApplied;

    void Start()
    {
        if (playerUpgradeController == null)
        {
            Debug.LogError("PlayerUpgradeController is not assigned.");
        }

        if (upgradeParents == null || upgradeParents.Count == 0)
        {
            Debug.LogError("No upgrade parent objects assigned.");
            return;
        }

        // Initialize the upgrade index and counts for each parent
        foreach (var parent in upgradeParents)
        {
            upgradeIndices[parent] = 0;
            upgradeCounts[parent] = 0;
        }
    }

    void Update()
    {
        // Check for 'L' key press to cycle forward
        if (Input.GetKeyDown(KeyCode.L))
        {
            CycleUpgradeParent();
        }

        // Check for 'K' key press to cycle backward
        if (Input.GetKeyDown(KeyCode.K))
        {
            CycleUpgradeParentBackward();
        }

        // Check for 'Enter' key press to spend upgrade points and change capsule color
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SpendUpgradePointsAndChangeCapsuleColor();
        }
    }

    private void CycleUpgradeParent()
    {
        // Reset the color of the current sprite renderer if it exists
        if (currentSpriteRenderer != null)
        {
            currentSpriteRenderer.color = originalColor;
        }

        // Move to the next parent object in the list
        currentIndex = (currentIndex + 1) % upgradeParents.Count;

        // Get the new current parent object and its sprite renderer
        GameObject currentParent = upgradeParents[currentIndex];
        currentSpriteRenderer = currentParent.GetComponent<SpriteRenderer>();

        if (currentSpriteRenderer != null)
        {
            // Store the original color
            originalColor = currentSpriteRenderer.color;

            // Change the sprite color to C19191 to highlight the current parent object
            currentSpriteRenderer.color = new Color(0.756f, 0.569f, 0.569f); // RGB values for #C19191
        }
        else
        {
            Debug.LogError($"SpriteRenderer not found in {currentParent.name}.");
        }
    }

    private void CycleUpgradeParentBackward()
    {
        // Reset the color of the current sprite renderer if it exists
        if (currentSpriteRenderer != null)
        {
            currentSpriteRenderer.color = originalColor;
        }

        // Move to the previous parent object in the list
        currentIndex = (currentIndex - 1 + upgradeParents.Count) % upgradeParents.Count;

        // Get the new current parent object and its sprite renderer
        GameObject currentParent = upgradeParents[currentIndex];
        currentSpriteRenderer = currentParent.GetComponent<SpriteRenderer>();

        if (currentSpriteRenderer != null)
        {
            // Store the original color
            originalColor = currentSpriteRenderer.color;

            // Change the sprite color to C19191 to highlight the current parent object
            currentSpriteRenderer.color = new Color(0.756f, 0.569f, 0.569f); // RGB values for #C19191
        }
        else
        {
            Debug.LogError($"SpriteRenderer not found in {currentParent.name}.");
        }
    }

    private void SpendUpgradePointsAndChangeCapsuleColor()
    {
        if (currentSpriteRenderer != null)
        {
            // Get the current parent object
            GameObject currentParent = upgradeParents[currentIndex];

            // Check if the parent object has already been upgraded 3 times
            if (upgradeCounts[currentParent] >= 3)
            {
                Debug.Log($"No more upgrades allowed for {currentParent.name}.");
                return;
            }

            if (playerUpgradeController.SpendUpgradePoints(2)) // Spend 2 points
            {
                Debug.Log("2 upgrade points spent.");

                // Find the next child capsule to upgrade
                int nextUpgradeIndex = upgradeIndices[currentParent];
                if (nextUpgradeIndex < 3)
                {
                    string childName = "upgrade" + (nextUpgradeIndex + 1);
                    SpriteRenderer capsuleRenderer = currentParent.transform.Find(childName).GetComponent<SpriteRenderer>();

                    if (capsuleRenderer != null)
                    {
                        capsuleRenderer.color = Color.red;
                        upgradeIndices[currentParent] = nextUpgradeIndex + 1;
                        upgradeCounts[currentParent]++;

                        // Notify that an upgrade has been applied
                        OnUpgradeApplied?.Invoke(currentParent, upgradeCounts[currentParent]);
                    }
                    else
                    {
                        Debug.LogError($"Capsule {childName} not found in {currentParent.name}.");
                    }
                }
                else
                {
                    Debug.Log("All capsules are already upgraded for this parent object.");
                }
            }
            else
            {
                Debug.Log("Not enough upgrade points available.");
            }
        }
        else
        {
            Debug.LogWarning("No current sprite renderer to upgrade.");
        }
    }

    public int GetUpgradeCount(GameObject parent)
    {
        if (upgradeCounts.ContainsKey(parent))
        {
            return upgradeCounts[parent];
        }
        return 0;
    }
}
