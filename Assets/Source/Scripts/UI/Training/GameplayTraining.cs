using LastTrain.Core;
using LastTrain.Core.FSM;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LastTrain.Training
{
    public class GameplayTraining : TypeStateMachineMono
    {
        [Header("Training Screens")]
        [SerializeField] private GameObject _startTraining;
        [SerializeField] private GameObject _computerCameraTraining;
        [SerializeField] private GameObject _mobileCameraTraining;
        [SerializeField] private GameObject _computerShootingTraining;
        [SerializeField] private GameObject _mobileShootingTraining;
        [SerializeField] private GameObject _computerSwitchTraining;
        [SerializeField] private GameObject _mobileSwitchTraining;
        [SerializeField] private GameObject _pickUpAmmunitionTraining;

        [Header("Buttons")]
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _computerCameraOkButton;
        [SerializeField] private Button _mobileCameraOkButton;
        [SerializeField] private Button _computerShootingOkButton;
        [SerializeField] private Button _mobileShootingOkButton;
        [SerializeField] private Button _computerSwitchOkButton;
        [SerializeField] private Button _mobileSwitchOkButton;
        [SerializeField] private Button _pickUpOkButton;

        [Header("Delays (sec)")]
        [SerializeField] private int _cameraTrainingDelay = 5;
        [SerializeField] private int _shootingTrainingDelay = 7;
        [SerializeField] private int _switchingTrainingDelay = 10;
        [SerializeField] private int _pickUpTrainingDelay = 15;

        public event Action ScreenShowed;
        public event Action ScreenLeft;

        public sealed class StartLevel { }
        public sealed class CameraMovement { }
        public sealed class Shooting { }
        public sealed class SwitchWeapon { }
        public sealed class Ammunition { }

        private void Start()
        {
            if (TrainingHandler.Instance != null && !TrainingHandler.Instance.IsDoneGameplayTraining)
            {
                Register<StartLevel>(new StartState(this, _startTraining, _startButton));
                Register<CameraMovement>(new VariantDelayedState<CameraMovement, Shooting>(
                    owner: this,
                    delay: _cameraTrainingDelay,
                    pcScreen: _computerCameraTraining, pcOk: _computerCameraOkButton,
                    mobileScreen: _mobileCameraTraining, mobileOk: _mobileCameraOkButton));

                Register<Shooting>(new VariantDelayedState<Shooting, SwitchWeapon>(
                    owner: this,
                    delay: _shootingTrainingDelay,
                    pcScreen: _computerShootingTraining, pcOk: _computerShootingOkButton,
                    mobileScreen: _mobileShootingTraining, mobileOk: _mobileShootingOkButton));

                Register<SwitchWeapon>(new VariantDelayedState<SwitchWeapon, Ammunition>(
                    owner: this,
                    delay: _switchingTrainingDelay,
                    pcScreen: _computerSwitchTraining, pcOk: _computerSwitchOkButton,
                    mobileScreen: _mobileSwitchTraining, mobileOk: _mobileSwitchOkButton));

                Register<Ammunition>(new SingleDelayedState<Ammunition, StartLevel>(
                    owner: this,
                    delay: _pickUpTrainingDelay,
                    screen: _pickUpAmmunitionTraining,
                    okButton: _pickUpOkButton,
                    switchToNext: false));

                Switch<StartLevel>();
            }
        }

        private void HideAll()
        {
            _startTraining.SetActive(false);
            _computerCameraTraining.SetActive(false);
            _mobileCameraTraining.SetActive(false);
            _computerShootingTraining.SetActive(false);
            _mobileShootingTraining.SetActive(false);
            _computerSwitchTraining.SetActive(false);
            _mobileSwitchTraining.SetActive(false);
            _pickUpAmmunitionTraining.SetActive(false);
        }

        // ===== состояния =====

        private sealed class StartState : IState
        {
            private readonly GameplayTraining _gameplayTraining;
            private readonly GameObject _screen;
            private readonly Button _start;

            public StartState(GameplayTraining gameplayTraining, GameObject screen, Button start)
            { _gameplayTraining = gameplayTraining; _screen = screen; _start = start; }

            public void Enter()
            {
                _gameplayTraining.HideAll();
                _screen.SetActive(true);
                _gameplayTraining.ScreenShowed?.Invoke();
                _start.onClick.AddListener(OnNext);
            }

            public void Exit()
            {
                _start.onClick.RemoveListener(OnNext);
                _screen.SetActive(false);
                _gameplayTraining.ScreenLeft?.Invoke();
            }

            private void OnNext() => _gameplayTraining.Switch<CameraMovement>();
        }

        /// ПК/мобайл вариант, с задержкой и OK-кнопками
        private sealed class VariantDelayedState<TSelf, TNext> : IState
            where TSelf : class where TNext : class
        {
            private readonly GameplayTraining _o;
            private readonly int _delay;
            private readonly GameObject _pc, _mobile;
            private readonly Button _pcOk, _mobileOk;

            private Coroutine _routine;

            public VariantDelayedState(GameplayTraining owner, int delay,
                GameObject pcScreen, Button pcOk,
                GameObject mobileScreen, Button mobileOk)
            {
                _o = owner; _delay = delay;
                _pc = pcScreen; _pcOk = pcOk;
                _mobile = mobileScreen; _mobileOk = mobileOk;
            }

            public void Enter()
            {
                _o.HideAll();
                _routine = _o.StartCoroutine(Flow());
            }

            public void Exit()
            {
                if (_routine != null) { _o.StopCoroutine(_routine); _routine = null; }
                if (_pcOk != null) _pcOk.onClick.RemoveListener(OnOk);
                if (_mobileOk != null) _mobileOk.onClick.RemoveListener(OnOk);
                if (_pc) _pc.SetActive(false);
                if (_mobile) _mobile.SetActive(false);
                _o.ScreenLeft?.Invoke();
            }

            private IEnumerator Flow()
            {
                yield return new WaitForSeconds(_delay);

                bool isPC = PlatformDetector.Instance == null ||
                            PlatformDetector.Instance.CurrentControlScheme == PlatformDetector.ControlScheme.Computer;

                if (isPC)
                {
                    if (_pc) _pc.SetActive(true);
                    _pcOk?.onClick.AddListener(OnOk);
                }
                else
                {
                    if (_mobile) _mobile.SetActive(true);
                    _mobileOk?.onClick.AddListener(OnOk);
                }

                _o.ScreenShowed?.Invoke();
            }

            private void OnOk() => _o.Switch<TNext>();
        }

        private sealed class SingleDelayedState<TSelf, TNext> : IState
            where TSelf : class where TNext : class
        {
            private readonly GameplayTraining _gameplayTraining;
            private readonly int _delay;
            private readonly GameObject _screen;
            private readonly Button _okButton;
            private readonly bool _switchToNext;

            private Coroutine _routine;

            public SingleDelayedState(GameplayTraining owner, int delay, GameObject screen, Button okButton, bool switchToNext)
            {
                _gameplayTraining = owner;
                _delay = delay;
                _screen = screen;
                _okButton = okButton;
                _switchToNext = switchToNext;
            }

            public void Enter()
            {
                _gameplayTraining.HideAll();
                _routine = _gameplayTraining.StartCoroutine(Flow());
            }

            public void Exit()
            {
                if (_routine != null)
                {
                    _gameplayTraining.StopCoroutine(_routine); _routine = null;
                }

                _okButton?.onClick.RemoveListener(OnOk);

                if (_screen)
                    _screen.SetActive(false);
                _gameplayTraining.ScreenLeft?.Invoke();
            }

            private IEnumerator Flow()
            {
                yield return new WaitForSeconds(_delay);

                if (_screen)
                    _screen.SetActive(true);

                _okButton?.onClick.AddListener(OnOk);
                _gameplayTraining.ScreenShowed?.Invoke();
            }

            private void OnOk()
            {
                if (_switchToNext)
                    _gameplayTraining.Switch<TNext>();
                else
                {
                    _gameplayTraining.HideAll();
                    _gameplayTraining.ScreenLeft?.Invoke();
                }
            }
        }

    }
}
