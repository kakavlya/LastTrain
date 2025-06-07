using UnityEngine;

public class Flamethrower : Weapon
{
    [SerializeField] private ParticleSystem _flameParticle;

    private bool _isDoingAttack;

    protected override void OnWeaponFire()
    {
        if (_isDoingAttack == false)
        {
            _isDoingAttack = true;
            _flameParticle.Play();
        }
    }

    public override void StopFire()
    {
        base.StopFire();

        if (_isDoingAttack)
        {
            _flameParticle.Stop();
            _isDoingAttack = false;
        }
    }

    public override bool GetIsLoopedFireSound() => true;

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Yes");
        if (other.TryGetComponent(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(Damage);
        }
    }
}
