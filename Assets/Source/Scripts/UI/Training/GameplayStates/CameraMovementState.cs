using System.Collections;
using UnityEngine;

namespace LastTrain.Training
{
    public class CameraMovementState : GTState
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
            GT.ComputerCameraOkButton.onClick.RemoveListener(OnOk);
            GT.MobileCameraOkButton.onClick.RemoveListener(OnOk);
            GT.ComputerCameraTraining.SetActive(false);
            GT.MobileCameraTraining.SetActive(false);
            Left();
            base.Exit();
        }

        private IEnumerator Flow()
        {
            yield return new WaitForSeconds(GT.CameraTrainingDelay);

            if (GT.IsPC())
            {
                GT.ComputerCameraTraining.SetActive(true);
                GT.ComputerCameraOkButton.onClick.AddListener(OnOk);
            }
            else
            {
                GT.MobileCameraTraining.SetActive(true);
                GT.MobileCameraOkButton.onClick.AddListener(OnOk);
            }

            Showed();
        }

        private void OnOk()
        {
            GT.FSM.Switch<ShootingState>();
        }
    }
}