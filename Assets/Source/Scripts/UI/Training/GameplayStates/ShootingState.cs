using System.Collections;
using UnityEngine;

namespace LastTrain.Training
{
    public class ShootingState : GTState
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
            GT.ComputerShootingOkButton.onClick.RemoveListener(OnOk);
            GT.MobileShootingOkButton.onClick.RemoveListener(OnOk);
            GT.ComputerShootingTraining.SetActive(false);
            GT.MobileShootingTraining.SetActive(false);
            Left();
            base.Exit();
        }

        private IEnumerator Flow()
        {
            yield return new WaitForSeconds(GT.ShootingTrainingDelay);

            if (GT.IsPC())
            {
                GT.ComputerShootingTraining.SetActive(true);
                GT.ComputerShootingOkButton.onClick.AddListener(OnOk);
            }
            else
            {
                GT.MobileShootingTraining.SetActive(true);
                GT.MobileShootingOkButton.onClick.AddListener(OnOk);
            }

            Showed();
        }

        private void OnOk()
        {
            GT.FSM.Switch<SwitchWeaponState>();
        }
    }
}