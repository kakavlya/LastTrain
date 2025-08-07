using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsHandler : MonoBehaviour
{
    [SerializeField] private LevelSetting[] _levelSettings;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private Transform _contentButtonsTransform;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private TextMeshProUGUI _textCurrentLevel;
    [SerializeField] private SharedData _sharedData;

    public event Action LevelChosed;

    public bool IsChosed { get; private set; }

    private void Start()
    {
        CreateLevelButtons();
        IsChosed = false;
    }

    private void OnEnable()
    {
        StartCoroutine(CreateLevelButtonsAndResetScroll());
    }

    private void CreateLevelButtons()
    {
        for (int i = 0;  i < _levelSettings.Length; i++)
        {
            GameObject objectButton = Instantiate(_buttonPrefab, _contentButtonsTransform);
            TextMeshProUGUI buttonText = objectButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = _levelSettings[i].LevelName;
            Button button = objectButton.GetComponent<Button>();
            button.enabled = _levelSettings[i].IsAvailable;
            LevelSetting currentLevel = _levelSettings[i];
            button.onClick.AddListener(() => LoadLevelSettings(currentLevel));
        }
    }

    private IEnumerator CreateLevelButtonsAndResetScroll()
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
        LevelChosed?.Invoke();
    }

    private void ShowCurrentLevel(LevelSetting levelSetting)
    {
        _textCurrentLevel.text = levelSetting.LevelName;
    }
}
