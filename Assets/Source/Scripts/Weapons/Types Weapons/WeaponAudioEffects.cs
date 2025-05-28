using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class WeaponAudioEffects : MonoBehaviour
{
    [SerializeField] private AudioClip _shootSound;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource> ();
    }

    private void PlayAudionEffect()
    {
        if (_audioSource != null && _shootSound != null)
        {
            _audioSource.clip = _shootSound;
            _audioSource.Play();
        }
    }
}
