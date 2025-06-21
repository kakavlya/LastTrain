using Assets.Source.Scripts.Enemies;
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

    private void Awake()
    {
        _levelStateMachine.Construct(_spawner, _player, _playerHealth, _trainMovement, _levelGenerator);
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
