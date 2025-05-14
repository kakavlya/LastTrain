using UnityEngine;

public class WeaponAudioEffects : MonoBehaviour
{
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private AudioSource _audioSource;

    private void PlayAudionEffect()
    {
        if (_audioSource != null && _shootSound != null)
            _audioSource.PlayOneShot(_shootSound);
    }
}
