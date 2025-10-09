using LastTrain.Core.FSM;

namespace LastTrain.Training
{
    public class ChoseLevelCloseState : IState
    {
        private readonly MenuTraining _menuTraining;
        public ChoseLevelCloseState(MenuTraining c) => _menuTraining = c;

        public void Enter()
        {
            _menuTraining.DisableAllTrainingScreens();
            _menuTraining.ChoseLevelCloseScreen.SetActive(true);
            _menuTraining.SaveStep(MenuTrainingState.ChoseLevelClose);
            _menuTraining.ChoseLevelCloseButton.onClick.AddListener(OnNext);
        }

        public void Exit()
        {
            _menuTraining.ChoseLevelCloseButton.onClick.RemoveAllListeners();
            _menuTraining.ChoseLevelCloseScreen.SetActive(false);
        }

        private void OnNext() => _menuTraining.FSM.Switch<EndState>();
    }
}