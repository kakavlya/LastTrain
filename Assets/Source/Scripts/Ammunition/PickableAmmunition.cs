using Unity.VisualScripting;
using UnityEngine;

public class PickableAmmunition : MonoBehaviour
{
    [field: SerializeField] public Weapon PrefabTypeOfWeapon { get; private set; }
    [field: SerializeField] public int CountProjectiles { get; private set; }

    private PickableAmmunition _ammoPrefabKey;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Projectile projectile))
        {
            var ammunitionType = PrefabTypeOfWeapon.GetType();

            Ammunition[] ammunitions = projectile.Owner.GetComponentsInChildren<Ammunition>();

            foreach (Ammunition ammunition in ammunitions)
            {
                if (ammunition.WeaponPrefab.GetType() == ammunitionType)
                {
                    ammunition.IncreaseProjectilesCount(CountProjectiles);
                }
            }

            PickableAmmunitionPool.Instance.RealeseAmmunition(this, _ammoPrefabKey);
        }
    }

    public void SetPrefabKey(PickableAmmunition pickableAmmunition) => _ammoPrefabKey = pickableAmmunition;
}
