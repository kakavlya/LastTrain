using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Source.Scripts.Weapons
{
    public class WeaponInput : MonoBehaviour
    {
        public event Action Fired;
        public event Action StopFired;
        public event Action<int> WeaponChanged;

        private void LateUpdate()
        {
            if (IsPointerOverUI())
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

        private bool IsPointerOverUI()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }
}
