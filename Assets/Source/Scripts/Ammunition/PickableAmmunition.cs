using UnityEngine;

public class PickableAmmunition : MonoBehaviour
{
    [field: SerializeField] public Weapon PrefabTypeOfWeapon { get; private set; }
    [field: SerializeField] public int CountProjectiles { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Projectile projectile))
        {
            var ammunitionType = PrefabTypeOfWeapon.GetType();

            var ownerWeaponType = projectile.Owner.GetComponentInChildren<Weapon>().GetType();

            if (ownerWeaponType == ammunitionType)
            {
                projectile.Owner.GetComponentInChildren<Ammunition>().IncreaseProjectilesCount(CountProjectiles);
            }
        }
    }
}
