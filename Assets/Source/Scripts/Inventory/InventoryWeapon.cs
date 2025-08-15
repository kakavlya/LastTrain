using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof (Image))]
public class InventoryWeapon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private WeaponUpgradeConfig _defaultWeaponConfig;

    private WeaponUpgradeConfig _weaponConfig;
    private Image _currentIcon;
    private TextMeshProUGUI _currentText;

    public WeaponUpgradeConfig WeaponConfig => _weaponConfig;

    private void Awake()
    {
        if (_defaultWeaponConfig != null)
            Init(_defaultWeaponConfig);
    }

    public void Init(WeaponUpgradeConfig weaponConfig)
    {
        _weaponConfig = weaponConfig;
        _currentIcon = GetComponent<Image>();
        _currentIcon.sprite = _weaponConfig.Icon;
        _currentText = GetComponentInChildren<TextMeshProUGUI>();
        _currentText.enabled = true;
        _currentText.text = _weaponConfig.WeaponName;
        _currentText.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _currentText.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _currentText.enabled = false;
    }
}
