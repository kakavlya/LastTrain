using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting.FullSerializer;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class ShopItemUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _levelText;

    private WeaponUpgradeConfig _cfg;
    private WeaponProgress _progress;
    private Action<WeaponUpgradeConfig, WeaponProgress> _onSelected;

    public void Init(WeaponUpgradeConfig cfg,
                     WeaponProgress progress,
                     Action<WeaponUpgradeConfig, WeaponProgress> onSelected)
    {
        _cfg = cfg;
        _progress = progress;
        _onSelected = onSelected;

        _icon.sprite = cfg.Icon;
        UpdateLevelLabel();
    }

    public void OnPointerClick(PointerEventData _)
    {
        _onSelected?.Invoke(_cfg, _progress);
    }

    private void UpdateLevelLabel()
    {
        int sumLevel = _progress.DamageLevel + _progress.RangeLevel;
        _levelText.text = $"Lvl {sumLevel}";
    }

    public void Refresh()
    {
        UpdateLevelLabel();
    }
}
