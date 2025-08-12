using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlotUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool _isActive;
    [SerializeField] private bool _isFilled;

    public bool IsActive => _isActive;
    public bool IsFilled => _isFilled;

    public event Action Filled;

    private void Start()
    {
        if (GetComponentInChildren<InventoryWeapon>() != null)
        {
            _isFilled = true;
        }
    }

    private void SetActiveTrue()
    {
        _isActive = true;
    }

    public void SetSlotUnfilled()
    {
        _isFilled = false;
        Filled?.Invoke();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (_isFilled)
            return;

        var otherItemTranform = eventData.pointerDrag.transform;
        otherItemTranform.SetParent(transform);
        otherItemTranform.position = Vector3.zero;
        _isFilled = true;
        Filled?.Invoke();
    }
}
