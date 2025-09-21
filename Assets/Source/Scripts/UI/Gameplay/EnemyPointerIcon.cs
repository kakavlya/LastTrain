using UnityEngine;
using UnityEngine.UI;

namespace LastTrain.UI.Gameplay
{
    public class EnemyPointerIcon : MonoBehaviour
    {
        [SerializeField] private Image _pointerImage;

        private bool _isShowed;
        private Color _color;

        private void Awake()
        {
            _pointerImage.enabled = false;
            _isShowed = false;
            _color = _pointerImage.color;
        }

        public void SetIconPosition(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }

        public void Show()
        {
            if (_isShowed) return;
            _isShowed = true;
            _pointerImage.enabled = true;
        }

        public void Hide()
        {
            if (!_isShowed) return;
            _isShowed = false;
            _pointerImage.enabled = false;
        }

        public void ChangeAlpha(float alphaPercent)
        {
            _color.a = 1 - alphaPercent;
            _pointerImage.color = _color;
        }
    }
}