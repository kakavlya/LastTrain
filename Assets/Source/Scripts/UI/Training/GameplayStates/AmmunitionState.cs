using System.Collections;
using UnityEngine;

namespace LastTrain.Training
{
    public class AmmunitionState : GTState
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
            GT.PickUpOkButton.onClick.RemoveListener(OnOk);
            GT.PickUpAmmunitionTraining.SetActive(false);
            Left();
            base.Exit();
        }

        private IEnumerator Flow()
        {
            yield return new WaitForSeconds(GT.PickUpTrainingDelay);

            GT.PickUpAmmunitionTraining.SetActive(true);
            GT.PickUpOkButton.onClick.AddListener(OnOk);
            Showed();
        }

        private void OnOk()
        {
            GT.HideAll();
            Left();
        }
    }
}