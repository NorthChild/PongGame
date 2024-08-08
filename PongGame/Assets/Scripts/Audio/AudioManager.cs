using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioClip mainMenuMusic; // Reference to the main menu music clip
    public AudioClip singlePlayMusic; // Reference to the single play music clip

    private AudioSource audioSource;

    void Awake()
    {
        // Ensure this GameObject is not destroyed when loading a new scene
        //DontDestroyOnLoad(gameObject);

        // Add an AudioSource component if it doesn't already exist
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the AudioSource to loop the clip
        audioSource.loop = true;

        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        // Play the appropriate music for the current scene
        PlayMusicForCurrentScene();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Play the appropriate music for the newly loaded scene
        PlayMusicForCurrentScene();
    }

    void PlayMusicForCurrentScene()
    {
        // Check the name of the current scene
        string sceneName = SceneManager.GetActiveScene().name;

        // Stop the current music
        audioSource.Stop();

        // Play the appropriate music based on the scene name
        if (sceneName == "MainMenu")
        {
            audioSource.clip = mainMenuMusic;
        }
        else if (sceneName == "SinglePlay")
        {
            audioSource.clip = singlePlayMusic;
        }

        // Play the selected music clip
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event when this GameObject is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
