using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigationController : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
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
