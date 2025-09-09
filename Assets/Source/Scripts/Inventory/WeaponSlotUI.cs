using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LastTrain.Inventory
{
    public class WeaponSlotUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] private bool _isFilled;

        private InventoryWeapon _currentWeapon;

        public event Action Filled;

        public bool IsFilled => _isFilled;

        private void Start()
        {
            if (GetComponentInChildren<InventoryWeapon>() != null)
            {
                _isFilled = true;
                _currentWeapon = GetComponentInChildren<InventoryWeapon>();
                _currentWeapon.SetCurrentSlot(this);
            }
        }

        public void SetSlotUnfilled()
        {
            _currentWeapon = null;
            _isFilled = false;
            Filled?.Invoke();
        }

        public void SetSlotFilled(InventoryWeapon weapon)
        {
            _currentWeapon = weapon;
            _isFilled = true;
            Filled?.Invoke();
        }

        public void OnDrop(PointerEventData eventData)
        {
            var drag = eventData.pointerDrag;

            if (drag.GetComponent<InventoryWeapon>() == null)
                return;

            InventoryWeapon newWeapon = drag.GetComponent<InventoryWeapon>();
            WeaponSlotUI originalSlot = newWeapon.CurrentSlot;

            if (originalSlot == this) return;

            if (_isFilled)
            {
                InventoryWeapon currentWeapon = _currentWeapon;
                currentWeapon.transform.SetParent(originalSlot.transform);
                currentWeapon.transform.localPosition = Vector3.zero;
                currentWeapon.SetCurrentSlot(originalSlot);
                newWeapon.transform.SetParent(transform);
                newWeapon.transform.localPosition = Vector3.zero;
                newWeapon.SetCurrentSlot(this);
                originalSlot.SetSlotFilled(currentWeapon);
                SetSlotFilled(newWeapon);
            }
            else
            {
                newWeapon.transform.SetParent(transform);
                newWeapon.transform.localPosition = Vector3.zero;
                newWeapon.SetCurrentSlot(this);
                SetSlotFilled(newWeapon);
                originalSlot.SetSlotUnfilled();
            }
        }
    }
}
