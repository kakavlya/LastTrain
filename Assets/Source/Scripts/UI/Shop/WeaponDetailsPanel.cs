using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDetailsPanel : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] private Button _closeBtn;
    [SerializeField] private CanvasGroup _cg;
    [SerializeField] private Image _weaponIcon;

    [Header("Damage Row")]
    [SerializeField] private Slider _damageSlider;
    [SerializeField] private TextMeshProUGUI _damageLvl;
    [SerializeField] private TextMeshProUGUI _damageAmount;
    [SerializeField] private TextMeshProUGUI _damageCost;
    [SerializeField] private Button _damageUpgrade;

    [Header("Range Row")]
    [SerializeField] private Slider _rangeSlider;
    [SerializeField] private TextMeshProUGUI _rangeLvl;
    [SerializeField] private TextMeshProUGUI _rangeDistance;
    [SerializeField] private TextMeshProUGUI _rangeCost;
    [SerializeField] private Button _rangeUpgrade;

    private WeaponUpgradeConfig _cfg;
    private WeaponProgress _prog;
    private Action _onClose;

    public void Show(WeaponUpgradeConfig cfg, WeaponProgress prog, Action onClose)
    {
        _cfg = cfg;
        _prog = prog;
        _onClose = onClose;
        _weaponIcon.sprite = _cfg.Icon;

        _closeBtn.onClick.RemoveAllListeners();
        _damageUpgrade.onClick.RemoveAllListeners();
        _rangeUpgrade.onClick.RemoveAllListeners();

        _closeBtn.onClick.AddListener(Close);
        _damageUpgrade.onClick.AddListener(() => Upgrade(StatType.Damage));
        _rangeUpgrade.onClick.AddListener(() => Upgrade(StatType.Range));

        Refresh();
        FadeIn();
    }

    private void Upgrade(StatType stat)
    {
        int level = _prog.GetLevel(stat);
        int maxLevel = _cfg.GetMaxLevel(stat);
        if (level >= maxLevel) return;

        var save = SaveManager.Instance.Data;
        int cost = _cfg.GetCost(stat, level);

        if(save.Coins < cost)
        {
            Debug.LogWarning($"Not enough coins to upgrade {stat}. Required: {cost}, Available: {save.Coins}");
            return;
        }

        save.Coins -= cost;
        _prog.Increment(stat);

        SaveManager.Instance.Save();
        Refresh();
    }

    private void Refresh()
    {
        float dmgRatio = _prog.DamageLevel / (float)_cfg.MaxDamageLevel;
        float rngRatio = _prog.RangeLevel / (float)_cfg.MaxRangeLevel;

        _damageLvl.text = $"{_prog.DamageLevel}/{_cfg.MaxDamageLevel}";
        _rangeLvl.text = $"{_prog.RangeLevel}/{_cfg.MaxRangeLevel}";

        _damageSlider.value = dmgRatio;
        _rangeSlider.value = rngRatio;

        _rangeDistance.text = _cfg.GetStat(StatType.Range, _prog.RangeLevel).ToString("F1");
        _damageAmount.text = _cfg.GetStat(StatType.Damage, _prog.DamageLevel).ToString("F1");

        _damageCost.text = _cfg.GetCost(StatType.Damage, _prog.DamageLevel).ToString();
        _rangeCost.text = _cfg.GetCost(StatType.Range, _prog.RangeLevel).ToString();
    }

    private void FadeIn()
    {
        _cg.alpha = 0;
        gameObject.SetActive(true);
        DOTween.To(v => _cg.alpha = v, 0, 1, .25f).SetEase(Ease.OutQuad);
    }

    private void Close()
    {
        DOTween.To(v => _cg.alpha = v, 1, 0, .2f)
               .OnComplete(() =>
               {
                   gameObject.SetActive(false);
                   _onClose?.Invoke();
               });
    }
}
