using LastTrain.Core;
using LastTrain.Weapons.System;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LastTrain.Player
{
    public class PlayerInput : MonoBehaviour
    {
        private const int _mouseLeftButton = 0;
        private const int _weaponSlot1 = 1;
        private const int _weaponSlot2 = 2;
        private const int _weaponSlot3 = 3;
        private const float _rotateLeftValue = -1f;
        private const float _rotateRightValue = 1f;
        private const float _rotateNeutralValue = 0f;

        private readonly List<RaycastResult> _raycastResults = new List<RaycastResult>();

        [SerializeField] private Joystick _joystick;
        [SerializeField] private AimingTargetProvider _aim;

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
                    Rotated?.Invoke(_rotateNeutralValue);
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
            if (Input.GetMouseButton(_mouseLeftButton))
            {
                var target = _aim.GetAim().WorldPoint;
                Fired?.Invoke(target);
            }

            if (Input.GetMouseButtonUp(_mouseLeftButton))
                StopFired?.Invoke();
        }

        private void HandleWeaponSwitch()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                WeaponChanged?.Invoke(_weaponSlot1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                WeaponChanged?.Invoke(_weaponSlot2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                WeaponChanged?.Invoke(_weaponSlot3);
            }
        }

        private void HandleRotateKeys()
        {
            if (Input.GetKey(KeyCode.A))
            {
                _rotateValue = _rotateLeftValue;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _rotateValue = _rotateRightValue;
            }
            else
            {
                _rotateValue = _rotateNeutralValue;
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

            PointerEventData eventData = new PointerEventData(EventSystem.current) {
                position = Input.mousePosition};

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

            PointerEventData eventData = new PointerEventData(EventSystem.current) {
                position = Input.mousePosition};

            _raycastResults.Clear();
            EventSystem.current.RaycastAll(eventData, _raycastResults);
            return _raycastResults.Count > 0;
        }
    }
}