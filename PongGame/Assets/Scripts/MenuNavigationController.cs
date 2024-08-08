using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigationController : MonoBehaviour
{
    public AudioClip selectionSound;

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSinglePlayScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        // If running in the Unity editor, stop playing the game
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
