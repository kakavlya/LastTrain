using LastTrain.AmmunitionSystem;
using LastTrain.CameraSystem;
using LastTrain.Core.FSM;
using LastTrain.Enemies;
using LastTrain.Level;
using LastTrain.Particles;
using LastTrain.Player;
using LastTrain.Projectiles;
using LastTrain.Training;
using LastTrain.Turrets;
using LastTrain.UI.Gameplay;
using LastTrain.Weapons.System;
using UnityEngine;

namespace LastTrain.Core
{
    public class CompositionRoot : MonoBehaviour
    {
        [Header("UI & Camera")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Camera _cam;
        [SerializeField] private UIStateMachine _uIStateMachine;
        [SerializeField] private UICursorFollower _uiCursorFollower;

        [Header("Core gameplay")]
        [SerializeField] private LevelStateMachine _levelStateMachine;
        [SerializeField] private GameplayTraining _gameplayTraining;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private Transform _player;
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private TrainMovement _trainMovement;
        [SerializeField] private LevelGenerator _levelGenerator;
        [SerializeField] private LevelProgress _levelProgress;
        [SerializeField] private LevelElementsCreator _levelElementsCreator;

        [Header("Weapons & Aiming")]
        [SerializeField] private WeaponsHandler _weaponHandler;
        [SerializeField] private WeaponRotator _weaponRotator;
        [SerializeField] private AimingTargetProvider _aimingTargetProvider;

        [Header("Pools & Camera Follow")]
        [SerializeField] private ParticlePool _particlePool;
        [SerializeField] private PickableAmmunitionPool _pickableAmmunitionPool;
        [SerializeField] private PickableAmmunitionSpawner _pickableAmmunitionSpawner;
        [SerializeField] private ProjectilePool _projectilePool;
        [SerializeField] private EnemyPool _enemyPool;
        [SerializeField] private CameraFollower _cameraFollower;

        [Header("Turrets")]
        [Tooltip("Optional — leave unassigned in scenes without turrets.")]
        [SerializeField] private TurretsHandler _turretsHandler;

        [Header("Scenes")]
        [SerializeField] private string _menuScene = "MainMenu";

        private void Awake()
        {
            _enemySpawner.Init();
            _aimingTargetProvider.Init();
            _uiCursorFollower.Init(_canvas, _cam, _aimingTargetProvider);
            _weaponRotator.Init();
            _weaponHandler.Init(_aimingTargetProvider);
            _trainMovement.Init();
            _levelElementsCreator.Init();
            _pickableAmmunitionPool.Init();
            _pickableAmmunitionSpawner.Init();
            _levelGenerator.Init();
            _particlePool.Init();
            _projectilePool.Init();
            _levelProgress.Init();

            // Turrets — optional; skipped if no TurretsHandler is assigned
            _turretsHandler?.Init();

            _levelStateMachine.Construct(
                _enemySpawner,
                _player,
                _playerHealth,
                _trainMovement,
                _levelProgress,
                _menuScene);

            // ── UI → Level state machine ──────────────────────────────────────
            _uIStateMachine.StartClicked   += _levelStateMachine.StartLevel;
            _uIStateMachine.RestartClicked += _levelStateMachine.RestartLevel;
            _uIStateMachine.PauseClicked   += _levelStateMachine.PauseLevel;
            _uIStateMachine.ResumeClicked  += _levelStateMachine.ResumeLevel;
            _uIStateMachine.MenuClicked    += _levelStateMachine.ReturnToMenu;
            _uIStateMachine.FSM.Switch<LevelStartState>();

            _levelStateMachine.PlayerDied      += () => _uIStateMachine.FSM.Switch<GameOverState>();
            _levelStateMachine.LevelCompleted  += () => _uIStateMachine.FSM.Switch<EndLevelState>();

            // ── UI → Turrets (mirrors spawner lifecycle) ──────────────────────
            if (_turretsHandler != null)
            {
                _uIStateMachine.StartClicked  += _turretsHandler.Begin;
                _uIStateMachine.PauseClicked  += _turretsHandler.Pause;
                _uIStateMachine.ResumeClicked += _turretsHandler.Resume;

                _levelStateMachine.PlayerDied     += _turretsHandler.Pause;
                _levelStateMachine.LevelCompleted += _turretsHandler.Pause;
            }

            // ── Training screen pause hooks ───────────────────────────────────
            _gameplayTraining.ScreenShowed += _levelStateMachine.PauseLevel;
            _gameplayTraining.ScreenLeft   += _levelStateMachine.ResumeLevel;

            if (_turretsHandler != null)
            {
                _gameplayTraining.ScreenShowed += _turretsHandler.Pause;
                _gameplayTraining.ScreenLeft   += _turretsHandler.Resume;
            }
        }
    }
}