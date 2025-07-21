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

        private bool _isMobilePlatform;

        private void Awake()
        {
            if (PlatformDetector.Instance.CurrentControlScheme == PlatformDetector.ControlScheme.Joystick)
            {
                _isMobilePlatform = true;
            }
            else
            {
                _isMobilePlatform = false;
            }
        }

        private void LateUpdate()
        {
            if (_isMobilePlatform == false)
            {
                if (IsPointerOverUI())
                {
                    return;
                }

                HandleMouseShooting();
                HandleWeaponSwitch();
            }
        }

        public void UIButtonShoot(BaseEventData eventData)
        {
            Debug.Log("Work");
            Fired?.Invoke();
        }

        public void UIButtonStopShoot(BaseEventData eventData)
        {
            StopFired?.Invoke();
        }


        private void HandleMouseShooting()
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
