using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using LastTrain.Core;

namespace LastTrain.Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private Joystick _joystick;

        private float _rotateValue;
        private bool _isMobilePlatform;

        public event Action Fired;
        public event Action StopFired;
        public event Action<int> WeaponChanged;
        public event Action<float> Rotated;

        private void Awake()
        {
            if (PlatformDetector.Instance != null &&
                PlatformDetector.Instance.CurrentControlScheme == PlatformDetector.ControlScheme.Mobile)
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
            if (!IsPointerOverAnyUI())
            {
                HandleShooting();
                HandleWeaponSwitch();
            }

            if (_isMobilePlatform)
            {
                if (!IsPointerOverUIWithJoystick())
                {
                    HandleRotateJoystick();
                }
                else
                {
                    Rotated?.Invoke(0);
                }
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
            Rotated?.Invoke(_joystick.Horizontal);
        }

        private bool IsPointerOverUIWithJoystick()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            foreach (var result in results)
            {
                if (result.gameObject.GetComponentInParent<Joystick>() != null)
                    continue;

                return true;
            }

            return false;
        }

        private bool IsPointerOverAnyUI()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }
}
