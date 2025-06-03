using UnityEngine;

public class WeaponVisualEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem _muzzleFlash;


    private void PlayMuzzleEffect()
    {
        if (_muzzleFlash != null)
        {
            _muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _muzzleFlash.Play();
        }
    }
}
