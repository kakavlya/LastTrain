using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Source.Scripts.Weapons
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private Joystick _joystick;

        public event Action Fired;
        public event Action StopFired;
        public event Action<int> WeaponChanged;
        public event Action<float> Rotated;

        private float _rotateValue;
        private bool _isMobilePlatform;

        private void Awake()
        {
            if (PlatformDetector.Instance != null &&
                PlatformDetector.Instance.CurrentControlScheme == PlatformDetector.ControlScheme.Joystick)
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
            if (IsPointerOverUI())
            {
                return;
            }

            HandleShooting();
            HandleWeaponSwitch();

            if (_isMobilePlatform)
            {
                HandleRotateJoystick();
            }
            else
            {
                HandleRotateKeys();
            }
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

        private void HandleRotateKeys()
        {
            if (Input.GetKey(KeyCode.A))
            {
                _rotateValue = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _rotateValue = 1;
            }
            else
            {
                _rotateValue = 0;
            }

            Rotated?.Invoke(_rotateValue);
        }

        private void HandleRotateJoystick()
        {
            Debug.Log(_joystick.Horizontal);
                Rotated?.Invoke(_joystick.Horizontal);
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
