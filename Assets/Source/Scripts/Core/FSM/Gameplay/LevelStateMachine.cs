using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using LastTrain.Enemies;
using LastTrain.Player;
using LastTrain.UI.Gameplay;

namespace LastTrain.Core.FSM
{
    public class LevelStateMachine : MonoBehaviour
    {
        public enum State
        {
            Idle,
            Running,
            Paused,
            PlayerDead,
            Completed
        }

        private static void SetTimeScale(float value)
        {
            if (Math.Abs(Time.timeScale - value) > 0.0001f)
                Time.timeScale = value;
        }

        private string _menuScene;
        private EnemySpawner _spawner;
        private PlayerHealth _playerHealth;
        private TrainMovement _trainMovement;
        private LevelProgress _levelProgress;
        private Transform _player;

        private State _state = State.Idle;
        private bool _bound;

        public event Action PlayerDied;
        public event Action LevelCompleted;

        private void Awake()
        {
            if (_spawner == null || _playerHealth == null || _trainMovement == null || _levelProgress == null || _player == null)
                ResolveFromSceneOrThrow();
        }

        private void OnEnable()
        {
            Bind();
        }

        private void OnDisable()
        {
            Unbind();
        }

        public void Construct(
            EnemySpawner spawner,
            Transform player,
            PlayerHealth playerHealth,
            TrainMovement trainMovement,
            LevelProgress levelProgress,
            string menuScene = null)
        {
            _spawner = spawner;
            _player = player;
            _playerHealth = playerHealth;
            _trainMovement = trainMovement;
            _levelProgress = levelProgress;
            _menuScene = menuScene;

            if (_spawner != null && _player != null)
                _spawner.SetTarget(_player);
        }

        public void StartLevel()
        {
            if (_state == State.Running)
                return;

            SetTimeScale(1f);
            _trainMovement.StartMovement();
            _levelProgress.StartCountdown();
            _spawner.Begin();

            _state = State.Running;
        }

        public void PauseLevel()
        {
            if (_state != State.Running)
                return;

            SetTimeScale(0f);
            _trainMovement.StopMovement();
            _spawner.Pause();

            _state = State.Paused;
        }

        public void ResumeLevel()
        {
            if (_state != State.Paused)
                return;

            SetTimeScale(1f);
            _trainMovement.StartMovement();
            _spawner.Resume();

            _state = State.Running;
        }

        public void RestartLevel()
        {
            StopGameplayInternal();
            SetTimeScale(1f);

            var current = SceneManager.GetActiveScene();
            SceneManager.LoadScene(current.name);
        }

        public void ReturnToMenu()
        {
            StopGameplayInternal();
            SetTimeScale(1f);

            if (!string.IsNullOrEmpty(_menuScene))
                SceneManager.LoadScene(_menuScene);
        }

        private void Bind()
        {
            if (_bound)
                return;

            if (_spawner == null || _playerHealth == null || _trainMovement == null || _levelProgress == null || _player == null)
                ResolveFromSceneOrThrow();

            _playerHealth.Died += OnPlayerDiedInternal;
            _levelProgress.LevelCompleted += OnLevelCompletedInternal;

            _bound = true;
        }

        private void Unbind()
        {
            if (!_bound)
                return;

            if (_playerHealth != null)
                _playerHealth.Died -= OnPlayerDiedInternal;

            if (_levelProgress != null)
                _levelProgress.LevelCompleted -= OnLevelCompletedInternal;

            _bound = false;
        }

        private void ResolveFromSceneOrThrow()
        {
            if (_player == null)
            {
                var playerGo = GameObject.FindGameObjectWithTag("Player");
                if (playerGo == null)
                    throw new InvalidOperationException("LevelStateMachine: GameObject with tag 'Player' not found.");

                _player = playerGo.transform;
            }

            _spawner ??= FindObjectOfType<EnemySpawner>();
            _playerHealth ??= FindObjectOfType<PlayerHealth>();
            _trainMovement ??= FindObjectOfType<TrainMovement>();
            _levelProgress ??= FindObjectOfType<LevelProgress>();

            if (_spawner == null)
                throw new InvalidOperationException("LevelStateMachine: EnemySpawner not found in scene.");

            if (_playerHealth == null)
                throw new InvalidOperationException("LevelStateMachine: PlayerHealth not found in scene.");

            if (_trainMovement == null)
                throw new InvalidOperationException("LevelStateMachine: TrainMovement not found in scene.");

            if (_levelProgress == null)
                throw new InvalidOperationException("LevelStateMachine: LevelProgress not found in scene.");

            _spawner.SetTarget(_player);
        }

        private void OnPlayerDiedInternal()
        {
            _state = State.PlayerDead;
            StopGameplayInternal();
            SetTimeScale(0f);
            PlayerDied?.Invoke();
        }

        private void OnLevelCompletedInternal()
        {
            _state = State.Completed;
            StopGameplayInternal();
            SetTimeScale(0f);
            LevelCompleted?.Invoke();
        }

        private void StopGameplayInternal()
        {
            _trainMovement.StopMovement();
            _spawner.Pause();
        }
    }
}