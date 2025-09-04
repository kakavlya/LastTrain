using UnityEngine;
using YG;

namespace LastTrain.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioSource _effectsAudioSource;
        [SerializeField] private AudioSource _musicAudioSource;

        public float EffectsAudioVolume => _effectsAudioSource.volume;
        public float MusicAudioVolume => _musicAudioSource.volume;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _effectsAudioSource.volume = YG2.saves.EffectsVolume;
            _musicAudioSource.volume = YG2.saves.MusicVolume;
        }

        public void PlayWeaponSound(AudioClip clip, bool loop)
        {
            if (_effectsAudioSource == null || clip == null)
                return;

            _effectsAudioSource.loop = loop;
            _effectsAudioSource.clip = clip;
            _effectsAudioSource.Play();
        }

        public void StopWeaponSound()
        {
            if (_effectsAudioSource != null && _effectsAudioSource.isPlaying)
            {
                _effectsAudioSource.Stop();
                _effectsAudioSource.loop = false;
            }
        }

        public void PlayBackgroundMusic(AudioClip clip)
        {
            if (_musicAudioSource == null || clip == null)
                return;

            _musicAudioSource.clip = clip;
            _musicAudioSource.Play();
        }

        public void ChangeEffectsVolume(float value)
        {
            _effectsAudioSource.volume = value;
            YG2.saves.EffectsVolume = value;
            YG2.SaveProgress();
        }

        public void ChangeMusicVolume(float value)
        {
            _musicAudioSource.volume = value;
            YG2.saves.MusicVolume = value;
            YG2.SaveProgress();
        }
    }
}
