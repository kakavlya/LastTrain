using LastTrain.Data;
using LastTrain.Persistence;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;
using TMPro;

namespace LastTrain.ShopSystem
{
    public class HardpointsShopPanel : MonoBehaviour
    {
        [Header("Views")]
        [SerializeField] private GameObject _slotListView;
        [SerializeField] private GameObject _slotDetailView;

        [Header("Slot List View")]
        [SerializeField] private Button[] _hardpointButtons; // Exactly 4 buttons
        [SerializeField] private TextMeshProUGUI[] _hardpointButtonTexts;

        [Header("Detail View")]
        [SerializeField] private Button _backButton;
        [SerializeField] private Transform _contentParent;
        [SerializeField] private ShopItemUI _shopItemPrefab;

        [Header("Configs")]
        [SerializeField] private HardpointUpgradeConfig[] _hardpointConfigs;
        [SerializeField] private TurretUpgradeConfig[] _turretConfigs;

        [Header("Details Overlay (shared)")]
        [SerializeField] private Image _blocker;
        [SerializeField] private DetailsPanel _detailsPrefab;
        [SerializeField] private Transform _detailsParent;

        private int _selectedHardpointIndex = -1;
        private List<ShopItemUI> _uiItems = new List<ShopItemUI>();
        private DetailsPanel _detailsPanel;

        private void Awake()
        {
            _backButton.onClick.AddListener(ShowSlotList);

            for (int i = 0; i < _hardpointButtons.Length; i++)
            {
                int index = i;
                _hardpointButtons[i].onClick.AddListener(() => OnHardpointClicked(index));
            }
        }

        private void OnEnable()
        {
            ShowSlotList();
        }

        private void OnDestroy()
        {
            if (_detailsPanel != null)
                _detailsPanel.Incremented -= OnStatIncremented;
        }

        // ── View Management ──────────────────────────────────────────────────

        private void ShowSlotList()
        {
            _slotDetailView.SetActive(false);
            _slotListView.SetActive(true);

            // Update button texts to show if locked or unlocked
            for (int i = 0; i < _hardpointButtons.Length; i++)
            {
                if (i < YG2.saves.HardpointsProgress.Count)
                {
                    bool isUnlocked = YG2.saves.HardpointsProgress[i].IsUnlocked;
                    _hardpointButtonTexts[i].text = isUnlocked ? $"Slot {i + 1}" : $"Slot {i + 1} (Locked)";
                }
            }
        }

        private void OnHardpointClicked(int index)
        {
            _selectedHardpointIndex = index;
            _slotListView.SetActive(false);
            _slotDetailView.SetActive(true);
            BuildDetailView();
        }

        // ── Detail View Building ─────────────────────────────────────────────

        private void BuildDetailView()
        {
            // Clear old items
            foreach (var item in _uiItems)
            {
                item.TurretUnlocked -= OnTurretUnlocked;
                item.HardpointUnlocked -= OnHardpointUnlocked;
                Destroy(item.gameObject);
            }
            _uiItems.Clear();

            var hpProgress = YG2.saves.HardpointsProgress[_selectedHardpointIndex];
            
            // 1. Show the Hardpoint itself (Unlock or Upgrade)
            if (_hardpointConfigs != null && _selectedHardpointIndex < _hardpointConfigs.Length)
            {
                var hpConfig = _hardpointConfigs[_selectedHardpointIndex];
                if (hpConfig != null)
                {
                    var hpUi = Instantiate(_shopItemPrefab, _contentParent);
                    hpUi.Init(hpConfig, hpProgress, OnItemSelected);
                    hpUi.HardpointUnlocked += OnHardpointUnlocked;
                    _uiItems.Add(hpUi);
                }
            }

            // 2. If unlocked, show Turrets for this slot
            if (hpProgress.IsUnlocked && _turretConfigs != null)
            {
                foreach (var turretConfig in _turretConfigs)
                {
                    if (turretConfig == null || string.IsNullOrEmpty(turretConfig.TurretId)) continue;

                    var turretProg = hpProgress.TurretsProgress.Find(t => t.TurretId == turretConfig.TurretId);
                    if (turretProg == null)
                    {
                        turretProg = new TurretProgress(turretConfig.TurretId);
                        hpProgress.TurretsProgress.Add(turretProg);
                    }

                    var turretUi = Instantiate(_shopItemPrefab, _contentParent);
                    turretUi.Init(turretConfig, turretProg, OnItemSelected);
                    turretUi.TurretUnlocked += OnTurretUnlocked;
                    _uiItems.Add(turretUi);
                }
            }

            YG2.SaveProgress();
        }

        // ── Interaction ──────────────────────────────────────────────────────

        private void OnHardpointUnlocked(HardpointProgress progress, HardpointUpgradeConfig config)
        {
            progress.SetUnlocked(true);
            YG2.SaveProgress();
            BuildDetailView();
        }

        private void OnTurretUnlocked(TurretProgress progress, TurretUpgradeConfig config)
        {
            progress.SetUnlocked(true);
            
            // Equip it automatically if it's the first one unlocked, or we can just always equip the newest one
            var hpProgress = YG2.saves.HardpointsProgress[_selectedHardpointIndex];
            hpProgress.SetActiveTurret(config.TurretId);
            
            YG2.SaveProgress();
            
            // Rebuild view so Equip/Upgrade states update
            BuildDetailView();
        }

        private void OnItemSelected(UpgradeConfig cfg, BaseProgress prog)
        {
            // Note: If they click a locked Hardpoint, ShopItemUI handles the purchase using the "Unlock Cost".
            // Once bought, we would technically need an event to know when it unlocked, but for now
            // we can just rebuild the view if they buy it from the popup.
            
            if (_detailsPanel == null)
            {
                _detailsPanel = Instantiate(_detailsPrefab, _detailsParent);
                _detailsPanel.Incremented += OnStatIncremented;
            }

            if (_blocker != null)
                _blocker.gameObject.SetActive(true);

            _detailsPanel.Show(cfg, prog, OnDetailsClosed);
        }

        private void OnDetailsClosed()
        {
            if (_blocker != null)
                _blocker.gameObject.SetActive(false);

            BuildDetailView(); // Refresh in case they upgraded something
        }

        private void OnStatIncremented(StatType stat) { }
    }
}
