using UnityEngine;

namespace LastTrain.UI.Gameplay
{
    public class UICursorFollower : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private void Update()
        {
            Vector2 mousePosition = Input.mousePosition;
            _rectTransform.position = mousePosition;
        }

        private void OnDestroy()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void Init()
        {
            _rectTransform = GetComponent<RectTransform>();
            Cursor.visible = false;
        }
    }
}
