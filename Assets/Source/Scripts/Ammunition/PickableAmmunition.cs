using UnityEngine;
using DG.Tweening;
using System.Collections;
using LastTrain.Weapons.Types;
using LastTrain.Projectiles.Types;

namespace LastTrain.AmmunitionSystem
{
    public class PickableAmmunition : MonoBehaviour
    {
        private const float _maxPercent = 100f;

        [SerializeField] private int _countProjectiles;
        [field: SerializeField] public Weapon PrefabTypeOfWeapon { get; private set; }

        private PickableAmmunition _ammoPrefabKey;
        private float _distanceCatch = 30f;
        private float _durationMovement = 1f;
        private int _currentProjectilesCount;
        private Collider _collider;

        public int CountProjectiles { get; private set; }

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Projectile projectile))
            {
                Collect(projectile);
            }
        }

        public void Init(PickableAmmunition pickableAmmunition, float ammoPercent)
        {
            _collider.enabled = true;
            _currentProjectilesCount = (int)(_countProjectiles * ammoPercent / _maxPercent);
            CountProjectiles = _currentProjectilesCount;
            SetPrefabKey(pickableAmmunition);
        }

        public void Collect(Transform target)
        {
            var ammunitionType = PrefabTypeOfWeapon.GetType();
            Ammunition[] ammunitions = target.transform.GetComponentsInChildren<Ammunition>();
            _collider.enabled = false;

            foreach (Ammunition ammunition in ammunitions)
            {
                if (ammunition.WeaponPrefab.GetType() == ammunitionType)
                {
                    ammunition.IncreaseProjectilesCount(CountProjectiles);
                }
            }

            StartCoroutine(DoPickableAnimation(target.transform));
        }

        private void Collect(Projectile projectile)
        {
            var ammunitionType = PrefabTypeOfWeapon.GetType();
            Ammunition[] ammunitions = projectile.Owner.transform.parent.GetComponentsInChildren<Ammunition>();
            _collider.enabled = false;

            foreach (Ammunition ammunition in ammunitions)
            {
                if (ammunition.WeaponPrefab.GetType() == ammunitionType)
                {
                    ammunition.IncreaseProjectilesCount(CountProjectiles);
                }
            }

            StartCoroutine(DoPickableAnimation(projectile.Owner.transform));
        }

        private void SetPrefabKey(PickableAmmunition pickableAmmunition)
        {
            _ammoPrefabKey = pickableAmmunition;
        }

        private IEnumerator DoPickableAnimation(Transform owner)
        {
            while (Vector3.Distance(transform.position, owner.position) > _distanceCatch)
            {
                transform.DOMove(owner.position, _durationMovement);
                yield return null;
            }

            PickableAmmunitionPool.Instance.RealeseAmmunition(this, _ammoPrefabKey);
        }
    }
}
