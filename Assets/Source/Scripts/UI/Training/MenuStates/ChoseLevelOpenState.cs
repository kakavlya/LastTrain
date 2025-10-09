using LastTrain.Core.FSM;

namespace LastTrain.Training
{
    public class ChoseLevelOpenState : IState
    {
        private readonly MenuTraining _menuTraining;
        public ChoseLevelOpenState(MenuTraining c) => _menuTraining = c;

        public void Enter()
        {
            _menuTraining.DisableAllTrainingScreens();
            _menuTraining.InventoryScreen.SetActive(false);
            _menuTraining.InventoryLockerScreen.SetActive(false);
            _menuTraining.InventoryCloseTrainingScreen.SetActive(false);
            _menuTraining.InventoryButton.interactable = false;
            _menuTraining.ShopButton.interactable = false;
            _menuTraining.ChoseLevelButton.interactable = true;
            _menuTraining.SaveStep(MenuTrainingState.ChoseLevelOpen);
            _menuTraining.BackFromChoseLevelButton.onClick.AddListener(OnNext);
            _menuTraining.ChoseLevelOpenScreen.SetActive(true);
        }

        public void Exit()
        {
            _menuTraining.BackFromChoseLevelButton.onClick.RemoveListener(OnNext);
            _menuTraining.ChoseLevelOpenScreen.SetActive(false);
        }

        private void OnNext() => _menuTraining.FSM.Switch<ChoseLevelCloseState>();
    }
}