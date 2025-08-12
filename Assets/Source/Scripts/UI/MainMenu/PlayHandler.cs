using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayHandler : MonoBehaviour
{
    [SerializeField] private LevelsHandler _levelsHandler;
    [SerializeField] private string _gameplayScene;
    [SerializeField] private Button _playButton;

    public event Action GameStarted;

    private void Awake()
    {
        _playButton.onClick.AddListener(StartPlay);
    }

    private void StartPlay()
    {
        if (_levelsHandler.IsChosed)
        {
            GameStarted?.Invoke();
            SceneManager.LoadScene(_gameplayScene);
        }
    }
}
