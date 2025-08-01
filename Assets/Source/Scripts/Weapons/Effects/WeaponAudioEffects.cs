using System.Collections;
using UnityEngine;

public class WeaponAudioEffects : MonoBehaviour
{
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private Weapon _weapon;

    private bool _isPlaying;
    private float _trackDelay = 0.1f;
    private float _clipLength;

    private void OnEnable()
    {
        _weapon.OnFired += PlayAudioEffect;
        _weapon.OnStopFired += StopAudioEffect;
        _clipLength = _shootSound.length;
    }

    private void OnDisable()
    {
        _weapon.OnFired -= PlayAudioEffect;
        _weapon.OnStopFired -= StopAudioEffect;
    }

    private void PlayAudioEffect()
    {
        if (_shootSound == null && _isPlaying)
            return;

        if (_weapon.GetIsLoopedFireSound())
        {
            _isPlaying = true;
            AudioManager.Instance.PlayWeaponSound(_shootSound, true);
        }
        else
        {
            AudioManager.Instance.PlayWeaponSound(_shootSound, false);
        }
    }

    private void StopAudioEffect()
    {
        if (_shootSound != null && _isPlaying == true)
        {
            _isPlaying = false;
            AudioManager.Instance.StopWeaponSound();
        }
    }
}
