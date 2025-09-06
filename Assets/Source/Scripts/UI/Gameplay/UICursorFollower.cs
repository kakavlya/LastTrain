using LastTrain.Weapons.System;
using UnityEngine;

namespace LastTrain.UI.Gameplay
{
    [RequireComponent(typeof(RectTransform))]
    public class UICursorFollower : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Camera _camera;
        [SerializeField] private AimingTargetProvider _aim;

        [Header("Options")]
        [Tooltip("≈сли true Ч использовать 3D хит (TryGetWorldTarget), иначе Ч планарную точку GetTargetPoint().")]
        [SerializeField] private bool _useWorldHit = false;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            if (_camera == null) _camera = Camera.main;
        }

        private void OnEnable()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
        }

        private void OnDisable()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void LateUpdate()
        {
            if (_aim == null || _camera == null || _canvas == null) return;

            Vector3 worldPoint = _useWorldHit && _aim.TryGetWorldTarget(out var wp) ? wp : _aim.GetTargetPoint();
            Vector3 screenPoint = _camera.WorldToScreenPoint(worldPoint);

            switch (_canvas.renderMode)
            {
                case RenderMode.ScreenSpaceOverlay:
                    _rectTransform.position = screenPoint;
                    break;
                case RenderMode.ScreenSpaceCamera:
                    {
                        var canvasRect = (RectTransform)_canvas.transform;
                        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, _canvas.worldCamera, out var local))
                            _rectTransform.anchoredPosition = local;
                        break;
                    }
                case RenderMode.WorldSpace:
                    {
                        var canvasRect = (RectTransform)_canvas.transform;
                        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRect, screenPoint, _canvas.worldCamera ?? _camera, out var worldOnCanvas))
                            _rectTransform.position = worldOnCanvas;
                        break;
                    }
            }
        }

        public void Init()
        {
            _rectTransform = GetComponent<RectTransform>();
            Cursor.visible = true;
        }
    }
}
