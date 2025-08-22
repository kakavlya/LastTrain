using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class GameplayTraining : MonoBehaviour
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

    private int _cameraTrainingDelay = 5;
    private int _shootingTrainingDelay = 7;
    private int _switchingTrainingDelay = 10;
    private int _pickUpTrainingDelay = 15;

    public event Action ScreenShowed;
    public event Action ScreenLeft;

    private void Start()
    {
        if (TrainingHandler.Instance.IsDoneGameplayTraining == false)
        {
            SwitchTrainingWindows(TrainingState.StartLevel);
            _startButton.onClick.AddListener(OnStartButtonClick);
            _computerCameraOkButton.onClick.AddListener(OnCameraTrainingOkButton);
            _mobileCameraOkButton.onClick.AddListener(OnCameraTrainingOkButton);
            _computerShootingOkButton.onClick.AddListener(OnShootinTrainingButton);
            _mobileShootingOkButton.onClick.AddListener(OnShootinTrainingButton);
            _computerSwitchOkButton.onClick.AddListener(OnSwitchTrainingOkButton);
            _mobileSwitchOkButton.onClick.AddListener(OnSwitchTrainingOkButton);
            _pickUpOkButton.onClick.AddListener(OnPickUpAmmoOkButton);
        }
    }

    public enum TrainingState
    {
        StartLevel,
        CameraMovement,
        Shooting,
        SwitchWeapon,
        Ammunition
    }

    private void SwitchTrainingWindows(TrainingState trainingState)
    {
        DisableAllScreens();

        switch (trainingState)
        {
            case TrainingState.StartLevel:
                _startTraining.SetActive(true);
                ScreenShowed?.Invoke();
                break;

            case TrainingState.CameraMovement:
                StartCoroutine(ShowCameraMovementTrainingAfterDelay());
                ScreenLeft?.Invoke();
                break;

            case TrainingState.Shooting:
                StartCoroutine(ShowShootingTrainingAfterDelay());
                ScreenLeft?.Invoke();
                break;

            case TrainingState.SwitchWeapon:
                StartCoroutine(ShowWeaponSwitchTrainingAfterDelay());
                ScreenLeft?.Invoke();
                break;

            case TrainingState.Ammunition:
                StartCoroutine(ShowPickUpAmmunitionTraining());
                ScreenLeft?.Invoke();
                break;
        }
    }

    private void OnStartButtonClick()
    {
        SwitchTrainingWindows(TrainingState.CameraMovement);
    }

    private void OnCameraTrainingOkButton()
    {
        SwitchTrainingWindows(TrainingState.Shooting);
    }

    private void OnShootinTrainingButton()
    {
        SwitchTrainingWindows(TrainingState.SwitchWeapon);
    }

    private void OnSwitchTrainingOkButton()
    {
        SwitchTrainingWindows(TrainingState.Ammunition);
    }

    private void OnPickUpAmmoOkButton()
    {
        DisableAllScreens();
        ScreenLeft?.Invoke();
    }

    private IEnumerator ShowCameraMovementTrainingAfterDelay()
    {
        yield return new WaitForSeconds(_cameraTrainingDelay);

        if (PlatformDetector.Instance.CurrentControlScheme == PlatformDetector.ControlScheme.Computer)
        {
            _computerCameraTraining.SetActive(true);
        }
        else if (PlatformDetector.Instance.CurrentControlScheme == PlatformDetector.ControlScheme.Mobile)
        {
            _mobileCameraTraining.SetActive(true);
        }

        ScreenShowed?.Invoke();
    }

    private IEnumerator ShowShootingTrainingAfterDelay()
    {
        yield return new WaitForSeconds(_shootingTrainingDelay);

        if (PlatformDetector.Instance.CurrentControlScheme == PlatformDetector.ControlScheme.Computer)
        {
            _computerShootingTraining.SetActive(true);
        }
        else if (PlatformDetector.Instance.CurrentControlScheme == PlatformDetector.ControlScheme.Mobile)
        {
            _mobileShootingTraining.SetActive(true);
        }

        ScreenShowed?.Invoke();
    }

    private IEnumerator ShowWeaponSwitchTrainingAfterDelay()
    {
        yield return new WaitForSeconds(_switchingTrainingDelay);

        if (PlatformDetector.Instance.CurrentControlScheme == PlatformDetector.ControlScheme.Computer)
        {
            _computerSwitchTraining.SetActive(true);
        }
        else if (PlatformDetector.Instance.CurrentControlScheme == PlatformDetector.ControlScheme.Mobile)
        {
            _mobileSwitchTraining.SetActive(true);
        }

        ScreenShowed?.Invoke();
    }

    private IEnumerator ShowPickUpAmmunitionTraining()
    {
        yield return new WaitForSeconds(_pickUpTrainingDelay);

        _pickUpAmmunitionTraining.SetActive(true);
        ScreenShowed?.Invoke();
    }

    private void DisableAllScreens()
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
}
