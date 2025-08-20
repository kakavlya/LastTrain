using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailsPanel : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] private Button _closeBtn;
    [SerializeField] private CanvasGroup _cg;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _levelText;

    [Header("Stats")]
    [SerializeField] private Transform _statsContainer;
    [SerializeField] private StatRow _statRowPrefab;

    private UpgradeConfig _upgradeConfig;
    private BaseProgress _progress;
    private List<StatRow> _statRows = new List<StatRow>();

    private Action _onClose;

    public event Action<StatType> Incremented;

    public void Show(UpgradeConfig cfg, BaseProgress prog, Action onClose)
    {
        _upgradeConfig = cfg;
        _progress = prog;
        _onClose = onClose;
        _icon.sprite = _upgradeConfig.Icon;

        _closeBtn.onClick.RemoveAllListeners();
        _closeBtn.onClick.AddListener(Close);

        foreach (var raw in _statRows)
            Destroy(raw.gameObject);

        _statRows.Clear();

        foreach (var stat in _upgradeConfig.StatConfigs)
        {
            var row = Instantiate(_statRowPrefab, _statsContainer);
            row.Init(stat.StatType, _upgradeConfig, _progress, Upgrade);
            _statRows.Add(row);
        }

        Refresh();
        FadeIn();
    }

    private void Upgrade(StatType stat)
    {
        int level = _progress.GetLevel(stat);
        int maxLevel = _upgradeConfig.GetMaxLevel(stat);

        if (level >= maxLevel) return;

        var coins = CoinsHandler.Instance.CoinsCount;
        int cost = _upgradeConfig.GetCost(stat, level);

        if (coins < cost)
        {
            Debug.LogWarning($"Not enough coins to upgrade {stat}. Required: {cost}, Available: {coins}");
            return;
        }

        CoinsHandler.Instance.RemoveCoins(cost);
        _progress.Increment(stat);
        Incremented?.Invoke(stat);

        SaveManager.Instance.Save();
        Refresh();
    }

    private void Refresh()
    {
        foreach (var row in _statRows)
            row.Refresh();

        int totalLevel = 0;

        foreach (var stat in _upgradeConfig.StatConfigs)
            totalLevel += _progress.GetLevel(stat.StatType);

        _levelText.text = $"{totalLevel} level";
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
