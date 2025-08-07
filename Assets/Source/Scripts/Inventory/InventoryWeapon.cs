using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof (Image))]
public class InventoryWeapon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] WeaponInfo _weaponInfo;

    private Image _currentIcon;
    private TextMeshProUGUI _currentText;

    public WeaponInfo WeaponInfo => _weaponInfo;

    private void Start()
    {
        _currentIcon = GetComponent<Image>();
        _currentIcon.sprite = _weaponInfo.Icon;
        _currentText = GetComponentInChildren<TextMeshProUGUI>();
        _currentText.enabled = true;
        _currentText.text = _weaponInfo.WeaponName;
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
