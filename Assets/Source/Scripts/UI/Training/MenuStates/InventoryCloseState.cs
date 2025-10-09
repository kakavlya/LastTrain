using LastTrain.Core.FSM;

namespace LastTrain.Training
{
    public class InventoryCloseState : IState
    {
        private readonly MenuTraining _menuTraining;
        public InventoryCloseState(MenuTraining c) => _menuTraining = c;

        public void Enter()
        {
            _menuTraining.DisableAllTrainingScreens();
            _menuTraining.InventoryScreen.SetActive(true);
            _menuTraining.InventoryLockerScreen.SetActive(true);
            _menuTraining.InventoryCloseTrainingScreen.SetActive(true);
            _menuTraining.BackFromInventoryButton.interactable = true;
            _menuTraining.SaveStep(MenuTrainingState.InventoryClose);
            _menuTraining.BackFromInventoryButton.onClick.AddListener(OnNext);
        }

        public void Exit()
        {
            _menuTraining.BackFromInventoryButton.onClick.RemoveListener(OnNext);
            _menuTraining.InventoryCloseTrainingScreen.SetActive(false);
        }

        private void OnNext() => _menuTraining.FSM.Switch<ChoseLevelOpenState>();
    }
}