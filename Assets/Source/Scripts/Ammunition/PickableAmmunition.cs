using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class PickableAmmunition : MonoBehaviour
{
    [field: SerializeField] public Weapon PrefabTypeOfWeapon { get; private set; }
    [field: SerializeField] public int CountProjectiles { get; private set; }

    private PickableAmmunition _ammoPrefabKey;
    private float _distanceCatch = 30f;
    private float _durationMovement = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Projectile projectile))
        {
            var ammunitionType = PrefabTypeOfWeapon.GetType();

            Ammunition[] ammunitions = projectile.Owner.transform.parent.GetComponentsInChildren<Ammunition>();
            Debug.Log(projectile.Owner.name);

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

    public void SetPrefabKey(PickableAmmunition pickableAmmunition) => _ammoPrefabKey = pickableAmmunition;

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
