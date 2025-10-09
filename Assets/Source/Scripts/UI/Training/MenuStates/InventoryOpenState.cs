using LastTrain.Core.FSM;

namespace LastTrain.Training
{
    public class InventoryOpenState : IState
    {
        private readonly MenuTraining _menuTraining;
        public InventoryOpenState(MenuTraining c) => _menuTraining = c;

        public void Enter()
        {
            _menuTraining.DisableAllTrainingScreens();
            _menuTraining.ShopScreen.SetActive(false);
            _menuTraining.InventoryOpenTrainingScreen.SetActive(true);
            _menuTraining.InventoryButton.interactable = true;
            _menuTraining.ShopButton.interactable = false;
            _menuTraining.SaveStep(MenuTrainingState.InventoryOpen);
            _menuTraining.InventoryButton.onClick.AddListener(OnNext);
        }

        public void Exit()
        {
            _menuTraining.InventoryButton.onClick.RemoveListener(OnNext);
            _menuTraining.InventoryOpenTrainingScreen.SetActive(false);
        }

        private void OnNext() => _menuTraining.FSM.Switch<InventoryDragState>();
    }
}