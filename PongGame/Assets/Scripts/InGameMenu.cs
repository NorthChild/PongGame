using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject inGameMenuUI; // UI panel for in-game menu
    [SerializeField] private GameObject overlayPanel; // Grey overlay panel
    [SerializeField] private GameObject optionsMenuUI; // Options menu UI panel
    public AudioClip selectionSound;

    private void Start()
    {
        //Debug.Log("InGameMenu: Start - Setting Time.timeScale to 1");
        Time.timeScale = 1; // Ensure the game starts unpaused
        if (inGameMenuUI != null)
        {
            inGameMenuUI.SetActive(false); // Ensure the in-game menu UI is hidden at the start
        }
        if (overlayPanel != null)
        {
            overlayPanel.SetActive(false); // Ensure the overlay panel is hidden at the start
        }
        if (optionsMenuUI != null)
        {
            optionsMenuUI.SetActive(false); // Ensure the options menu UI is hidden at the start
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleInGameMenu();
        }
    }

    private void ToggleInGameMenu()
    {
        if (inGameMenuUI != null && overlayPanel != null)
        {
            // Play the destruction sound
            if (selectionSound != null)
            {
                // Create a temporary GameObject to play the sound
                GameObject soundObject = new GameObject("SelectionSound");
                AudioSource audioSource = soundObject.AddComponent<AudioSource>();
                audioSource.clip = selectionSound;
                audioSource.Play();

                // Destroy the soundObject after the sound has finished playing
                Destroy(soundObject, selectionSound.length);
            }

            bool isActive = inGameMenuUI.activeSelf;
            inGameMenuUI.SetActive(!isActive);
            overlayPanel.SetActive(!isActive);
            Time.timeScale = isActive ? 1 : 0; // Pause the game when menu is active
            //Debug.Log("InGameMenu: ToggleInGameMenu - Time.timeScale set to " + Time.timeScale);
        }
    }

    public void GoToOptions()
    {
        if (inGameMenuUI != null && optionsMenuUI != null)
        {
            // Play the destruction sound
            if (selectionSound != null)
            {
                // Create a temporary GameObject to play the sound
                GameObject soundObject = new GameObject("SelectionSound");
                AudioSource audioSource = soundObject.AddComponent<AudioSource>();
                audioSource.clip = selectionSound;
                audioSource.Play();

                // Destroy the soundObject after the sound has finished playing
                Destroy(soundObject, selectionSound.length);
            }

            inGameMenuUI.SetActive(false); // Disable the current in-game menu UI
            optionsMenuUI.SetActive(true); // Enable the options menu UI
            Time.timeScale = 0; // Pause the game when options menu is active
            //Debug.Log("InGameMenu: GoToOptions - Time.timeScale set to " + Time.timeScale);
        }
    }

    public void GoToMainMenu(string sceneName)
    {

        //Debug.Log("InGameMenu: GoToMainMenu - Setting Time.timeScale to 1");
        Time.timeScale = 1; // Ensure the game is not paused when loading a new scene
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {

        //Debug.Log("InGameMenu: QuitGame - Quitting the game.");
        Application.Quit();
#if UNITY_EDITOR
        // If running in the Unity editor, stop playing the game
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}


