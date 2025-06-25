using Assets.Source.Scripts.Enemies;
using Level;
using Player;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LevelStateMachine : MonoBehaviour
{
    public event Action PlayerDied;
    public event Action LevelCompleted;

    private EnemySpawner _spawner;
    private PlayerHealth _playerHealth;
    private TrainMovement _trainMovement;
    private LevelGenerator _levelGenerator;

    private bool _running;
    private bool _paused;

    internal void Construct(EnemySpawner spawner, 
        Transform player, PlayerHealth playerHealth,
        TrainMovement trainMovement, LevelGenerator levelGenerator)
    {
        _spawner = spawner;
        _playerHealth = playerHealth;
        _levelGenerator = levelGenerator;
        _trainMovement = trainMovement;
        _spawner.Init(player);
    }

    public void StartLevel()
    {
        _spawner.Begin();
        _playerHealth.Died += OnPlayerDied;

        _spawner.Begin();
        _trainMovement.StartMovement();

        Time.timeScale = 1f;
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
    public void OnAllEnemiesDestroyed()
    {
        LevelCompleted?.Invoke();
    }

    public void StopGameplay()
    {
        _running = false;
        _trainMovement.StopMovement();
        _spawner.Stop();
        Time.timeScale = 0f;

        _playerHealth.Died -= OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        PlayerDied?.Invoke();
        _playerHealth.Died -= OnPlayerDied;
        Time.timeScale = 0f;
    }
}
