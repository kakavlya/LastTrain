using UnityEngine;

namespace LastTrain.UI.MainMenu
{
    public class MenuCursorFollower : MonoBehaviour
    {
        private void Awake()
        {
            Cursor.visible = false;
        }

        private void Update()
        {
            transform.position = Input.mousePosition;
        }

        private void OnDisable()
        {
            Cursor.visible = true;
        }
    }
}
