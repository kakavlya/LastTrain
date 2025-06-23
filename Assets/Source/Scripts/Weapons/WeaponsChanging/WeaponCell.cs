using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCell : MonoBehaviour
{
    [SerializeField] private int _cellNumber;

    public event Action<int> UconClicked;

    public int CellNumber => _cellNumber;

    public void SetIcon(Weapon currentWeapon)
    {
        if (currentWeapon != null)
        {
            TryGetComponent(out Image currentImage);

            if (currentImage != null)
                currentImage.sprite = currentWeapon.UISprite;
        }
    }

    public void OnClickHandle()
    {
        UconClicked?.Invoke(_cellNumber);
    }
}
