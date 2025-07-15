using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private int _cellNumber;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _ammoCountText;

    private Ammunition _currentAmmunition;

    public event Action<int> UconClicked;

    public int CellNumber => _cellNumber;

    private void OnDisable()
    {
        _currentAmmunition.Updated -= UpdateAmmoText;
    }

    public void ActivateWeapon(Weapon currentWeapon)
    {
        if (currentWeapon != null)
        {
            _currentAmmunition = currentWeapon.Ammunition;

            if (_currentAmmunition != null)
            {
                _currentAmmunition.Updated += UpdateAmmoText;
                UpdateAmmoText(_currentAmmunition.CurrentAmmo);
            }
            else
            {
                _ammoCountText.text = "∞";
            }

            _image.sprite = currentWeapon.UISpriteActive;
        }
    }

    public void DeactivateWeapon(Weapon lastWeapon)
    {
        _image.sprite = lastWeapon.UISpriteDeactive;
    }

    public void OnClickHandle()
    {
        UconClicked?.Invoke(_cellNumber);
    }

    private void UpdateAmmoText(int num)
    {
        _ammoCountText.text = num.ToString();
    }
}
