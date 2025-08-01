using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource _weaponAudioSource;

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
    }

    public void PlayWeaponSound(AudioClip clip, bool loop)
    {
        if (_weaponAudioSource == null || clip == null)
            return;

        if (_weaponAudioSource.isPlaying && _weaponAudioSource.clip == clip && _weaponAudioSource.loop == loop)
            return;

        _weaponAudioSource.loop = loop;
        _weaponAudioSource.clip = clip;
        _weaponAudioSource.Play();
    }

    public void StopWeaponSound()
    {
        if (_weaponAudioSource != null && _weaponAudioSource.isPlaying)
        {
            _weaponAudioSource.Stop();
            _weaponAudioSource.loop = false;
        }
    }
}
