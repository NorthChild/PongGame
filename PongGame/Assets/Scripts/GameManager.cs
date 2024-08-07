using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winCanvas; // Win Canvas object
    [SerializeField] private GameObject loseCanvas; // Lose Canvas object
    [SerializeField] private GameObject overlayPanel; // Grey overlay panel
    private GameObject[] antagonistBuildings;
    private GameObject[] playerBuildings;

    void Start()
    {
        winCanvas.SetActive(false); // Ensure Win Canvas is hidden at the start
        loseCanvas.SetActive(false); // Ensure Lose Canvas is hidden at the start
        overlayPanel.SetActive(false); // Ensure overlay panel is hidden at the start
    }

    void Update()
    {
        CheckGameStatus();
    }

    private void CheckGameStatus()
    {
        UpdateBuildingLists();
        Debug.Log("Remaining player buildings: " + playerBuildings.Length);
        Debug.Log("Remaining antagonist buildings: " + antagonistBuildings.Length);

        bool allAntagonistBuildingsDestroyed = true;
        foreach (GameObject building in antagonistBuildings)
        {
            if (building != null && building.CompareTag("antagonistBuilding"))
            {
                allAntagonistBuildingsDestroyed = false;
                break;
            }
        }

        bool allPlayerBuildingsDestroyed = true;
        foreach (GameObject building in playerBuildings)
        {
            if (building != null && building.CompareTag("playerBuilding"))
            {
                allPlayerBuildingsDestroyed = false;
                break;
            }
        }

        if (allAntagonistBuildingsDestroyed)
        {
            WinGame();
        }
        else if (allPlayerBuildingsDestroyed)
        {
            LoseGame();
        }
    }

    private void UpdateBuildingLists()
    {
        antagonistBuildings = GameObject.FindGameObjectsWithTag("antagonistBuilding");
        playerBuildings = GameObject.FindGameObjectsWithTag("playerBuilding");
    }

    private void WinGame()
    {
        Debug.Log("You Win!");
        Time.timeScale = 0; // Pause the game
        overlayPanel.SetActive(true); // Show overlay panel
        winCanvas.SetActive(true); // Show Win Canvas
    }

    private void LoseGame()
    {
        Debug.Log("You Lose!");
        Time.timeScale = 0; // Pause the game
        overlayPanel.SetActive(true); // Show overlay panel
        loseCanvas.SetActive(true); // Show Lose Canvas
    }
}
