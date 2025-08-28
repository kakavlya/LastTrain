using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LastTrain.Inventory
{
    public class WeaponSlotUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] private bool _isFilled;

        public event Action Filled;

        public bool IsFilled => _isFilled;

        private void Start()
        {
            if (GetComponentInChildren<InventoryWeapon>() != null)
            {
                _isFilled = true;
            }
        }

        public void SetSlotUnfilled()
        {
            _isFilled = false;
            Filled?.Invoke();
        }

        public void SetSlotFilled()
        {
            _isFilled = true;
            Filled?.Invoke();
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (_isFilled)
                return;

            var drag = eventData.pointerDrag;

            if (drag.GetComponent<InventoryWeapon>() == null)
                return;

            var otherItemTranform = eventData.pointerDrag.transform;
            otherItemTranform.SetParent(transform);
            otherItemTranform.position = Vector3.zero;
            _isFilled = true;
            Filled?.Invoke();
        }
    }
}
