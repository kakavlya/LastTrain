using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICursorFollower : MonoBehaviour
{
    private RectTransform _rectTransform;

    public void Init()
    {
        _rectTransform = GetComponent<RectTransform>();
        Cursor.visible = false;
    }

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
}
