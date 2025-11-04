
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
            
            rotationSpeedSlider.value = cameraController.RotationSpeed;
            porogSlider.value = cameraController.porog;
           
            rotationSpeedSlider.onValueChanged.AddListener(OnSliderChanged);
            porogSlider.onValueChanged.AddListener(OnPorogChanged);
        }
    }

    private void OnSliderChanged(float value)
    {
        cameraController.RotationSpeed = value;
    }

    private void OnPorogChanged(float value)
    {
        cameraController.porog = value;
    }

    private void OnDestroy()
    {
        if (rotationSpeedSlider != null)
            rotationSpeedSlider.onValueChanged.RemoveListener(OnSliderChanged);
    }
}
