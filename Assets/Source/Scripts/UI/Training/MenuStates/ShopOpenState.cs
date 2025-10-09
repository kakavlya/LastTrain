using LastTrain.Core.FSM;

namespace LastTrain.Training
{
    public class ShopOpenState : IState
    {
        private readonly MenuTraining _menuTraining;
        public ShopOpenState(MenuTraining c) => _menuTraining = c;

        public void Enter()
        {
            _menuTraining.DisableAllTrainingScreens();
            _menuTraining.ShopOpenTrainingScreen.SetActive(true);
            _menuTraining.ShopButton.interactable = true;
            _menuTraining.StartLevelButton.interactable = false;
            _menuTraining.InventoryButton.interactable = false;
            _menuTraining.ChoseLevelButton.interactable = false;
            _menuTraining.RewardButton.interactable = false;
            _menuTraining.SaveStep(MenuTrainingState.ShopOpen);
            _menuTraining.ShopButton.onClick.AddListener(OnNext);
        }

        public void Exit()
        {
            _menuTraining.ShopButton.onClick.RemoveListener(OnNext);
            _menuTraining.ShopOpenTrainingScreen.SetActive(false);
        }

        private void OnNext() => _menuTraining.FSM.Switch<ShopInfoState>();
    }
}