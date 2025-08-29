using UnityEngine;
using UnityEngine.UI;

namespace LastTrain.Audio
{
    public class VolumeSettings : MonoBehaviour
    {
        [SerializeField] private Slider _effectsVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;

        private void Awake()
        {
            if (AudioManager.Instance != null)
            {
                _effectsVolumeSlider.value = AudioManager.Instance.EffectsAudioVolume;
                _musicVolumeSlider.value = AudioManager.Instance.MusicAudioVolume;
            }

            _effectsVolumeSlider.onValueChanged.AddListener(ChangeWeaponVolume);
            _musicVolumeSlider.onValueChanged.AddListener(ChangeBackgroundVolume);
        }

        private void ChangeWeaponVolume(float volume)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.ChangeEffectsVolume(volume);
            }
        }

        private void ChangeBackgroundVolume(float volume)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.ChangeMusicVolume(volume);
            }
        }
    }
}
