using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUpgradeController : MonoBehaviour
{
    private int totalUpgradePoints = 0;

    public TextMeshProUGUI upgradePointsText;
    public List<GameObject> upgradeableObjects;
    public Material notificationOutlineMaterial;
    private Dictionary<GameObject, Material> originalMaterials = new Dictionary<GameObject, Material>();
    public UpgradeHandler upgradeHandler;

    public int TotalUpgradePoints
    {
        get { return totalUpgradePoints; }
    }

    public void AntagonistBuildingDestroyed()
    {
        AddUpgradePoints();
    }

    private void AddUpgradePoints()
    {
        totalUpgradePoints += 1;
        UpdateUpgradePointsUI();
        ApplyOrRemoveNotificationMaterial();
    }

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

    public bool SpendUpgradePoints(int points)
    {
        if (totalUpgradePoints >= points)
        {
            totalUpgradePoints -= points;
            UpdateUpgradePointsUI();
            ApplyOrRemoveNotificationMaterial();
            return true;
        }
        return false;
    }

    public void StoreOriginalMaterial(GameObject obj, Material material)
    {
        if (!originalMaterials.ContainsKey(obj))
        {
            originalMaterials[obj] = material;
        }
    }

    public bool HasOriginalMaterial(GameObject obj)
    {
        return originalMaterials.ContainsKey(obj);
    }

    public void RestoreOriginalMaterial(GameObject obj)
    {
        if (originalMaterials.ContainsKey(obj))
        {
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.material = originalMaterials[obj];
            }
        }
    }

    private void ApplyOrRemoveNotificationMaterial()
    {
        if (notificationOutlineMaterial == null) return;

        bool shouldHighlight = totalUpgradePoints >= 4;

        foreach (GameObject obj in upgradeableObjects)
        {
            if (obj != null)
            {
                SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    bool isMaxedOut = upgradeHandler.GetUpgradeCount(obj) >= 3;

                    if (shouldHighlight && !isMaxedOut)
                    {
                        // Store the original material if not already stored
                        if (!originalMaterials.ContainsKey(obj))
                        {
                            originalMaterials[obj] = spriteRenderer.material;
                        }

                        // Apply the NotificationOutlineMaterial if conditions are met
                        spriteRenderer.material = notificationOutlineMaterial;
                    }
                    else
                    {
                        // Restore the original material when not highlighting or if maxed out
                        RestoreOriginalMaterial(obj);
                    }
                }
            }
        }
    }

    public void RestoreMaterialAfterCycling(GameObject obj)
    {
        if (originalMaterials.ContainsKey(obj))
        {
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                if (totalUpgradePoints >= 4 && upgradeHandler.GetUpgradeCount(obj) < 3)
                {
                    spriteRenderer.material = notificationOutlineMaterial;
                }
                else
                {
                    spriteRenderer.material = originalMaterials[obj];
                }
            }
        }
    }

    public void RemoveNotificationOutlineMaterialFromAll()
    {
        foreach (GameObject obj in upgradeableObjects)
        {
            if (obj != null)
            {
                RestoreOriginalMaterial(obj);
            }
        }
    }
}
