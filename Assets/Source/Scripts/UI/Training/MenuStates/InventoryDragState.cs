using LastTrain.Core.FSM;

namespace LastTrain.Training
{
    public class InventoryDragState : IState
    {
        private readonly MenuTraining _menuTraining;
        public InventoryDragState(MenuTraining c) => _menuTraining = c;

        public void Enter()
        {
            _menuTraining.DisableAllTrainingScreens();
            _menuTraining.InventoryScreen.SetActive(true);
            _menuTraining.InventoryDragTrainingScreen.SetActive(true);
            _menuTraining.BackFromInventoryButton.interactable = false;
            _menuTraining.SaveStep(MenuTrainingState.InventoryDrag);
            _menuTraining.SignUpPlayerInventorySlots(OnFilled);
        }

        public void Exit()
        {
            _menuTraining.UnsubscribeInventorySlots(OnFilled);
            _menuTraining.InventoryDragTrainingScreen.SetActive(false);
        }

        private void OnFilled()
        {
            _menuTraining.FSM.Switch<InventoryCloseState>();
            _menuTraining.UnsubscribeInventorySlots(OnFilled);
        }
    }
}