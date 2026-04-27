using LastTrain.Persistence;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace LastTrain.ShopSystem
{
    /// <summary>
    /// Standalone panel for the Train tab.
    /// Shows a single ShopItemUI card for the train upgrades.
    /// When clicked it opens the shared DetailsPanel overlay.
    /// </summary>
    public class TrainShopPanel : MonoBehaviour
    {
        [Header("Train config")]
        [SerializeField] private TrainUpgradeConfig _trainConfig;

        [Header("List")]
        [SerializeField] private ShopItemUI _shopItemPrefab;
        [SerializeField] private Transform  _contentParent;

        [Header("Details overlay (shared with the rest of the shop)")]
        [SerializeField] private Image        _blocker;
        [SerializeField] private DetailsPanel _detailsPrefab;
        [SerializeField] private Transform    _detailsParent;

        private ShopItemUI  _item;
        private DetailsPanel _detailsPanel;

        // ── Lifecycle ──────────────────────────────────────────────────────────

        private void Start()
        {
            BuildPanel();
        }

        private void OnDestroy()
        {
            if (_detailsPanel != null)
                _detailsPanel.Incremented -= OnStatIncremented;
        }

        // ── Private ────────────────────────────────────────────────────────────

        private void BuildPanel()
        {
            // Clear previous (rebuild after details closed)
            if (_item != null)
                Destroy(_item.gameObject);

            _item = Instantiate(_shopItemPrefab, _contentParent);
            _item.Init(_trainConfig, YG2.saves.TrainProgress, OnItemSelected);
        }

        private void OnItemSelected(UpgradeConfig cfg, BaseProgress prog)
        {
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

            BuildPanel();
        }

        private void OnStatIncremented(StatType _) { /* Train has no special stat events */ }
    }
}
