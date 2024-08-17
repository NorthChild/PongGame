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
    private SpriteRenderer previousSpriteRenderer; // Previous sprite renderer to reset material
    public Material outlineMaterial; // Material with outline shader

    // Dictionary to keep track of the next upgrade index for each parent
    private Dictionary<GameObject, int> upgradeIndices = new Dictionary<GameObject, int>();

    // Dictionary to keep track of the number of upgrades for each parent
    private Dictionary<GameObject, int> upgradeCounts = new Dictionary<GameObject, int>();

    public delegate void UpgradeApplied(GameObject parent, int upgradeCount);
    public event UpgradeApplied OnUpgradeApplied;

    private bool isSlowingTime = false;

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
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CycleUpgradeParent();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CycleUpgradeParentBackward();
        }

        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            SpendUpgradePointsAndChangeCapsuleColor();
        }
    }

    private void CycleUpgradeParent()
    {
        // Reset the material of the previous sprite renderer if it exists
        if (previousSpriteRenderer != null)
        {
            playerUpgradeController.RestoreMaterialAfterCycling(previousSpriteRenderer.gameObject);
        }

        // Move to the next parent object in the list
        currentIndex = (currentIndex + 1) % upgradeParents.Count;

        // Get the new current parent object and its sprite renderer
        GameObject currentParent = upgradeParents[currentIndex];
        currentSpriteRenderer = currentParent.GetComponent<SpriteRenderer>();

        if (currentSpriteRenderer != null)
        {
            // Store the original material if not already stored
            if (!playerUpgradeController.HasOriginalMaterial(currentParent))
            {
                playerUpgradeController.StoreOriginalMaterial(currentParent, currentSpriteRenderer.material);
            }

            // Change the sprite to use the outline material
            currentSpriteRenderer.material = outlineMaterial;
        }
        else
        {
            Debug.LogError($"SpriteRenderer not found in {currentParent.name}.");
        }

        // Set previousSpriteRenderer to the current one
        previousSpriteRenderer = currentSpriteRenderer;

        // Slow down time
        if (!isSlowingTime)
        {
            StartCoroutine(SlowTimeCoroutine());
        }
    }

    private void CycleUpgradeParentBackward()
    {
        // Reset the material of the previous sprite renderer if it exists
        if (previousSpriteRenderer != null)
        {
            playerUpgradeController.RestoreMaterialAfterCycling(previousSpriteRenderer.gameObject);
        }

        // Move to the previous parent object in the list
        currentIndex = (currentIndex - 1 + upgradeParents.Count) % upgradeParents.Count;

        // Get the new current parent object and its sprite renderer
        GameObject currentParent = upgradeParents[currentIndex];
        currentSpriteRenderer = currentParent.GetComponent<SpriteRenderer>();

        if (currentSpriteRenderer != null)
        {
            // Store the original material if not already stored
            if (!playerUpgradeController.HasOriginalMaterial(currentParent))
            {
                playerUpgradeController.StoreOriginalMaterial(currentParent, currentSpriteRenderer.material);
            }

            // Change the sprite to use the outline material
            currentSpriteRenderer.material = outlineMaterial;
        }
        else
        {
            Debug.LogError($"SpriteRenderer not found in {currentParent.name}.");
        }

        // Set previousSpriteRenderer to the current one
        previousSpriteRenderer = currentSpriteRenderer;

        // Slow down time
        if (!isSlowingTime)
        {
            StartCoroutine(SlowTimeCoroutine());
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
                return;
            }

            if (playerUpgradeController.SpendUpgradePoints(4)) // Spend 4 points
            {
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

                        // Trigger the OnUpgradeApplied event after upgrading
                        OnUpgradeApplied?.Invoke(currentParent, upgradeCounts[currentParent]);
                    }
                    else
                    {
                        Debug.LogError($"Capsule {childName} not found in {currentParent.name}.");
                    }
                }
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

    private IEnumerator SlowTimeCoroutine()
    {
        isSlowingTime = true;
        Time.timeScale = 0.1f; // Slow down time by 90%
        yield return new WaitForSecondsRealtime(2f); // Wait for 2 seconds (real-time)
        Time.timeScale = 1f; // Restore time to normal
        isSlowingTime = false;
    }
}
