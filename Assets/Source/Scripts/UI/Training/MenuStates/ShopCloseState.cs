using LastTrain.Core.FSM;

namespace LastTrain.Training
{
    public class ShopCloseState : IState
    {
        private readonly MenuTraining _menuTraining;
        public ShopCloseState(MenuTraining c) => _menuTraining = c;

        public void Enter()
        {
            _menuTraining.DisableAllTrainingScreens();
            _menuTraining.ShopScreen.SetActive(true);
            _menuTraining.ShopBackTrainingScreen.SetActive(true);
            _menuTraining.BackFromShopButton.interactable = true;
            _menuTraining.SaveStep(MenuTrainingState.ShopClose);
            _menuTraining.BackFromShopButton.onClick.AddListener(OnNext);
        }

        public void Exit()
        {
            _menuTraining.BackFromShopButton.onClick.RemoveListener(OnNext);
            _menuTraining.ShopBackTrainingScreen.SetActive(false);
        }

        private void OnNext() => _menuTraining.FSM.Switch<InventoryOpenState>();
    }
}