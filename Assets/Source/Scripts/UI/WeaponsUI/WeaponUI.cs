using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class WeaponUI : MonoBehaviour
{
    [SerializeField] private int _cellNumber;
    [SerializeField] private TextMeshProUGUI _ammoCountText;
    [SerializeField] private TextMeshProUGUI _addedCountText;

    private int _showTime = 2;
    private Image _image;
    private Ammunition _currentAmmunition;

    public event Action<int> UconClicked;

    public int CellNumber => _cellNumber;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void OnDisable()
    {
        if (_currentAmmunition != null)
        {
            _currentAmmunition.Updated -= UpdateAmmoText;
            _currentAmmunition.AmmoAdded -= LaunchAddedAmmo;
        }
    }

    public void ActivateWeapon(Weapon currentWeapon)
    {
        if (currentWeapon != null)
        {
            _currentAmmunition = currentWeapon.Ammunition;

            if (_currentAmmunition != null)
            {
                _currentAmmunition.Updated += UpdateAmmoText;
                _currentAmmunition.AmmoAdded += LaunchAddedAmmo;
                UpdateAmmoText(_currentAmmunition.CurrentAmmo);
            }
            else
            {
                _ammoCountText.text = "∞";
            }

            _image.sprite = currentWeapon.UISpriteActive;
        }
    }

    public void DeactivateWeapon(Weapon weapon)
    {
        _image.sprite = weapon.UISpriteDeactive;
    }

    public void OnClickHandle()
    {
        UconClicked?.Invoke(_cellNumber);
    }

    private void UpdateAmmoText(int num)
    {
        _ammoCountText.text = num.ToString();
    }

    private void LaunchAddedAmmo(int addedAmmo)
    {
        StartCoroutine(ShowAddedAmmo(addedAmmo));
    }

    private IEnumerator ShowAddedAmmo(int addedAmmo)
    {
        _addedCountText.text = "+ " + addedAmmo.ToString();
        yield return new WaitForSeconds(_showTime);
        _addedCountText.text = null;
    }
}
