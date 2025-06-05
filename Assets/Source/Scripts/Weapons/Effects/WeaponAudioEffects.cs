using System.Collections;
using Assets.Source.Scripts.Weapons;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponAudioEffects : MonoBehaviour
{
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private float _volume = 0.5f;

    private AudioSource _audioSource;
    private bool _isPlaying;
    private float _trackDelay = 0.1f;
    private float _clipLength;
    private IEventTrigger _eventTrigger;

    private void OnEnable()
    {
        if (_weapon is IEventTrigger)
        {
            _eventTrigger = _weapon as IEventTrigger;
            _eventTrigger.OnTriggered += PlayAudioEffect;
        }
        else
        {
            Debug.LogError("MonoBehaviour object is not an IEventTrigger, its type is" +
                _eventTrigger.GetType());
        }

        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = _volume;
        _audioSource.clip = _shootSound;
        _weapon.OnStopFired += StopAudioEffect;
        _clipLength = _audioSource.clip.length;
    }

    private void OnDisable()
    {
        if (_eventTrigger != null)
        {
            _eventTrigger.OnTriggered -= PlayAudioEffect;
        }

        _weapon.OnStopFired -= StopAudioEffect;
    }

    private void PlayAudioEffect()
    {
        if (_audioSource != null && _shootSound != null && _isPlaying == false)
        {
            if (_weapon.GetIsLoopedFireSound())
            {
                _isPlaying = true;
                _audioSource.Play();
                StartCoroutine(PlayNextLoop());
            }
            else
            {
                _audioSource.PlayOneShot(_shootSound);
            }
        }
    }

    private IEnumerator PlayNextLoop()
    {
        _clipLength = _audioSource.clip.length;
        yield return new WaitForSeconds(_clipLength - _trackDelay);

        if (_isPlaying == true)
        {
            _audioSource.Play();
            StartCoroutine(PlayNextLoop());
        }
    }

    private void StopAudioEffect()
    {
        if (_audioSource != null && _shootSound != null && _isPlaying == true)
        {
            _isPlaying = false;
            _audioSource.Stop();
        }
    }
}
