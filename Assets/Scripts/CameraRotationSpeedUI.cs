using UnityEngine;
using UnityEngine.UI;

public class CameraRotationSpeedUI : MonoBehaviour
{
    [SerializeField] private CameraController cameraController; // твой скрипт камеры
    [SerializeField] private Slider rotationSpeedSlider;
    [SerializeField] private Slider porogSlider;

    private void Start()
    {
        if (rotationSpeedSlider != null)
        {
            rotationSpeedSlider.onValueChanged.AddListener(OnSliderChanged);
        }
    }

    private void OnSliderChanged(float value)
    {
      
    }

    private void OnDestroy()
    {
        if (rotationSpeedSlider != null)
            rotationSpeedSlider.onValueChanged.RemoveListener(OnSliderChanged);
    }
}
