using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LastTrain.AmmunitionSystem;
using LastTrain.Weapons.Types;

namespace LastTrain.UI.Gameplay
{
    [RequireComponent(typeof(Image))]
    public class WeaponUI : MonoBehaviour
    {
        [SerializeField] private int _cellNumber;
        [SerializeField] private TextMeshProUGUI _ammoCountText;
        [SerializeField] private TextMeshProUGUI _addedCountText;
        [SerializeField] private GameObject _addedAmmoBackground;

        private int _showTime = 2;
        private Image _image;
        private Ammunition _currentAmmunition;
        private string _infinitySymbol = "∞";
        private string _plusSymbol = "+";

        public event Action<int> UconClicked;

        public int CellNumber => _cellNumber;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _addedAmmoBackground.SetActive(false);
        }

        private void OnDestroy()
        {
            if (_currentAmmunition != null)
            {
                _currentAmmunition.Updated -= UpdateAmmoText;
                _currentAmmunition.AmmoAdded -= LaunchAddedAmmo;
            }
        }

        public void ActivateWeapon(Weapon currentWeapon, Ammunition ammunition)
        {
            if (currentWeapon != null)
            {
                if (_currentAmmunition != null)
                {
                    _currentAmmunition.Updated -= UpdateAmmoText;
                    _currentAmmunition.AmmoAdded -= LaunchAddedAmmo;
                }

                _currentAmmunition = ammunition;

                if (_currentAmmunition != null)
                {
                    _currentAmmunition.Updated += UpdateAmmoText;
                    _currentAmmunition.AmmoAdded += LaunchAddedAmmo;
                    UpdateAmmoText(_currentAmmunition.CurrentAmmo);
                }
                else
                {
                    _ammoCountText.text = _infinitySymbol;
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
            _addedAmmoBackground.gameObject.SetActive(true);
            _addedCountText.text = _plusSymbol + addedAmmo.ToString();
            yield return new WaitForSeconds(_showTime);
            _addedCountText.text = null;
            _addedAmmoBackground.gameObject.SetActive(false);
        }
    }
}
