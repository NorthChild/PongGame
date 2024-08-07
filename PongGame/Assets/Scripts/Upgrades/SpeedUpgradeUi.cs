using UnityEngine;
using UnityEngine.UI;

public class SpeedUpgradeUi : MonoBehaviour
{
    public PlatformBehavior platformBehavior; // Reference to the PlatformBehavior script
    public Slider speedSlider; // Reference to the UI Slider

    void Start()
    {
        if (speedSlider != null)
        {
            speedSlider.onValueChanged.AddListener(OnSpeedSliderValueChanged);
        }
    }

    void OnSpeedSliderValueChanged(float value)
    {
        if (platformBehavior != null)
        {
            platformBehavior.SetMoveSpeed(value);
        }
    }
}
