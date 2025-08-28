using UnityEngine;
using DG.Tweening;
using System.Collections;
using LastTrain.Weapons.Types;

namespace LastTrain.AmmunitionSystem
{
    public class PickableAmmunition : MonoBehaviour
    {
        [SerializeField] private int _countProjectiles;
        [field: SerializeField] public Weapon PrefabTypeOfWeapon { get; private set; }

        private PickableAmmunition _ammoPrefabKey;
        private float _distanceCatch = 30f;
        private float _durationMovement = 1f;
        private int _currentProjectilesCount;

        public int CountProjectiles { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Projectile projectile))
            {
                var ammunitionType = PrefabTypeOfWeapon.GetType();
                Ammunition[] ammunitions = projectile.Owner.transform.parent.GetComponentsInChildren<Ammunition>();

                foreach (Ammunition ammunition in ammunitions)
                {
                    if (ammunition.WeaponPrefab.GetType() == ammunitionType)
                    {
                        ammunition.IncreaseProjectilesCount(CountProjectiles);
                    }
                }

                StartCoroutine(DoPickableAnimation(projectile.Owner.transform));
            }
        }

        public void Init(PickableAmmunition pickableAmmunition, float ammoPercent)
        {
            _currentProjectilesCount = (int)(_countProjectiles * ammoPercent / 100f);
            CountProjectiles = _currentProjectilesCount;
            SetPrefabKey(pickableAmmunition);
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
