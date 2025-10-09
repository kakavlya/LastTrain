using LastTrain.Core.FSM;

namespace LastTrain.Training
{
    public class StartStateMenu : IState
    {
        private readonly MenuTraining _menuTraining;

        public StartStateMenu(MenuTraining c) => _menuTraining = c;

        public void Enter()
        {
            _menuTraining.DisableAllTrainingScreens();
            _menuTraining.SetAllMainInteractable(false);
            _menuTraining.StartTrainingScreen.SetActive(true);
            _menuTraining.SaveStep(MenuTrainingState.Start);
            _menuTraining.StartTrainingOkButton.onClick.AddListener(OnNext);
        }

        public void Exit()
        {
            _menuTraining.StartTrainingOkButton.onClick.RemoveListener(OnNext);
            _menuTraining.StartTrainingScreen.SetActive(false);
        }

        private void OnNext() => _menuTraining.FSM.Switch<ShopOpenState>();
    }
}