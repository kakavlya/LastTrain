using Assets.Source.Scripts.Enemies;
using Assets.Source.Scripts.Weapons;
using Level;
using Player;
using UnityEngine;
using static UIStateMachine;

public class CompositionRoot : MonoBehaviour
{
    [SerializeField] private UIStateMachine _uIStateMachine;
    [SerializeField] private LevelStateMachine _levelStateMachine;
    [SerializeField] private GameplayTraining _gameplayTraining;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private Transform _player;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private TrainMovement _trainMovement;
    [SerializeField] private LevelGenerator _levelGenerator;
    [SerializeField] private WeaponsHandler _weaponHandler;
    [SerializeField] private WeaponRotator _weaponRotator;
    [SerializeField] private AimingTargetProvider _aimingTargetProvider;
    [SerializeField] private UICursorFollower _uiCursorFollower;
    [SerializeField] private ParticlePool _particlePool;
    [SerializeField] private PickableAmmunitionPool _pickableAmmunitionPool;
    [SerializeField] private ProjectilePool _projectilePool;
    [SerializeField] private CameraFollower _cameraFollower;
    [SerializeField] private LevelProgress _levelProgress;
    [SerializeField] private LevelElementsCreator _levelElementsCreator;
    [SerializeField] private PickableAmmunitionSpawner _pickableAmmunitionSpawner;
    [SerializeField] private EnemyPool _enemyPool;

    private void Awake()
    {
        _enemySpawner.Init();
        _aimingTargetProvider.Init();
        _uiCursorFollower.Init();
        _weaponRotator.Init();
        _weaponHandler.Init();
        _trainMovement.Init();
        _levelElementsCreator.Init();
        _pickableAmmunitionPool.Init();
        _pickableAmmunitionSpawner.Init();
        _levelGenerator.Init();
        _particlePool.Init();
        _projectilePool.Init();
        _levelProgress.Init();


        _levelStateMachine.Construct(_enemySpawner, _player, _playerHealth, _trainMovement, _levelProgress);

        _uIStateMachine.StartClicked += _levelStateMachine.StartLevel;
        _uIStateMachine.RestartClicked += _levelStateMachine.RestartLevel;
        _uIStateMachine.PauseClicked += _levelStateMachine.PauseLevel;
        _uIStateMachine.ResumeClicked += _levelStateMachine.ResumeLevel;
        _uIStateMachine.MenuClicked += _levelStateMachine.ReturnToMenu;
        _uIStateMachine.SwitchState(UIState.LevelStart);

        _levelStateMachine.PlayerDied += () => _uIStateMachine.SwitchState(UIState.GameOver);
        _levelStateMachine.LevelCompleted += () => _uIStateMachine.SwitchState(UIState.EndLevel);

        _gameplayTraining.ScreenShowed += _levelStateMachine.StopGameplay;
        _gameplayTraining.ScreenLeft += _levelStateMachine.ResumeGameplay;
    }
}
