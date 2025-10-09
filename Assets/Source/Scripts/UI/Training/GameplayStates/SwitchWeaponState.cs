using System.Collections;
using UnityEngine;

namespace LastTrain.Training
{
    public class SwitchWeaponState : GTState
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
            GT.ComputerSwitchOkButton.onClick.RemoveListener(OnOk);
            GT.MobileSwitchOkButton.onClick.RemoveListener(OnOk);
            GT.ComputerSwitchTraining.SetActive(false);
            GT.MobileSwitchTraining.SetActive(false);
            Left();
            base.Exit();
        }

        private IEnumerator Flow()
        {
            yield return new WaitForSeconds(GT.SwitchingTrainingDelay);

            if(GT.IsPC())
            {
                GT.ComputerSwitchTraining.SetActive(true);
                GT.ComputerSwitchOkButton.onClick.AddListener(OnOk);
            }
            else
            {
                GT.MobileSwitchTraining.SetActive(true);
                GT.MobileSwitchOkButton.onClick.AddListener(OnOk);
            }

            Showed();
        }

        private void OnOk()
        {
            GT.FSM.Switch<AmmunitionState>();
        }
    }
}