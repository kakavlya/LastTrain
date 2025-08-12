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
    private LevelGenerator _levelGenerator;
    private LevelProgress _levelProgress;

    private bool _running;
    private bool _paused;

    public event Action PlayerDied;
    public event Action LevelCompleted;

    internal void Construct(EnemySpawner spawner, 
        Transform player, PlayerHealth playerHealth,
        TrainMovement trainMovement, LevelGenerator levelGenerator, LevelProgress levelProgress)
    {
        _spawner = spawner;
        _playerHealth = playerHealth;
        _levelGenerator = levelGenerator;
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

        _running = true;
        _paused = false;
    }

    public void PauseLevel()
    {
        if (!_running || _paused) return;

        Time.timeScale = 0f;
        _trainMovement.StopMovement();
        _spawner.Pause();
        _paused = true;
    }

    public void ResumeLevel()
    {
        if (!_paused) return;
        Time.timeScale = 1f;
        _trainMovement.StartMovement();
        _spawner.Resume();
        _paused = false;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    public void StopGameplay()
    {
        _running = false;
        _trainMovement.StopMovement();
        _spawner.Stop();
        Time.timeScale = 0f;

        _playerHealth.Died -= OnPlayerDied;
    }

    public void ReturnToMenu()
    {
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
