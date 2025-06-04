using UnityEngine;

public class WeaponVisualEffects : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private ParticleSystem _muzzleFlash;

    private void OnEnable()
    {
        _weapon.OnFired += PlayMuzzleEffect;
    }

    private void PlayMuzzleEffect()
    {
        if (_muzzleFlash != null)
        {
            _muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _muzzleFlash.Play();
        }
    }
}
