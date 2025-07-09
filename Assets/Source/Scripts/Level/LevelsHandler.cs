using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsHandler : MonoBehaviour
{
    [SerializeField] private LevelSetting[] _levelSettings;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private Transform _buttonTransform;
    [SerializeField] private SharedData _sharedData;

    private void Awake()
    {
        CreateLevelButtons();
    }

    private void CreateLevelButtons()
    {
        for (int i = 0;  i < _levelSettings.Length; i++)
        {
            GameObject objectButton = Instantiate(_buttonPrefab, _buttonTransform);
            TextMeshProUGUI buttonText = objectButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = _levelSettings[i].LevelName;
            Button button = objectButton.GetComponent<Button>();
            button.enabled = _levelSettings[i].IsAvailable;
            LevelSetting currentLevel = _levelSettings[i];
            button.onClick.AddListener(() => LoadLevelSettings(currentLevel));
        }
            
    }

    private void LoadLevelSettings(LevelSetting levelSetting)
    {
        _sharedData.LevelSetting = levelSetting;
        SceneManager.LoadScene(1);
    }
}
