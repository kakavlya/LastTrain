using UnityEngine;
using UnityEngine.UI;

namespace LastTrain.ShopSystem
{
    /// <summary>
    /// Manages the three top-level Shop tabs: Train / Weapons / Hardpoints.
    /// Attach to a parent GameObject in the Shop canvas.
    /// Wire up the three tab buttons and three panel GameObjects in the Inspector.
    /// </summary>
    public class ShopTabController : MonoBehaviour
    {
        [Header("Panels (one per tab)")]
        [SerializeField] private GameObject _trainPanel;
        [SerializeField] private GameObject _weaponsPanel;
        [SerializeField] private GameObject _hardpointsPanel;

        [Header("Tab Buttons")]
        [SerializeField] private Button _trainTabButton;
        [SerializeField] private Button _weaponsTabButton;
        [SerializeField] private Button _hardpointsTabButton;

        [Header("Tab visuals — selected vs deselected icon tint")]
        [SerializeField] private Color _selectedColor   = Color.white;
        [SerializeField] private Color _deselectedColor = new Color(0.55f, 0.55f, 0.55f, 1f);

        private Button[]     _tabs;
        private GameObject[] _panels;

        private void Awake()
        {
            _tabs   = new[] { _trainTabButton, _weaponsTabButton, _hardpointsTabButton };
            _panels = new[] { _trainPanel,     _weaponsPanel,     _hardpointsPanel     };

            _trainTabButton     .onClick.AddListener(() => SwitchTo(0));
            _weaponsTabButton   .onClick.AddListener(() => SwitchTo(1));
            _hardpointsTabButton.onClick.AddListener(() => SwitchTo(2));
        }

        private void Start()
        {
            // Default to Train tab on open
            SwitchTo(0);
        }

        public void SwitchTo(int index)
        {
            for (int i = 0; i < _panels.Length; i++)
            {
                _panels[i].SetActive(i == index);
                ApplyTabTint(_tabs[i], i == index);
            }
        }

        private void ApplyTabTint(Button tab, bool selected)
        {
            if (tab == null) return;
            var img = tab.GetComponent<Image>();
            if (img != null)
                img.color = selected ? _selectedColor : _deselectedColor;
        }
    }
}
