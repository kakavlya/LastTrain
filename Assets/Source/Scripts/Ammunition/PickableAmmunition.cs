using Unity.VisualScripting;
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

            Debug.Log(projectile.Owner);
            Ammunition[] ammunitions = projectile.Owner.GetComponentsInChildren<Ammunition>();

            foreach (Ammunition ammunition in ammunitions)
            {
                if (ammunition.WeaponPrefab.GetType() == ammunitionType)
                {
                    ammunition.IncreaseProjectilesCount(CountProjectiles);
                }
            }
        }
    }
}
