using System;
using System.Collections;
using LastTrain.Core;
using LastTrain.Core.FSM;
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

        private void Start()
        {
            if (TrainingHandler.Instance != null && !TrainingHandler.Instance.IsDoneGameplayTraining)
            {
                FSM.Register(new StartState(this));
                FSM.Register(new CameraMovementState(this));
                FSM.Register(new ShootingState(this));
                FSM.Register(new SwitchWeaponState(this));
                FSM.Register(new AmmunitionState(this));

                FSM.Switch<StartState>();
            }
        }

        internal void HideAll()
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

        internal static bool IsPC()
        {
            return PlatformDetector.Instance == null ||
                   PlatformDetector.Instance.CurrentControlScheme == PlatformDetector.ControlScheme.Computer;
        }

        private abstract class GTState : IState
        {
            protected readonly GameplayTraining GT;
            protected Coroutine DelayRoutine;

            protected GTState(GameplayTraining gt)
            {
                GT = gt;
            }

            public virtual void Enter() { }

            public virtual void Exit()
            {
                if (DelayRoutine != null)
                {
                    GT.StopCoroutine(DelayRoutine);
                    DelayRoutine = null;
                }
            }

            protected void StartDelay(IEnumerator routine)
            {
                if (DelayRoutine != null)
                {
                    GT.StopCoroutine(DelayRoutine);
                }

                DelayRoutine = GT.StartCoroutine(routine);
            }

            protected void Showed()
            {
                GT.ScreenShowed?.Invoke();
            }

            protected void Left()
            {
                GT.ScreenLeft?.Invoke();
            }
        }

        private sealed class StartState : GTState
        {
            public StartState(GameplayTraining gt)
                : base(gt)
            { }

            public override void Enter()
            {
                GT.HideAll();
                GT._startTraining.SetActive(true);
                Showed();
                GT._startButton.onClick.AddListener(OnNext);
            }

            public override void Exit()
            {
                GT._startButton.onClick.RemoveListener(OnNext);
                GT._startTraining.SetActive(false);
                Left();
                base.Exit();
            }

            private void OnNext()
            {
                GT.FSM.Switch<CameraMovementState>();
            }
        }

        private sealed class CameraMovementState : GTState
        {
            public CameraMovementState(GameplayTraining gt)
                : base(gt)
            { }

            public override void Enter()
            {
                GT.HideAll();
                StartDelay(Flow());
            }

            public override void Exit()
            {
                GT._computerCameraOkButton.onClick.RemoveListener(OnOk);
                GT._mobileCameraOkButton.onClick.RemoveListener(OnOk);
                GT._computerCameraTraining.SetActive(false);
                GT._mobileCameraTraining.SetActive(false);
                Left();
                base.Exit();
            }

            private IEnumerator Flow()
            {
                yield return new WaitForSeconds(GT._cameraTrainingDelay);

                if (IsPC())
                {
                    GT._computerCameraTraining.SetActive(true);
                    GT._computerCameraOkButton.onClick.AddListener(OnOk);
                }
                else
                {
                    GT._mobileCameraTraining.SetActive(true);
                    GT._mobileCameraOkButton.onClick.AddListener(OnOk);
                }

                Showed();
            }

            private void OnOk()
            {
                GT.FSM.Switch<ShootingState>();
            }
        }

        private sealed class ShootingState : GTState
        {
            public ShootingState(GameplayTraining gt)
                : base(gt)
            { }

            public override void Enter()
            {
                GT.HideAll();
                StartDelay(Flow());
            }

            public override void Exit()
            {
                GT._computerShootingOkButton.onClick.RemoveListener(OnOk);
                GT._mobileShootingOkButton.onClick.RemoveListener(OnOk);
                GT._computerShootingTraining.SetActive(false);
                GT._mobileShootingTraining.SetActive(false);
                Left();
                base.Exit();
            }

            private IEnumerator Flow()
            {
                yield return new WaitForSeconds(GT._shootingTrainingDelay);

                if (IsPC())
                {
                    GT._computerShootingTraining.SetActive(true);
                    GT._computerShootingOkButton.onClick.AddListener(OnOk);
                }
                else
                {
                    GT._mobileShootingTraining.SetActive(true);
                    GT._mobileShootingOkButton.onClick.AddListener(OnOk);
                }

                Showed();
            }

            private void OnOk()
            {
                GT.FSM.Switch<SwitchWeaponState>();
            }
        }

        private sealed class SwitchWeaponState : GTState
        {
            public SwitchWeaponState(GameplayTraining gt)
                : base(gt)
            { }

            public override void Enter()
            {
                GT.HideAll();
                StartDelay(Flow());
            }

            public override void Exit()
            {
                GT._computerSwitchOkButton.onClick.RemoveListener(OnOk);
                GT._mobileSwitchOkButton.onClick.RemoveListener(OnOk);
                GT._computerSwitchTraining.SetActive(false);
                GT._mobileSwitchTraining.SetActive(false);
                Left();
                base.Exit();
            }

            private IEnumerator Flow()
            {
                yield return new WaitForSeconds(GT._switchingTrainingDelay);

                if (IsPC())
                {
                    GT._computerSwitchTraining.SetActive(true);
                    GT._computerSwitchOkButton.onClick.AddListener(OnOk);
                }
                else
                {
                    GT._mobileSwitchTraining.SetActive(true);
                    GT._mobileSwitchOkButton.onClick.AddListener(OnOk);
                }

                Showed();
            }

            private void OnOk()
            {
                GT.FSM.Switch<AmmunitionState>();
            }
        }

        private sealed class AmmunitionState : GTState
        {
            public AmmunitionState(GameplayTraining gt)
                : base(gt)
            { }

            public override void Enter()
            {
                GT.HideAll();
                StartDelay(Flow());
            }

            public override void Exit()
            {
                GT._pickUpOkButton.onClick.RemoveListener(OnOk);
                GT._pickUpAmmunitionTraining.SetActive(false);
                Left();
                base.Exit();
            }

            private IEnumerator Flow()
            {
                yield return new WaitForSeconds(GT._pickUpTrainingDelay);

                GT._pickUpAmmunitionTraining.SetActive(true);
                GT._pickUpOkButton.onClick.AddListener(OnOk);
                Showed();
            }

            private void OnOk()
            {
                GT.HideAll();
                Left();
            }
        }
    }
}