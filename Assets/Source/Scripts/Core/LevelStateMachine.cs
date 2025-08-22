using Assets.Source.Scripts.Enemies;
using Level;
using Player;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStateMachine : MonoBehaviour
{
    [SerializeField] private string _menuScene;

    private EnemySpawner _spawner;
    private PlayerHealth _playerHealth;
    private TrainMovement _trainMovement;
    private LevelProgress _levelProgress;

    private bool _isRunning;
    private bool _isPaused;

    public event Action PlayerDied;
    public event Action LevelCompleted;

    internal void Construct(EnemySpawner spawner, 
        Transform player, PlayerHealth playerHealth,
        TrainMovement trainMovement, LevelProgress levelProgress)
    {
        _spawner = spawner;
        _playerHealth = playerHealth;
        _trainMovement = trainMovement;
        _spawner.Init(player);
        _levelProgress = levelProgress;
    }

    public void StartLevel()
    {
        Time.timeScale = 1f;
        _trainMovement.StartMovement();
        _levelProgress.StartCountdown();
        _spawner.Begin();
        _playerHealth.Died += OnPlayerDied;
        _levelProgress.LevelComplited += OnLevelComplited;
        _isRunning = true;
        _isPaused = false;
    }

    public void PauseLevel()
    {
        if (!_isRunning || _isPaused) return;

        Time.timeScale = 0f;
        _trainMovement.StopMovement();
        _spawner.Pause();
        _isPaused = true;
    }

    public void ResumeLevel()
    {
        if (!_isPaused) return;
        Time.timeScale = 1f;
        _trainMovement.StartMovement();
        _spawner.Resume();
        _isPaused = false;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    public void StopGameplay()
    {
        _isRunning = false;
        _trainMovement.StopMovement();
        _spawner.Pause();
        Time.timeScale = 0f;
        _playerHealth.Died -= OnPlayerDied;
    }

    public void ResumeGameplay()
    {
        _isRunning = true;
        _trainMovement.StartMovement();
        _spawner.Resume();
        Time.timeScale = 1f;
        _playerHealth.Died += OnPlayerDied;
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(_menuScene);
    }

    private void OnPlayerDied()
    {
        PlayerDied?.Invoke();
        _playerHealth.Died -= OnPlayerDied;
        Time.timeScale = 0f;
    }

    private void OnLevelComplited()
    {
        StopGameplay();
        _levelProgress.LevelComplited -= OnLevelComplited;
        LevelCompleted?.Invoke();
    }
}
