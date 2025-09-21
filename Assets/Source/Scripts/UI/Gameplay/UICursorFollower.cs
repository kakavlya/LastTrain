using LastTrain.Weapons.System;
using UnityEngine;

namespace LastTrain.UI.Gameplay
{
    [RequireComponent(typeof(RectTransform))]
    public class UICursorFollower : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Camera _cam;
        [SerializeField] private AimingTargetProvider _aim;

        private RectTransform _rt;

        public void Init(Canvas canvas, Camera cam, AimingTargetProvider aim)
        {
            _rt = GetComponent<RectTransform>();
            _canvas = canvas != null ? canvas : GetComponentInParent<Canvas>();
            _cam = cam != null ? cam : Camera.main;
            _aim = aim;

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
            if (_cam == null || _canvas == null) return;

            Vector3 screen = Vector3.zero;

            if (_aim != null)
            {
                var ad = _aim.GetAim();

                if (ad.HasHit)
                {
                    screen = _cam.WorldToScreenPoint(ad.WorldPoint);
                }
                else
                {
                    screen = Input.mousePosition;
                }
            }
            else
            {
                screen = Input.mousePosition;
            }

            switch (_canvas.renderMode)
            {
                case RenderMode.ScreenSpaceOverlay:
                    _rt.position = screen;
                    break;

                case RenderMode.ScreenSpaceCamera:
                    {
                        var canvasRT = (RectTransform)_canvas.transform;
                        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, screen, _canvas.worldCamera, out var local))
                            _rt.anchoredPosition = local;
                        break;
                    }

                case RenderMode.WorldSpace:
                    {
                        var canvasRT = (RectTransform)_canvas.transform;
                        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRT, screen, _canvas.worldCamera ?? _cam, out var worldOnCanvas))
                            _rt.position = worldOnCanvas;
                        break;
                    }
            }
        }
    }
}