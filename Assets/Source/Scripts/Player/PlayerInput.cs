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
        private const int MouseLeftButton = 0;
        private const int WeaponSlot1 = 1;
        private const int WeaponSlot2 = 2;
        private const int WeaponSlot3 = 3;
        private const float RotateLeftValue = -1f;
        private const float RotateRightValue = 1f;
        private const float RotateNeutralValue = 0f;

        private readonly List<RaycastResult> _raycastResults = new List<RaycastResult>();

        [SerializeField] private Joystick _joystick;
        [SerializeField] private AimingTargetProvider _aim;

        private float _rotateValue;
        private bool _isMobilePlatform;

        public event Action<Vector3> Fired;

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
            if (_isMobilePlatform)
            {
                if (!IsPointerOverUIWithJoystick())
                {
                    HandleRotateJoystick();
                }
                else
                {
                    Rotated?.Invoke(RotateNeutralValue);
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
            if (Input.GetMouseButton(MouseLeftButton))
            {
                var target = _aim.GetAim().WorldPoint;
                Fired?.Invoke(target);
            }

            if (Input.GetMouseButtonUp(MouseLeftButton))
                StopFired?.Invoke();
        }

        private void HandleWeaponSwitch()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                WeaponChanged?.Invoke(WeaponSlot1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                WeaponChanged?.Invoke(WeaponSlot2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                WeaponChanged?.Invoke(WeaponSlot3);
            }
        }

        private void HandleRotateKeys()
        {
            if (Input.GetKey(KeyCode.A))
            {
                _rotateValue = RotateLeftValue;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _rotateValue = RotateRightValue;
            }
            else
            {
                _rotateValue = RotateNeutralValue;
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