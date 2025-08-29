using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using LastTrain.Data;

namespace LastTrain.Level
{
    public class LevelsHandler : MonoBehaviour
    {
        [SerializeField] private LevelSetting[] _levelSettings;
        [SerializeField] private GameObject _buttonPrefab;
        [SerializeField] private Transform _contentButtonsTransform;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private TextMeshProUGUI _textCurrentLevel;
        [SerializeField] private Sprite _unavailableIcon;
        [SerializeField] private SharedData _sharedData;

        public bool IsChosed { get; private set; }

        private void Start()
        {
            LoadLevels();
            CreateLevelButtons();
            IsChosed = false;
        }

        private void OnEnable()
        {
            StartCoroutine(ResizeScrollOnTop());
        }

        private void LoadLevels()
        {
            var levelsAvailability = YG2.saves.LevelsAvailability;

            if (levelsAvailability.Count == 0)
            {
                for (int i = 0; i < _levelSettings.Length; i++)
                {
                    levelsAvailability.Add(new LevelAvailability(_levelSettings[i].LevelNumber, false));

                    if (i == 0)
                    {
                        levelsAvailability[0].IsAvailable = true;
                    }
                }

                YG2.SaveProgress();
            }

            foreach (var setting in _levelSettings)
            {
                foreach (var level in levelsAvailability)
                {
                    if (setting.LevelNumber == level.LevelNumber)
                    {
                        setting.IsAvailable = level.IsAvailable;
                    }
                }
            }

            _sharedData.SetAllLevels(_levelSettings);
        }

        private void CreateLevelButtons()
        {
            for (int i = 0; i < _levelSettings.Length; i++)
            {
                GameObject objectButton = Instantiate(_buttonPrefab, _contentButtonsTransform);
                TextMeshProUGUI buttonText = objectButton.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = _levelSettings[i].LevelNumber.ToString();
                Button button = objectButton.GetComponent<Button>();
                LevelAvailability levelAvail = YG2.saves.LevelsAvailability.Find(
                    level => level.LevelNumber == _levelSettings[i].LevelNumber);
                bool isAvailable = levelAvail != null && levelAvail.IsAvailable;

                if (!isAvailable)
                {
                    button.enabled = false;
                    var buttonIcon = objectButton.GetComponent<Image>();
                    buttonIcon.sprite = _unavailableIcon;
                }

                LevelSetting currentLevel = _levelSettings[i];
                button.onClick.AddListener(() => LoadLevelSettings(currentLevel));
            }
        }

        private IEnumerator ResizeScrollOnTop()
        {
            yield return null;

            ResizeContentForGrid();

            yield return null;

            _scrollRect.verticalNormalizedPosition = 1f;
        }

        private void ResizeContentForGrid()
        {
            var layout = _contentButtonsTransform.GetComponent<GridLayoutGroup>();
            var contentRect = _contentButtonsTransform.GetComponent<RectTransform>();
            int totalItems = _contentButtonsTransform.childCount;
            int columns = Mathf.Max(1, Mathf.FloorToInt((contentRect.rect.width + layout.spacing.x) / (layout.cellSize.x + layout.spacing.x)));
            int rows = Mathf.CeilToInt((float)totalItems / columns);
            float height = rows * layout.cellSize.y + layout.spacing.y * (rows - 1) + layout.padding.top + layout.padding.bottom;
            contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        private void LoadLevelSettings(LevelSetting levelSetting)
        {
            _sharedData.LevelSetting = levelSetting;
            ShowCurrentLevel(levelSetting);
            IsChosed = true;
        }

        private void ShowCurrentLevel(LevelSetting levelSetting)
        {
            _textCurrentLevel.text = levelSetting.LevelNumber.ToString();
        }
    }
}
