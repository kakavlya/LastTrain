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
    [SerializeField] private EnemySpawner _spawner;
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

    private void Awake()
    {
        _aimingTargetProvider.Init();
        _uiCursorFollower.Init();
        _weaponHandler.Init();
        _weaponRotator.Init();
        _trainMovement.Init();
        _levelGenerator.Init();
        _particlePool.Init();
        _pickableAmmunitionPool.Init();
        _projectilePool.Init();
        _levelProgress.Init();
        _cameraFollower.Init();


        _levelStateMachine.Construct(_spawner, _player, _playerHealth, _trainMovement, _levelGenerator, _levelProgress);
        _uIStateMachine.Construct(_levelStateMachine);

        _uIStateMachine.StartClicked += _levelStateMachine.StartLevel;
        _uIStateMachine.RestartClicked += _levelStateMachine.RestartLevel;
        _uIStateMachine.PauseClicked += _levelStateMachine.PauseLevel;
        _uIStateMachine.ResumeClicked += _levelStateMachine.ResumeLevel;

        _levelStateMachine.PlayerDied += () => _uIStateMachine.SwitchState(UIState.GameOver);
        _levelStateMachine.LevelCompleted += () => _uIStateMachine.SwitchState(UIState.EndLevel);

        _uIStateMachine.SwitchState(UIState.LevelStart);
        //_spawner.Init();
    }
}
