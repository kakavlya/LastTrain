using UnityEngine;

namespace Assets.Source.Scripts.Weapons
{
    public class WeaponInput : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                _weapon.Fire();
            }

            if (Input.GetMouseButtonUp(0))
            {
                _weapon.StopFire();
            }
        }
    }
}
