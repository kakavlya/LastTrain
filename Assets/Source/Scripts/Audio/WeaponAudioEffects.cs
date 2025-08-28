using UnityEngine;

namespace LastTrain.Audio
{
    public class WeaponAudioEffects : MonoBehaviour
    {
        [SerializeField] private AudioClip _shootSound;
        [SerializeField] private Weapon _weapon;

        private bool _isPlayingLoop;

        private void OnEnable()
        {
            _weapon.OnFired += PlayAudioEffect;
            _weapon.OnStopFired += StopAudioEffect;
        }

        private void OnDisable()
        {
            _weapon.OnFired -= PlayAudioEffect;
            _weapon.OnStopFired -= StopAudioEffect;
        }

        private void OnDestroy()
        {
            AudioManager.Instance.StopWeaponSound();
        }

        private void PlayAudioEffect()
        {
            if (_shootSound == null)
                return;

            if (_weapon.GetIsLoopedFireSound())
            {
                if (!_isPlayingLoop)
                {
                    _isPlayingLoop = true;
                    AudioManager.Instance.PlayWeaponSound(_shootSound, true);
                }
            }
            else
            {
                AudioManager.Instance.PlayWeaponSound(_shootSound, false);
            }
        }

        private void StopAudioEffect()
        {
            if (_shootSound != null && _isPlayingLoop)
            {
                _isPlayingLoop = false;
                AudioManager.Instance.StopWeaponSound();
            }
        }
    }
}