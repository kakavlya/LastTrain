using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlotUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool _isActive;

    public bool IsActive => _isActive;

    private void SetActiveTrue()
    {
        _isActive = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var otherItemTranform = eventData.pointerDrag.transform;
        otherItemTranform.SetParent(transform);
        otherItemTranform.position = Vector3.zero;
    }
}
