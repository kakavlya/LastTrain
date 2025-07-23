using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textCurrentLevel;
    [SerializeField] private LevelsHandler _levelsHandler;
    [SerializeField] private string _gameplayScene;

    private void OnEnable()
    {
        _levelsHandler.LevelChosed += ShowCurrentLevel;
    }

    private void OnDisable()
    {
        _levelsHandler.LevelChosed -= ShowCurrentLevel;
    }

    private void ShowCurrentLevel(LevelSetting levelSetting)
    {
        _textCurrentLevel.text = levelSetting.LevelName;
    }

    public void StartPlay()
    {
        if (_levelsHandler.IsChosed)
        {
            SceneManager.LoadScene(_gameplayScene);
        }
    }
}
