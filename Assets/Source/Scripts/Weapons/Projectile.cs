using Assets.Source.Scripts.Enemies;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _lifetime = 3f;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private int _damage = 50;
    private void Start()
    {
        Destroy(gameObject, _lifetime);
        Debug.Log($"Projectile created: {name}");
    }

    public float Getspeed()
    {
        return _speed;
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Projectile hit: {other.name}");
        if(other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
