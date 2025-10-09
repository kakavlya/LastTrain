using System;
using LastTrain.Core;
using LastTrain.Core.FSM;
using UnityEngine;
using UnityEngine.UI;
using YG;

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

        public int SwitchingTrainingDelay => _switchingTrainingDelay;

        public int ShootingTrainingDelay => _shootingTrainingDelay;

        public int CameraTrainingDelay => _cameraTrainingDelay;

        public int PickUpTrainingDelay => _pickUpTrainingDelay;

        public GameObject StartTraining => _startTraining;

        public GameObject ComputerCameraTraining => _computerCameraTraining;

        public GameObject MobileCameraTraining => _mobileCameraTraining;

        public GameObject ComputerShootingTraining => _computerShootingTraining;

        public GameObject MobileShootingTraining => _mobileShootingTraining;

        public GameObject ComputerSwitchTraining => _computerSwitchTraining;

        public GameObject MobileSwitchTraining => _mobileSwitchTraining;

        public GameObject PickUpAmmunitionTraining => _pickUpAmmunitionTraining;

        public Button StartButton => _startButton;

        public Button ComputerCameraOkButton => _computerCameraOkButton;

        public Button MobileCameraOkButton => _mobileCameraOkButton;

        public Button ComputerShootingOkButton => _computerShootingOkButton;

        public Button MobileShootingOkButton => _mobileShootingOkButton;

        public Button ComputerSwitchOkButton => _computerSwitchOkButton;

        public Button MobileSwitchOkButton => _mobileSwitchOkButton;

        public Button PickUpOkButton => _pickUpOkButton;

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

        public void HideAll()
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

        public bool IsPC()
        {
            return PlatformDetector.Instance == null ||
                   PlatformDetector.Instance.CurrentControlScheme == PlatformDetector.ControlScheme.Computer;
        }

        public void InvokeScreenShowed() => ScreenShowed?.Invoke();
        public void InvokeScreenLeft() => ScreenLeft?.Invoke();
    }
}