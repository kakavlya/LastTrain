using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Source.Scripts.Weapons
{
    public class WeaponInput : MonoBehaviour
    {
        public event Action Fired;
        public event Action StopFired;
        public event Action<int> WeaponChanged;

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            HandleShooting();
            HandleWeaponSwitch();
        }

        private void HandleShooting()
        {
            if (Input.GetMouseButton(0))
            {
                Fired?.Invoke();
            }

            if (Input.GetMouseButtonUp(0))
            {
                StopFired?.Invoke();
            }
        }

        private void HandleWeaponSwitch()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                WeaponChanged?.Invoke(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                WeaponChanged?.Invoke(2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                WeaponChanged?.Invoke(3);
            }
        }
    }
}
