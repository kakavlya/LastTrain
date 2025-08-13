using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class ShopItemUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _weaponName;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _dmgText;
    [SerializeField] private TMP_Text _rangeText;

    private WeaponUpgradeConfig _cfg;
    private WeaponProgress _progress;
    private Action<WeaponUpgradeConfig, WeaponProgress> _onSelected;
    private bool _isAvailable;

    public void Init(WeaponUpgradeConfig cfg,
                     WeaponProgress progress,
                     Action<WeaponUpgradeConfig, WeaponProgress> onSelected)
    {
        _cfg = cfg;
        _progress = progress;
        _onSelected = onSelected;

        _icon.sprite = cfg.IconAwailable;
        _isAvailable = progress.IsAvailable;
        Refresh();
    }

    public void OnPointerClick(PointerEventData _)
    {
        _onSelected?.Invoke(_cfg, _progress);
    }

    private void UpdateTextLabels()
    {
        int sumLevel = _progress.DamageLevel + _progress.RangeLevel;

        _rangeText.text = _cfg.GetStat(StatType.Range, _progress.RangeLevel).ToString("F1");
        _dmgText.text = _cfg.GetStat(StatType.Damage, _progress.DamageLevel).ToString("F1");

        _levelText.text = $"Lvl {sumLevel}";
        _weaponName.text = _cfg.WeaponName;
    }

    public void Refresh()
    {
        UpdateTextLabels();
    }
}
