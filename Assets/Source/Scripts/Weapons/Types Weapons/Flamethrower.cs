using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Weapon
{
    [SerializeField] private ParticleSystem _flameParticle;

    private bool _isDoingAttack;

    protected override void OnWeaponFire()
    {
        _isDoingAttack = true;
        _flameParticle.Play();
    }

    public void StopFire()
    {
        if (_isDoingAttack)
        {
            _flameParticle.Stop();
            _isDoingAttack = false;
        }
    }

    private void Update()
    {
        
    }
}
