using UnityEngine;

public class ForceResolution : MonoBehaviour
{
    void Start()
    {
        // Force the resolution to 1920x1080
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);

        // Optionally, you can lock the resolution and prevent further changes
        InvokeRepeating("EnforceResolution", 0.5f, 0.5f); // Reapply every 0.5 seconds
    }

    void EnforceResolution()
    {
        if (Screen.width != 1920 || Screen.height != 1080)
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        }
    }
}
