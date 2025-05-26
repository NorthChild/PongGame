using UnityEngine;

public class ForceFullHD : MonoBehaviour
{
    // Use Windowed mode so we get a standard OS window you can resize.
    [SerializeField]
    private FullScreenMode mode = FullScreenMode.Windowed;

    void Awake()
    {
        // 1) Force the window to open at 1920×1080, Windowed
        Screen.SetResolution(1920, 1080, mode);

        // 2) (Optional) lock vsync so you can observe frame-rate changes when you resize
        QualitySettings.vSyncCount = 1;

        // Remove any further enforcement so the user/OS can resize freely
        // (i.e. don’t re-apply Screen.fullScreenMode here)
    }
}
