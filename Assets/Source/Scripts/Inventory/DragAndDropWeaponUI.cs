using UnityEngine;
using UnityEngine.EventSystems;

namespace LastTrain.Inventory
{
    public class DragAndDropWeaponUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private Canvas _canvas;
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private Transform _originalParent;
        private string _dragLayerName = "DragLayer";
        private Transform _dragLayer;
        private WeaponSlotUI _originalWeaponSlotUI;

        private void Start()
        {
            _originalWeaponSlotUI = GetComponentInParent<WeaponSlotUI>();
            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _dragLayer = _canvas.transform.Find(_dragLayerName);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            var slotTransform = GetComponentInParent<RectTransform>();
            slotTransform.SetAsLastSibling();
            _canvasGroup.blocksRaycasts = false;
            _originalParent = transform.parent;
            transform.SetParent(_dragLayer);
            transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (transform.parent == _dragLayer)
            {
                transform.SetParent(_originalParent);
                transform.localPosition = Vector3.zero;
                _canvasGroup.blocksRaycasts = true;
            }
            else
            {
                _originalWeaponSlotUI.SetSlotUnfilled();
                _originalWeaponSlotUI = GetComponentInParent<WeaponSlotUI>();
                transform.localPosition = Vector3.zero;
                _canvasGroup.blocksRaycasts = true;
            }
        }
    }
}
