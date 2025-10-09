using LastTrain.Core.FSM;
using YG;

namespace LastTrain.Training
{
    public class EndState : IState
    {
        private readonly MenuTraining _menuTraining;
        public EndState(MenuTraining c) => _menuTraining = c;

        public void Enter()
        {
            _menuTraining.DisableAllTrainingScreens();
            _menuTraining.SetAllMainInteractable(true);
            _menuTraining.UnlockAllUpgradeButtons();
            _menuTraining.SaveStep(MenuTrainingState.End);
            YG2.saves.IsDoneMenuTraining = true;
            YG2.SaveProgress();
        }

        public void Exit() { }
    }
}