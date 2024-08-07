using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the UI Text element for player score
    //public TextMeshProUGUI antagonistScoreText; // Reference to the UI Text element for antagonist score

    private int playerBuildingsDestroyed = 0;
    private int antagonistBuildingsDestroyed = 0;

    void Start()
    {
        UpdateScoreUI();
    }

    public void PlayerBuildingDestroyed()
    {
        playerBuildingsDestroyed++;
        UpdateScoreUI();
    }

    public void AntagonistBuildingDestroyed()
    {
        antagonistBuildingsDestroyed++;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Player Buildings Destroyed: " + playerBuildingsDestroyed + " / Antagonist Buildings Destroyed: " + antagonistBuildingsDestroyed;
        //antagonistScoreText.text = "Antagonist Buildings Destroyed: " + antagonistBuildingsDestroyed;
    }
}
