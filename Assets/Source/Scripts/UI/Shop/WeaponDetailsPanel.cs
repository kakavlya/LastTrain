using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDetailsPanel : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] private Button _closeBtn;
    [SerializeField] private CanvasGroup _cg;
    [SerializeField] private Image _weaponIcon;
    [SerializeField] private TextMeshProUGUI _weaponLevelText;

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
    private WeaponProgress _weaponProgress;
    private Action _onClose;

    public void Show(WeaponUpgradeConfig cfg, WeaponProgress prog, Action onClose)
    {
        _cfg = cfg;
        _weaponProgress = prog;
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
        int level = _weaponProgress.GetLevel(stat);
        int maxLevel = _cfg.GetMaxLevel(stat);

        if (level >= maxLevel) return;

        var coins = CoinsHandler.Instance.CoinsCount;
        int cost = _cfg.GetCost(stat, level);

        if(coins < cost)
        {
            Debug.LogWarning($"Not enough coins to upgrade {stat}. Required: {cost}, Available: {coins}");
            return;
        }

        CoinsHandler.Instance.RemoveCoins(cost);
        _weaponProgress.Increment(stat);

        SaveManager.Instance.Save();
        Refresh();
    }

    private void Refresh()
    {
        float dmgRatio = _weaponProgress.DamageLevel / (float)_cfg.FindStat(StatType.Damage).MaxLevel;
        float rngRatio = _weaponProgress.RangeLevel / (float)_cfg.FindStat(StatType.Range).MaxLevel;

        _damageLvl.text = $"{_weaponProgress.DamageLevel}/{_cfg.FindStat(StatType.Damage).MaxValue}";
        _rangeLvl.text = $"{_weaponProgress.RangeLevel}/{_cfg.FindStat(StatType.Range).MaxValue}";

        _damageSlider.value = dmgRatio;
        _rangeSlider.value = rngRatio;

        _rangeDistance.text = _cfg.GetStat(StatType.Range, _weaponProgress.RangeLevel).ToString("F1");
        _damageAmount.text = _cfg.GetStat(StatType.Damage, _weaponProgress.DamageLevel).ToString("F1");

        bool canUpgradeDamage = _weaponProgress.DamageLevel < _cfg.FindStat(StatType.Damage).MaxLevel;
        _damageCost.text = canUpgradeDamage ? (_cfg.GetCost(StatType.Damage, _weaponProgress.DamageLevel).ToString()) : "-";
        _damageUpgrade.interactable = canUpgradeDamage;

        bool canUpgradeRange = _weaponProgress.RangeLevel < _cfg.FindStat(StatType.Range).MaxLevel;
        _rangeCost.text = canUpgradeRange ? (_cfg.GetCost(StatType.Range, _weaponProgress.RangeLevel).ToString()) : "-";
        _rangeUpgrade.interactable = canUpgradeRange;

        _weaponLevelText.text = $"{_weaponProgress.DamageLevel + _weaponProgress.RangeLevel} level";
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
