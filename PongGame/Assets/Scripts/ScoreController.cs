using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the UI Text element for player score
    public Slider playerStatusBar; // Reference to the UI Slider element for the player status bar
    public Slider antagonistStatusBar; // Reference to the UI Slider element for the antagonist status bar
    public int totalPlayerBuildings = 5; // Total number of player buildings at the start
    public int totalAntagonistBuildings = 5; // Total number of antagonist buildings at the start

    private int playerBuildingsDestroyed = 0;
    private int antagonistBuildingsDestroyed = 0;
    private Image playerFillImage;
    private Image antagonistFillImage;

    void Start()
    {
        playerStatusBar.maxValue = totalPlayerBuildings;
        playerStatusBar.value = totalPlayerBuildings;
        antagonistStatusBar.maxValue = totalAntagonistBuildings;
        antagonistStatusBar.value = totalAntagonistBuildings;

        // Get the Image component of the Fill Area
        playerFillImage = playerStatusBar.fillRect.GetComponent<Image>();
        antagonistFillImage = antagonistStatusBar.fillRect.GetComponent<Image>();

        UpdateScoreUI();
    }

    public void PlayerBuildingDestroyed()
    {
        playerBuildingsDestroyed++;
        UpdatePlayerStatusBar();
        UpdateScoreUI();
    }

    public void AntagonistBuildingDestroyed()
    {
        antagonistBuildingsDestroyed++;
        UpdateAntagonistStatusBar();
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Player Buildings Destroyed: " + playerBuildingsDestroyed + " / Antagonist Buildings Destroyed: " + antagonistBuildingsDestroyed;
    }

    void UpdatePlayerStatusBar()
    {
        float percentage = (totalPlayerBuildings - playerBuildingsDestroyed) / (float)totalPlayerBuildings;
        playerStatusBar.value = totalPlayerBuildings - playerBuildingsDestroyed;

        if (percentage <= 0.25f)
        {
            playerFillImage.color = Color.red;
        }
        else if (percentage <= 0.5f)
        {
            playerFillImage.color = Color.Lerp(Color.red, Color.yellow, percentage / 0.25f);
        }
        else
        {
            playerFillImage.color = Color.green;
        }

        //Debug.Log("Player Status Bar Updated: " + playerStatusBar.value);
    }

    void UpdateAntagonistStatusBar()
    {
        float percentage = (totalAntagonistBuildings - antagonistBuildingsDestroyed) / (float)totalAntagonistBuildings;
        antagonistStatusBar.value = totalAntagonistBuildings - antagonistBuildingsDestroyed;

        if (percentage <= 0.25f)
        {
            antagonistFillImage.color = Color.red;
        }
        else if (percentage <= 0.5f)
        {
            antagonistFillImage.color = Color.Lerp(Color.red, Color.yellow, percentage / 0.25f);
        }
        else
        {
            antagonistFillImage.color = Color.green;
        }

        //Debug.Log("Antagonist Status Bar Updated: " + antagonistStatusBar.value);
    }
}
