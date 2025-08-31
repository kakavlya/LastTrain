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

        private readonly List<RaycastResult> _raycastResults = new List<RaycastResult>();

        private float _rotateValue;
        private bool _isMobilePlatform;
        private Camera _mainCamera;

        public event Action<Vector3> Fired;
        public event Action StopFired;
        public event Action<int> WeaponChanged;
        public event Action<float> Rotated;

        private void Awake()
        {
            _mainCamera = Camera.main;

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

            if (!IsPointerOverAnyUI())
            {
                HandleShooting();
                HandleWeaponSwitch();
            }
        }

        private void HandleShooting()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Fired?.Invoke(hit.point);
                }
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
            if (EventSystem.current == null)
                return false;

            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            _raycastResults.Clear();
            EventSystem.current.RaycastAll(eventData, _raycastResults);

            foreach (var result in _raycastResults)
            {
                if (result.gameObject.GetComponentInParent<Joystick>() != null)
                    continue;

                return true;
            }

            return false;
        }

        private bool IsPointerOverAnyUI()
        {
            if (EventSystem.current == null)
                return false;

            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            _raycastResults.Clear();
            EventSystem.current.RaycastAll(eventData, _raycastResults);
            return _raycastResults.Count > 0;
        }
    }
}
