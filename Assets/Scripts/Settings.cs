using SaveContent;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField]private Slider _volumeSlider;
    
    private float _defaultVolume = 0.5f;

    public void Init()
    {
        float volume = SaveSystem.Data != null ? SaveSystem.Data.volumeSound : _defaultVolume;
        
        if (_volumeSlider != null)
        {
            _volumeSlider.value = volume;
            _volumeSlider.onValueChanged.AddListener(ChangeVolume);
        }
        
        AudioListener.volume = volume;
    }

    private void ChangeVolume(float volume)
    {
        AudioListener.volume = volume;
        
        if (SaveSystem.Data != null)
        {
            SaveSystem.Data.volumeSound = volume;
            SaveSystem.SaveToPlayerPrefs();
        }
    }
}